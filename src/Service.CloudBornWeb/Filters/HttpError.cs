// <copyright file="HttpError.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Filters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public struct HttpError
    {
        public HttpError(ErrorCode errorCode)
        {
            this.ErrorCode = errorCode.ToString();
        }

        public static HttpError GeneralServerError()
        {
            return new HttpError(Filters.ErrorCode.CloudBornWebServiceGeneralError);
        }

        public string ErrorCode { get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public override string ToString() => this.ErrorCode;
    }
}
