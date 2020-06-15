using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace DataAccess.Tests
{
	// To run unit tests
	//	- deploy database project
	//	- download Redis for Windows from https://github.com/dmajkic/redis/downloads
	//	- run redis-server.exe
	public class CacheCountryTests
	{
		private SqlCachingCountryRepository _repo;
		private RedisAppCache _cache;

		[SetUp]
		public void Setup()
		{
			string connectionString = "Server=.\\sqlexpress;Database=Database1;Integrated Security=true";
			var redisCacheOptions = new RedisCacheOptions() { Hostname = "localhost", Prefix = "cacheCountryTests", Port = "6379", };
			var cacheTtlOptions = Options.Create(new CacheTtlOptions() { CountriesTtlInSeconds = 10 });

			_cache = new RedisAppCache(redisCacheOptions);
			_repo = new SqlCachingCountryRepository(new SqlServerDataContext(connectionString, 200, NullLogger<SqlServerDataContext>.Instance), _cache, cacheTtlOptions, NullLogger<SqlCachingCountryRepository>.Instance);
		}

		[Test]
		public async Task GetFromCache()
		{
			await _cache.RemoveAsync(CacheKeys.CountriesKey);
			var countries = await _repo.Get();
			Assert.AreEqual(2, countries.ToList().Count);
		}

		[Test]
		public async Task CacheDown_GetFromDatabase()
		{
			string connectionString = "Server=.\\sqlexpress;Database=Database1;Integrated Security=true";
			var redisCacheOptions = new RedisCacheOptions() { Hostname = "incorrect_host", Prefix = "cacheCountryTests", Port = "6379", };
			var cacheTtlOptions = Options.Create(new CacheTtlOptions() { CountriesTtlInSeconds = 10});

			var redisAppCache = new RedisAppCache(redisCacheOptions);
			var repo = new SqlCachingCountryRepository(new SqlServerDataContext(connectionString, 200, NullLogger<SqlServerDataContext>.Instance),
				redisAppCache, cacheTtlOptions, NullLogger<SqlCachingCountryRepository>.Instance);
			var countries = await repo.Get();
			Assert.AreEqual(2, countries.ToList().Count);
		}

		[Test]
		public async Task CacheAsideOrLazyLoading()
		{
			await _cache.RemoveAsync(CacheKeys.CountriesKey);
			var countries = await _repo.Get();
			Assert.AreEqual(2, countries.ToList().Count);

			await _repo.AddCountryCacheAside(new Country() {Name = "UK"});

			var countriesAfterInsert = await _repo.Get();
			Assert.AreEqual(3, countriesAfterInsert.ToList().Count);
			
			await _repo.DeleteCountryCacheAside("UK");
		}

		[Test]
		public async Task WriteThrough()
		{
			await _cache.RemoveAsync(CacheKeys.CountriesKey);
			var countries = await _repo.Get();
			Assert.AreEqual(2, countries.ToList().Count);

			await _repo.AddCountryWriteThrough(new Country() { Name = "UK" });

			var countriesAfterInsert = await _repo.Get();
			Assert.AreEqual(3, countriesAfterInsert.ToList().Count);

			await _repo.DeleteCountryWriteThrough("UK");
		}

	}
}