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
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Marvin.TestTools.Test.Model;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Marvin.Model;

namespace Marvin.TestTools.TestMerge.Model
{
    internal partial class SimpleChildRepository : InheritedEntityFrameworkRepository<SimpleChild>, ISimpleChildRepository
    {
        public static IRepository Create(IUnitOfWork uow, DbContext context, IUnitOfWork parentUow)
        {
            var instance = new SimpleChildRepository();
            instance.UnitOfWork = uow;
            instance.Context = context;
            instance.ParentUow = parentUow;
            instance.DbSet = context.Set<SimpleChild>();
            return instance;
        }

        public SimpleChild LoadByTopParent(TopParent parent)
        {
            var simpleChild = DbSet.FirstOrDefault(e => e.Id == parent.Id);
            if (simpleChild != null)
                simpleChild.MergeParent = parent;
            return simpleChild;               
        }

        public SimpleChild LoadTopParentProperties(SimpleChild child)
        {
            child.MergeParent = ParentUow.GetRepository<ITopParentRepository>().GetByKey(child.Id);
            return child;               
        }
 
        public override SimpleChild Create()
        {
            var newInstance = base.Create();
            newInstance.MergeParent = ParentUow.GetRepository<ITopParentRepository>().Create();
            EntityIdListener.Listen(newInstance.MergeParent, newInstance);           
            return newInstance;  
        }

        public override SimpleChild GetByKey(long id)
        {
		    var result = DbSet.FirstOrDefault(e => e.Id == id);
            if (result == null)
                return result;
            result.MergeParent = ParentUow.GetRepository<ITopParentRepository>().GetByKey(result.Id);
            return result;
        }

        public override ICollection<SimpleChild> GetByKeys(long[] ids)
        {
			var result= DbSet.Where(e => ids.Contains(e.Id)).ToDictionary(e => e.Id, e => e);
			var entityIds = result.Keys.ToArray();
			var parents = ParentUow.GetRepository<ITopParentRepository>().GetByKeys(entityIds).ToDictionary(e => e.Id, e => e);
			foreach(var id in entityIds)
			{
				result[id].MergeParent = parents[id];
			}
            return result.Values;
        }

        public ICollection<SimpleChild> GetAll()
		{
			var result= DbSet.ToDictionary(e => e.Id, e => e);
			var entityIds = result.Keys.ToArray();
			var parents = ParentUow.GetRepository<ITopParentRepository>().GetByKeys(entityIds).ToDictionary(e => e.Id, e => e);
			foreach(var id in entityIds)
			{
				result[id].MergeParent = parents[id];
			}
            return result.Values;
		}
        protected override void ExecuteRemove(SimpleChild entity, bool permanent)
        {
		    base.ExecuteRemove(entity, permanent);
		    ParentUow.GetRepository<ITopParentRepository>().Remove(entity, permanent);
		}

		protected override void ExecuteRemoveRange(IEnumerable<SimpleChild> entities, bool permanent)
		{
		    base.ExecuteRemoveRange(entities, permanent);
		    ParentUow.GetRepository<ITopParentRepository>().RemoveRange(entities.Select(e => e.MergeParent), permanent);
		}

    }
}