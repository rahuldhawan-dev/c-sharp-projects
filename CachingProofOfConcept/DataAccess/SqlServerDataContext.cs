using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DataAccess
{
	public class SqlServerDataContext : IDataContext
	{
		private readonly ILogger<SqlServerDataContext> _logger;
		private readonly string _connectionString;
		private readonly object _sync = new object();
		private volatile IDbConnection _connection;
		private bool _disposed;

		public SqlServerDataContext(string connectionString, int maxPoolSize, ILogger<SqlServerDataContext> logger)
		{
			_logger = logger;
			var connectionBuilder = new SqlConnectionStringBuilder(connectionString)
			{
				MaxPoolSize = maxPoolSize,
				MultipleActiveResultSets = true
			};
			_connectionString = connectionBuilder.ToString();
		}

		public IDbConnection Connection => GetOpenConnection();

		public void Dispose()
		{
			Dispose(true);
		}

		private IDbConnection GetOpenConnection()
		{
			if (_connection != null)
			{
				return _connection;
			}
			lock (_sync)
			{
				FurnishConnection();
			}
			return _connection;
		}

		private void FurnishConnection()
		{
			IDbConnection connection = new SqlConnection(_connectionString);
			connection.Open();

			_logger?.LogTrace(FormatMessage("new connection furnished"));
			_connection = connection;
		}

		private string FormatMessage(string message)
		{
			return message;
		}

		private void Dispose(bool disposing)
		{
			_logger?.LogTrace(FormatMessage("data context disposing"));

			if (_disposed)
				return;

			if (disposing)
			{
				_logger?.LogTrace(FormatMessage("connection disposed"));
				_connection?.Dispose();
				_connection = null;
			}

			_disposed = true;
		}
	}
}