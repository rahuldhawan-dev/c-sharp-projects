using System;
using System.Data;

namespace DataAccess
{
	public interface IDataContext : IDisposable
	{
		/// <summary>
		/// Gets the current open connection to the database.
		/// If it does not exist, a new one is opened.
		/// </summary>
		IDbConnection Connection { get; }
	}
}