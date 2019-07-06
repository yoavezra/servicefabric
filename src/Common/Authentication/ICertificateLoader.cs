// <copyright file="ICertificateLoader.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Authentication
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Used to load a certificate from the local machine.
    /// </summary>
    public interface ICertificateLoader
    {
        /// <summary>
        /// Loads the certificate from machine local store or user store. Throws exception if the certificate is missing.
        /// </summary>
        /// <param name="certThumbprint">The thumprint of the certificate.</param>
        /// <returns>The certificate loaded from the machine.</returns>
        X509Certificate2 GetCertificate(string certThumbprint);
    }
}