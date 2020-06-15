using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DataAccess
{
	public class SqlCachingCountryRepository : ICountryRepository
	{
		private readonly IDataContext _dataContext;
		private readonly IAppCache _cache;
		private readonly CacheTtlOptions _cacheTtl;
		private readonly ILogger<SqlCachingCountryRepository> _logger;
		private string _sql = "SELECT Id, Name FROM Country";

		public SqlCachingCountryRepository(IDataContext dataContext, IAppCache cache, IOptions<CacheTtlOptions> cacheTTL, ILogger<SqlCachingCountryRepository> logger)
		{
			_dataContext = dataContext;
			_cache = cache;
			_cacheTtl = cacheTTL.Value;
			_logger = logger;
		}

		public async Task PrimeCache()
		{
			await GetDataFromDbAndSetCache();
		}

		public async Task<IEnumerable<Country>> Get()
		{
			(IEnumerable<Country>, bool) countries = await GetCountries();

			// Cache data in case not found in cache
			if (!countries.Item2)
			{
				await SetCache(countries.Item1);
			}

			return countries.Item1;
		}

		public async Task AddCountryCacheAside(Country country)
		{
			await Insert(country);
			await RemoveCache();
		}

		public async Task AddCountryWriteThrough(Country country)
		{
			await Insert(country);

			await RemoveCache();
			await Get();
		}

		public async Task DeleteCountryCacheAside(string name)
		{
			await Delete(name);
			await RemoveCache();
		}

		public async Task DeleteCountryWriteThrough(string name)
		{
			await Delete(name);
			await Get();
		}

		private async Task<(IEnumerable<Country>, bool)> GetCountries()
		{
			IEnumerable<Country> countries = null;
			var cacheHit = false;

			try
			{
				countries = await _cache.GetAsync<IEnumerable<Country>>(CacheKeys.CountriesKey);
			}
			catch (RedisConnectionException ex)
			{
				_logger.LogCritical(300, ex, ex.Message);
			}

			// Cache miss
			if (countries == null)
			{
				countries =
					await _dataContext.Connection.QueryAsync<Country>(_sql);
			}
			else
			{
				cacheHit = true;
			}

			return (countries, cacheHit);
		}

		private async Task GetDataFromDbAndSetCache()
		{
			var countries =
				await _dataContext.Connection.QueryAsync<Country>(_sql);
			await SetCache(countries);
		}

		private async Task SetCache(IEnumerable<Country> countries)
		{
			try
			{
				await _cache.SetAsync(CacheKeys.CountriesKey, countries, TimeSpan.FromSeconds(_cacheTtl.CountriesTtlInSeconds));
			}
			catch (RedisConnectionException ex)
			{
				_logger.LogCritical(302, ex, ex.Message);
			}
		}

		private async Task RemoveCache()
		{
			try
			{
				await _cache.RemoveAsync(CacheKeys.CountriesKey);
			}
			catch (RedisConnectionException ex)
			{
				_logger.LogCritical(303, ex, ex.Message);
			}
		}

		private async Task Insert(Country country)
		{
			await _dataContext.Connection.ExecuteAsync("INSERT INTO Country(Name) VALUES (@CountryName)",
				new { CountryName = country.Name });
		}

		private async Task Delete(string name)
		{
			await _dataContext.Connection.ExecuteAsync("DELETE Country WHERE Name = @CountryName",
				new {CountryName = name});
		}
	}

	public interface ICountryRepository
	{
		// Use this method to cache Country on service startup to avoid multiple calls to database/cache
		// from different controllers simultaneously
		Task PrimeCache();

		// Use this method to get data from cache if available otherwise from database
		// Data will be cached for 
		Task<IEnumerable<Country>> Get();
	}
}
