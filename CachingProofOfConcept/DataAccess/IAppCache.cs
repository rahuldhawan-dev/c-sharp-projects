using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
	public interface IAppCache
	{
		Task<T> GetAsync<T>(string key);

		Task SetAsync(string key, byte[] value, TimeSpan? absoluteExpirationRelativeToNow);

		Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);

		Task<bool> KeyExistsAsync(string key);
		
		Task RemoveAsync(string key);
		
		Task RemoveAsync(IEnumerable<string> keys);
		
		Task<T> GetAndRemoveAsync<T>(string key);

		// Sets
		Task<bool> AppendToSetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);
		Task<IEnumerable<T>> GetSetMembersAsync<T>(string key);
	}
}