// <copyright file="ResourceLoader.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Resources.Configuration
{
    using System.Reflection;
    using Newtonsoft.Json;

    public static class ResourceLoader
    {
        private const string NS = "ServiceSample.Services.Resources";

        public static T LoadConfigurations<T>(string resourceName)
        {
            string allEnvGepSettingsString = Common.General.ResourceReader.GetResource(
                Assembly.GetExecutingAssembly(),
                NS,
                resourceName);
            return JsonConvert.DeserializeObject<T>(allEnvGepSettingsString);
        }
    }
}