// <copyright file="ExtensionOperations.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public static class ExtensionOperations
    {
        /// <summary>
        /// JSON string for an empty property bag.
        /// </summary>
        private const string EmptyPropertyBagJson = "{}";

        /// <summary>
        /// Serialize dictionary to Json string
        /// </summary>
        /// <returns>Json string</returns>
        public static string ToJsonString(this IDictionary<string, object> propertyBag)
        {
            if (propertyBag == null)
            {
                return EmptyPropertyBagJson;
            }

            return JsonConvert.SerializeObject(propertyBag);
        }
    }
}