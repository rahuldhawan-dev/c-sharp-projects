using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DataAccess
{
	// Don't call async method from sync method, because StackExchange redis hangs app when sync code calls async Redis methods
	// This class can be put in common project if Cache needs to be used for other than DataAccess
	public class RedisAppCache : IAppCache
	{
		protected RedisCacheOptions _options;

		// Because the ConnectionMultiplexer does a lot, it is designed to be shared and reused between callers.
		// You should not create a ConnectionMultiplexer per operation. It is fully thread-safe and ready for this usage.
		// ConnectionMultiplexer implements IDisposable and can be disposed when no longer required.
		protected Lazy<ConnectionMultiplexer> _lazyConnection = null;

		// The object returned from GetDatabase is a cheap pass-thru object, and does not need to be stored.
		// Note that redis supports multiple databases (although this is not supported on “cluster”);
		private IDatabase _cache => _lazyConnection.Value.GetDatabase();

		public RedisAppCache() { }

		public RedisAppCache(RedisCacheOptions options)
		{
			ConfigurationOptions config = new ConfigurationOptions
			{
				// A more complicated scenario might involve a master/replica setup; for this usage,
				// simply specify all the desired nodes that make up that logical redis tier
				// it will automatically identify the master and send requests to master
				EndPoints = { $"{options.Hostname}:{options.Port}" }, 
				// Redis is designed to be accessed by trusted clients inside trusted environments.
				// This means that usually it is not a good idea to expose the Redis instance directly
				// to the internet or, in general, to an environment where untrusted clients can directly
				// access the Redis TCP port or UNIX socket.
				Password = options.Password,
				AbortOnConnectFail = false,
				// StackExchange.Redis automatically tries to reconnect in the background when the connection is lost for any reason.
				// By default its LinearRetry after every 3000 milliseconds
				//ReconnectRetryPolicy = new LinearRetry(3000),
				SyncTimeout = options.SyncTimeoutMilliSeconds == 0 ? 5000 : options.SyncTimeoutMilliSeconds
			};
			if (options.SslEnabled)
			{
				config.Ssl = true;
				config.CertificateSelection += delegate
				{
					CertificateHelper certificateHelper = new CertificateHelper();
					var cert = certificateHelper.ReadCert(StoreLocation.LocalMachine, options.ClientCertificateSubject);
					return cert;
				};

				if (options.SkipRemoteCertificateValidation)
				{
					config.CertificateValidation += delegate { return true; };
				}
			}
			_options = options;

			// Prefix would allow to segregate items for different environments in case same instance is used for multiple environments
			// e.g.: dev, test, staging
			if (!_options.Prefix.EndsWith(":"))
				_options.Prefix = _options.Prefix.Trim() + ":";

			_lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(config));
		}

		public RedisAppCache(IOptions<RedisCacheOptions> options) : this(options.Value) { }

		private string GetKeyWithPrefix(string key)
		{
			return string.Concat(_options.Prefix, key?.ToUpperInvariant());
		}

		public async Task<T> GetAsync<T>(string key)
		{
			var json = await _cache.StringGetAsync(GetKeyWithPrefix(key));
			if (string.IsNullOrWhiteSpace(json))
				return default(T);

			return JsonConvert.DeserializeObject<T>(json);
		}

		public Task SetAsync(string key, byte[] value, TimeSpan? absoluteExpirationRelativeToNow)
		{
			return _cache.StringSetAsync(GetKeyWithPrefix(key), value, absoluteExpirationRelativeToNow);
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
		{
			await _cache.StringSetAsync(GetKeyWithPrefix(key), JsonConvert.SerializeObject(value), absoluteExpirationRelativeToNow);
		}

		public async Task<bool> KeyExistsAsync(string key)
		{
			return await _cache.KeyExistsAsync(GetKeyWithPrefix(key));
		}

		public async Task RemoveAsync(string key)
		{
			await _cache.KeyDeleteAsync(GetKeyWithPrefix(key));
		}

		public async Task RemoveAsync(IEnumerable<string> keys)
		{
			await _cache.KeyDeleteAsync(keys.Select(x => (RedisKey)GetKeyWithPrefix(x)).ToArray());
		}

		public async Task<T> GetAndRemoveAsync<T>(string key)
		{
			T data = await GetAsync<T>(key);
			if (data != null)
				await _cache.KeyDeleteAsync(GetKeyWithPrefix(key));

			return data;
		}

		public Task<bool> AppendToSetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			var json = JsonConvert.SerializeObject(value);

			return _cache.SetAddAsync(GetKeyWithPrefix(key), json);
		}

		public async Task<IEnumerable<T>> GetSetMembersAsync<T>(string key)
		{
			var results = new List<T>();
			var values = await _cache.SetMembersAsync(GetKeyWithPrefix(key));
			foreach (var value in values)
				results.Add(JsonConvert.DeserializeObject<T>(value));
			return results;
		}
	}
}