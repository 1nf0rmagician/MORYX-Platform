// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using Moryx.Runtime.Modules;

namespace Moryx.Runtime.Kernel
{
    internal interface IModuleManagerComponent
    {
        /// <summary>
        /// Global dictionary of modules awaiting the start of other plugins
        /// </summary>
        IDictionary<IServerModule, ICollection<IServerModule>> WaitingModules { get; set; }

        /// <summary>
        /// Delegate to get a copy of the module list
        /// </summary>
        Func<IEnumerable<IServerModule>> AllModules { get; set; }
    }
}
