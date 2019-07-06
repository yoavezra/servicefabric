// <copyright file="EnvironmentSettingsTests.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.UnitTests
{
    using System.Resources;
    using ServiceSample.Services.Resources.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnvironmentSettingsTests
    {
        private static readonly string[] Environments = { "Int", };

        [TestMethod]
        public void EnvironmentSettingsLoader_LoadResourceForAllEnvironments_NoError()
        {
            // Make sure that resource files for all environments are loaded correctly
            foreach (var environment in Environments)
            {
                EnvironmentSettingsLoader.Load($"{environment}.Settings.json");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(MissingManifestResourceException))]
        public void EnvironmentSettingsLoader_LoadResourceForMissingEnvironment_RaiseError()
        {
            EnvironmentSettingsLoader.Load($"Missing.Settings.json");
        }
    }
}