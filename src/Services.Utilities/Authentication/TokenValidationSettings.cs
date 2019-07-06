// <copyright file="TokenValidationSettings.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Authentication
{
    public class TokenValidationSettings
    {
        /// <summary>
        /// Authority to validate on user token validation
        /// </summary>
        public string AuthorizationServer { get; set; }

        /// <summary>
        /// Resource IDs of our apps to validate on user token validation
        /// </summary>
        public string ValidAudiences { get; set; }

        /// <summary>
        /// Issuer prefix to validate on user token validation
        /// </summary>
        public string IssuerPrefix { get; set; }
    }
}