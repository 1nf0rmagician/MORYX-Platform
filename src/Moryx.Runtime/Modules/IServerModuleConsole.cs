// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;

namespace Moryx.Runtime.Modules
{
    /// <summary>
    /// Console interface to provide developer interaction with a module
    /// </summary>
    public interface IServerModuleConsole
    {
        /// <summary>
        /// Export the description of this module
        /// </summary>
        /// <param name="format">Descirption format</param>
        /// <returns>Formatted description string</returns>
        string ExportDescription(DescriptionExportFormat format);

        /// <summary>
        /// Pass a command to this module
        /// </summary>
        /// <param name="args">Arguments passed to the module</param>
        /// <param name="outputStream">Output stream to be used for feedback</param>
        void ExecuteCommand(string[] args, Action<string> outputStream);
    }

    /// <summary>
    /// Format for the description export
    /// </summary>
    public enum DescriptionExportFormat
    {
        /// <summary>
        /// Print module description to developer console. This might include runtime specific information.
        /// </summary>
        Console,

        /// <summary>
        /// Print module description to be used for general documentation
        /// </summary>
        Documentation,
    }
}
