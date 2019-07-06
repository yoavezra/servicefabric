// <copyright file="FodyWeavers.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>
// <summary>
// This file contains all Fody (https://github.com/Fody/Fody) weavers assembly level attributes.
// </summary>

using NullGuard;

[assembly: NullGuard(ValidationFlags.All)]