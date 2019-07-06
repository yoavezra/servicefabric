// <copyright file="AadAppTokenIssuer.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.ClientSdk
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public static class AadAppTokenIssuer
    {
        public static async Task<string> AcquireTokenUsingCertificateAsync(string authority, string resource, string clientId, X509Certificate2 certificate)
        {
            var assertionCert = new ClientAssertionCertificate(clientId, certificate);
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, assertionCert);
            return result.AccessToken;
        }

        public static async Task<string> AcquireTokenUsingSecretAsync(string authority, string resource, string clientId, string clientSecret)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(clientId, clientSecret);

            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
            {
                throw new InvalidOperationException($"Failed to obtain the JWT token for authority: {authority}, resource: {resource} ");
            }

            return result.AccessToken;
        }
    }
}