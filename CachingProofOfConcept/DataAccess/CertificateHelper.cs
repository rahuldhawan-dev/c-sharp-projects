using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DataAccess
{
	public class CertificateHelper : ICertificateHelper
	{
		public X509Certificate2 ReadCert(StoreLocation storeLocation, string certificateSubjectName)
		{
			X509Store store = new X509Store(storeLocation);

			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindBySubjectName,
				certificateSubjectName, false);

			X509Certificate2 x509Certificate2 = certificates.Count > 0 ? certificates[0] : null;

			if (x509Certificate2 == null)
			{
				foreach (
					X509Certificate2 mCert in
					store.Certificates.Cast<X509Certificate2>()
						.Where(mCert => certificateSubjectName == mCert.SubjectName.Name))
				{
					x509Certificate2 = mCert;
				}
			}

			store.Close();

			return x509Certificate2;
		}

		public X509Certificate2 ReadCert(string certificatePath, string certificatePassword = null)
		{
			X509Store store = new X509Store(StoreLocation.LocalMachine);

			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection certificates = new X509Certificate2Collection();

			if (string.IsNullOrEmpty(certificatePassword))
			{
				certificates.Import(certificatePath);
			}
			else
			{
				certificates.Import(certificatePath, certificatePassword, X509KeyStorageFlags.PersistKeySet);
			}

			X509Certificate2 x509Certificate2 = certificates.Count > 0 ? certificates[0] : null;

			store.Close();

			return x509Certificate2;
		}
	}
}