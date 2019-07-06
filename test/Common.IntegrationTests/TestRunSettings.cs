// <copyright file="TestRunSettings.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.IntegrationTests
{
    using System.Collections;

    public class TestRunSettings
    {
        private const string EnvironmentTypeKey = "EnvironmentType";
        private const string TestPasswordsKey = "TestPasswords";

        public TestRunSettings(IDictionary contextProperties)
        {
            this.TestPasswordsString = (string)contextProperties[TestPasswordsKey];
            this.Environment = (string)contextProperties[EnvironmentTypeKey];
        }

        public TestRunSettings(string environment, string testPasswordsString)
        {
            this.Environment = environment;
            this.TestPasswordsString = testPasswordsString;
        }

        public string TestPasswordsString { get; }

        public string Environment { get; }
    }
}