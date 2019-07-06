// <copyright file="AdalUserTokenIssuer.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.ClientSdk
{
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public class AdalUserTokenIssuer : ITokenIssuer
    {
        private readonly string authorityUri;
        private readonly string userName;
        private readonly string password;
        private readonly string validAudience;
        private readonly string clientId;

        public AdalUserTokenIssuer(string authority, string userName, string password, string validAudience, string clientId)
        {
            this.authorityUri = authority;
            this.userName = userName;
            this.password = password;
            this.validAudience = validAudience;
            this.clientId = clientId;
        }

        public async Task<string> CreateToken()
        {
            AuthenticationContext authContext = new AuthenticationContext(this.authorityUri);

            UserPasswordCredential userCredential = new UserPasswordCredential(this.userName, this.password);

            AuthenticationResult authResult = await authContext.AcquireTokenAsync(
                this.validAudience,
                this.clientId,
                userCredential).ConfigureAwait(false);

            return authResult.AccessToken;
        }
    }
}