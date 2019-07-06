// <copyright file="AllowNullSettingAttribute.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.General
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AllowNullSettingAttribute : Attribute
    {
    }
}