// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Data.Entity;
using System.Linq;
using Moryx.Configuration;
using Moryx.Logging;
using Moryx.Modules;
using Moryx.Tools;

namespace Moryx.Model
{
    public interface IDbContextFactory
    {
        TContext Create<TContext>(ContextMode contextMode)
            where TContext : DbContext;
    }

    public class DbContextFactory : IDbContextFactory, IInitializable
    {
        public IConfigManager ConfigManager { get; set; }

        public IModuleLogger ModuleLogger { get; set; }

        private ModelWrapper[] _knownModels;

        public void Initialize()
        {
            var dbContextTypes = ReflectionTool.GetPublicClasses(typeof(DbContext), delegate (Type type)
            {
                var modelAttr = type.GetCustomAttribute<ModelAttribute>();
                return modelAttr != null;
            });

            _knownModels = dbContextTypes
                .Select(dbContextType =>
                    new { DbContextType = dbContextType, ModelAttr = dbContextType.GetCustomAttribute<ModelAttribute>() })
                .Select(t =>
                {
                    var wrapper = new ModelWrapper();
                    wrapper.Name = t.ModelAttr.Name;
                    wrapper.DbContextType = t.DbContextType;
                    wrapper.Configurator = (IModelConfigurator)Activator.CreateInstance(t.ModelAttr.ConfiguratorType);
                    return wrapper;
                }).ToArray();

            foreach (var wrapper in _knownModels)
            {
                wrapper.Configurator.Initialize(wrapper.DbContextType, ConfigManager, ModuleLogger);
            }
        }

        public TContext Create<TContext>(ContextMode contextMode) where TContext : DbContext
        {
            var wrapper = _knownModels.FirstOrDefault(k => k.DbContextType == typeof(TContext));
            if (wrapper == null)
                throw new InvalidOperationException("Unknown model");

            var configurator = wrapper.Configurator;
            return (TContext)configurator.CreateContext(contextMode);

        }

        private class ModelWrapper
        {
            public string Name { get; set; }

            public Type DbContextType { get; set; }

            public IModelConfigurator Configurator { get; set; }
        }
    }
}
