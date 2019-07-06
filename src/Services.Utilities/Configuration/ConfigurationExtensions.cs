// <copyright file="ConfigurationExtensions.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Configuration
{
    using System;
    using System.Fabric.Security;
    using System.Runtime.InteropServices;
    using System.Security;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Translates encrypted secret which key given as argument to SecureString.
        /// </summary>
        /// <returns>The cofiguration passed - as SecureString, Or null if the configuration doesn't exist</returns>
        /// <exception cref="InvalidOperationException">Thrown when failed to translate encrypted secret to SecureString</exception>
        public static SecureString GetSecret(this IConfiguration configutations, string configurationName)
        {
            string encryptedSecret = configutations[configurationName];
            if (encryptedSecret == null)
            {
                return null;
            }

            try
            {
                return EncryptionUtility.DecryptText(encryptedSecret);
            }
            catch (COMException exception)
            {
                throw new InvalidOperationException($"configuration value for key {configurationName} is not a valid encrypted value", exception);
            }
        }
    }
}