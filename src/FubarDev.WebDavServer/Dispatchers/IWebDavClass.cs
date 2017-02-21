﻿// <copyright file="IWebDavClass.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Collections.Generic;

using JetBrains.Annotations;

namespace FubarDev.WebDavServer.Dispatchers
{
    public interface IWebDavClass
    {
        [NotNull]
        string Version { get; }

        [NotNull]
        [ItemNotNull]
        IEnumerable<string> HttpMethods { get; }

        [NotNull]
        IWebDavContext WebDavContext { get; }
    }
}