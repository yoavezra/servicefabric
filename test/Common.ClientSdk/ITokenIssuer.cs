// <copyright file="ITokenIssuer.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.ClientSdk
{
    using System.Threading.Tasks;

    public interface ITokenIssuer
    {
        Task<string> CreateToken();
    }
}