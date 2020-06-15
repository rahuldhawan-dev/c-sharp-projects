namespace DataAccess
{
	public class RedisCacheOptions
	{
		public string Prefix { get; set; }
		public string Hostname { get; set; }
		public string Port { get; set; }
		public string Password { get; set; }
		public bool SslEnabled { get; set; }
		public string ClientCertificateSubject { get; set; }
		public bool SkipRemoteCertificateValidation { get; set; }
		public int SyncTimeoutMilliSeconds { get; set; }
	}

	public class CacheTtlOptions
	{
		public int CountriesTtlInSeconds { get; set; }
	}
}