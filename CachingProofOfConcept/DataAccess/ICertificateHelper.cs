using System.Security.Cryptography.X509Certificates;

namespace DataAccess
{
	public interface ICertificateHelper
	{
		X509Certificate2 ReadCert(StoreLocation storeLocation, string certificateSubjectName);
	}
}