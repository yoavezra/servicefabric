// <copyright file="TrafficSource.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.ClientSdk
{
    // Used as a field of operations indicates whether request source is from test or from runners
    public enum TrafficSource
    {
        Test,
        Runners,
    }
}