// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.Runtime.Modules
{
    /// <summary>
    /// Evaluation summary calculated by the <see cref="IModuleManager"/>
    /// </summary>
    public interface IDependencyEvaluation
    {
        /// <summary>
        /// Full dependency tree
        /// </summary>
        IModuleDependencyTree FullTree { get; }

        /// <summary>
        /// Number of root modules
        /// </summary>
        int RootModules { get; }

        /// <summary>
        /// Maximum dependency depth
        /// </summary>
        int MaxDepth { get; }

        /// <summary>
        /// Maximum number of dependencies
        /// </summary>
        int MaxDependencies { get; }

        /// <summary>
        /// Maximum number of dependends
        /// </summary>
        int MaxDependends { get; }
    }
}
