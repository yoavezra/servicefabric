// <copyright file="AdalAppTokenIssuerWithSecret.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.ClientSdk
{
    using System.Threading.Tasks;

    public class AdalAppTokenIssuerWithSecret : ITokenIssuer
    {
        private readonly string authority;
        private readonly string validAudience;
        private readonly string clientId;
        private readonly string clientSecret;

        public AdalAppTokenIssuerWithSecret(string authority, string validAudience, string clientId, string clientSecret)
        {
            this.authority = authority;
            this.validAudience = validAudience;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public async Task<string> CreateToken()
        {
            return await AadAppTokenIssuer.AcquireTokenUsingSecretAsync(
                this.authority,
                this.validAudience,
                this.clientId,
                this.clientSecret);
        }
    }
}