﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using the Marvin template for generating Repositories and a Unit of Work for Entity Framework.
// If you have any questions or suggestions for improvement regarding this code, contact Thomas Fuchs. I allways need feedback to improve.
//
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. So even when you think you can do better,
// don't touch it.
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Marvin.Model;


namespace Marvin.TestTools.Test.Model
{
    /// <summary>
    /// The public API of the RecursiveReference repository.
    /// </summary>
    public partial interface IRecursiveReferenceRepository : IRepository<RecursiveReference>
    {
		/// <summary>
        /// Get all RecursiveReferences from the database
        /// </summary>
		/// <returns>A collection of entities. The result may be empty but not null.</returns>
        ICollection<RecursiveReference> GetAll();
        /// <summary>
        /// Get all RecursiveReferences where Count equals given value
        /// </summary>
        /// <param name="count">Value the entities have to match</param>
        /// <returns>Collection of all matching RecursiveReferences</returns>
        ICollection<RecursiveReference> GetAllByCount(int count);
        /// <summary>
        /// Creates instance with all not nullable properties prefilled
        /// </summary>
        RecursiveReference Create(int count, long recursiveReferenceId); 
    }
}