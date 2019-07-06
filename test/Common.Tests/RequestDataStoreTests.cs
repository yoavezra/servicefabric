// <copyright file="RequestDataStoreTests.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.UnitTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ServiceSample.Common.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RequestDataStoreTests
    {
        private RequestDataStore requestDataStore;

        [TestInitialize]
        public void Init()
        {
            this.requestDataStore = new RequestDataStore();
        }

        [TestMethod]
        public void RequestDataStore_SameThread_TeantIdReserved()
        {
            var id = Guid.NewGuid();

            this.requestDataStore.SetTenantId(id.ToString());

            Assert.AreEqual(id, Guid.Parse(this.requestDataStore.TenantId), "TenantId");
        }

        [TestMethod]
        public async Task RequestDataStore_AsyncTask_TeantIdReserved()
        {
            var id = Guid.NewGuid();

            this.requestDataStore.SetTenantId(id.ToString());

            await this.CheckIdAsync(id);
        }

        [TestMethod]
        public void RequestDataStore_DifferentThread_TeantIdNotReserved()
        {
            var id = Guid.NewGuid();

            this.requestDataStore.SetTenantId(id.ToString());

            Thread thread = new Thread(this.CheckIdkDifferntThread);

            thread.Start();
            thread.Join();
        }

        private void CheckIdkDifferntThread()
        {
            Assert.AreEqual(string.Empty, this.requestDataStore.TenantId, "TenantId");
        }

        private async Task CheckIdAsync(Guid id)
        {
            Assert.AreEqual(id, Guid.Parse(this.requestDataStore.TenantId), "TenantId");

            await Task.Yield();
        }
    }
}