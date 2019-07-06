// <copyright file="DiagnosticsTests.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.CloudBornWeb.IntegrationTests
{
    using System.Threading.Tasks;
    using ServiceSample.Common.ClientSdk;
    using ServiceSample.Common.IntegrationTests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DiagnosticsTests
    {
        private static CloudBornWebTestRunContext testRunContext;

        [ClassInitialize]
        public static async Task TestClassInitialize(TestContext context)
        {
            testRunContext = await CloudBornWebTestDriver.InitTestEnvironment(new TestRunSettings(context.Properties), TrafficSource.Test).ConfigureAwait(false);
        }

        [TestMethod]
        [TestCategory(TestCategories.IntegrationTests)]
        public async Task CloudBornWeb_Diagnostics_Health()
        {
            await testRunContext.CloudBornWebClient.GetHealth().ConfigureAwait(false);
        }
    }
}
