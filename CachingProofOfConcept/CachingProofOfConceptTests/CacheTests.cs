using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DataAccess.Tests
{
	// To run unit tests
	//	- download Redis for Windows from https://github.com/dmajkic/redis/downloads
	//	- run redis-server.exe
	public class CacheTests
	{
		private RedisAppCache _cache;

		[SetUp]
		public void Setup()
		{
			var redisCacheOptions = new RedisCacheOptions() { Hostname = "localhost", Prefix = "cacheTests", Port = "6379", };

			_cache = new RedisAppCache(redisCacheOptions);
		}

		[Test]
		public async Task GetFromCache()
		{
			var input = new Country() {Id = 1, Name = "Canada"};
			await _cache.SetAsync<Country>($"{CacheKeys.CountriesKey}:{input.Id}", input);
			
			var output = await _cache.GetAsync<Country>($"{CacheKeys.CountriesKey}:{input.Id}");

			Assert.AreEqual(input.Name, output.Name);
		}

		[Test]
		public async Task GetListFromCache()
		{
			var input = new List<Country>
			{
				new Country() {Id = 1, Name = "Canada"}, 
				new Country() {Id = 2, Name = "United States"}
			};
			await _cache.SetAsync<IEnumerable<Country>>(CacheKeys.CountriesKey, input);
			var output = await _cache.GetAsync<IEnumerable<Country>>(CacheKeys.CountriesKey);
			Assert.AreEqual(input.Count, output.Count());
		}

		[Test]
        [TestCase(true)]
        [TestCase(false)]
		public async Task KeyExists(bool keyExist)
		{
			await _cache.RemoveAsync(CacheKeys.CountriesKey);
			if (keyExist)
			{
				await _cache.SetAsync<IEnumerable<Country>>(CacheKeys.CountriesKey, null);
			}
			var exists = await _cache.KeyExistsAsync(CacheKeys.CountriesKey);
			Assert.AreEqual(keyExist, exists);
		}

		[Test]
		public async Task GetAndRemove()
		{
			await _cache.SetAsync<IEnumerable<Country>>(CacheKeys.CountriesKey, new List<Country>());
			
			var output = await _cache.GetAndRemoveAsync<IEnumerable<Country>>(CacheKeys.CountriesKey);
			Assert.NotNull(output);

			var existsAfterRemove = await _cache.KeyExistsAsync(CacheKeys.CountriesKey);
			Assert.AreEqual(false, existsAfterRemove);
		}

		[Test]
		public async Task ExpireCache()
		{
			await _cache.SetAsync<IEnumerable<Country>>(CacheKeys.CountriesKey, new List<Country>(), TimeSpan.FromSeconds(1));

			var exists= await _cache.KeyExistsAsync(CacheKeys.CountriesKey);
			Assert.AreEqual(true, exists);

			await Task.Delay(TimeSpan.FromSeconds(2));
			var existsAfterRemove = await _cache.KeyExistsAsync(CacheKeys.CountriesKey);
			Assert.AreEqual(false, existsAfterRemove);
		}

		[Test]
		public async Task CreateSet()
		{
			await _cache.RemoveAsync(CacheKeys.LogsKey);
			for (int i = 0; i < 10; i++)
			{
				await _cache.AppendToSetAsync(CacheKeys.LogsKey, new LogEntry() {Id = i, Message = $"info message {i}"});
			}

			var logs = await _cache.GetSetMembersAsync<LogEntry>(CacheKeys.LogsKey);
			Assert.AreEqual(10, logs.ToList().Count);
			
			await _cache.RemoveAsync(CacheKeys.LogsKey);
			var logsAfterRemove = await _cache.GetSetMembersAsync<LogEntry>(CacheKeys.LogsKey);
			Assert.AreEqual(0, logsAfterRemove.ToList().Count);
		}
	}
}