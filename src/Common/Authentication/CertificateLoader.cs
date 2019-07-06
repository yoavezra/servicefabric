// <copyright file="CertificateLoader.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Authentication
{
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    /// <inheritdoc />
    public class CertificateLoader : ICertificateLoader
    {
        /// <inheritdoc />
        public X509Certificate2 GetCertificate(string certThumbprint)
        {
            return GetCertificate(certThumbprint, StoreLocation.LocalMachine);
        }

        private static X509Certificate2 GetCertificate(string certThumbprint, StoreLocation storeLocation)
        {
            var certificate = TryGetCertificate(certThumbprint, storeLocation);

            if (certificate == null)
            {
                throw new FileNotFoundException($"Certificate with Thumbprint {certThumbprint} is not installed on the machine");
            }

            return certificate;
        }

        private static X509Certificate2 TryGetCertificate(string certThumbprint, StoreLocation storeLocation)
        {
            return TryGetCerficateByThumbprint(certThumbprint, storeLocation);
        }

        private static X509Certificate2 TryGetCerficateByThumbprint(string certThumbprint, StoreLocation storeLocation)
        {
            X509Certificate2 cert;
            var store = new X509Store(StoreName.My, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            try
            {
                cert = store.Certificates.OfType<X509Certificate2>().FirstOrDefault(x => x.Thumbprint?.ToUpperInvariant() == certThumbprint.ToUpperInvariant());
            }
            finally
            {
                store.Close();
            }

            return cert;
        }
    }
}