using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using EntityHistory.Core.Helpers;
using JetBrains.Annotations;

namespace EntityHistory.TestBase
{
    public class Bootstrapper : IDisposable
    {
        public IContainer Container { get; set; }

        protected bool IsDisposed;

        private readonly IList<Type> _registerModuleTypes;

        private Bootstrapper([NotNull] Type startupModule, [CanBeNull] Action<BootstrapperOptions> optionsAction = null)
        {
            _registerModuleTypes = new List<Type>();

            Check.NotNull(startupModule, nameof(startupModule));

            var options = new BootstrapperOptions();
            optionsAction?.Invoke(options);
            
            if (!typeof(IModule).GetTypeInfo().IsAssignableFrom(startupModule))
            {
                throw new ArgumentException($"{nameof(startupModule)} should be derived from {nameof(IModule)}.");
            }

            _registerModuleTypes.Add(startupModule);

            if (options.BaseModule != null)
            {
                if (!typeof(IModule).GetTypeInfo().IsAssignableFrom(options.BaseModule))
                {
                    throw new ArgumentException($"{nameof(options.BaseModule)} should be derived from {nameof(IModule)}.");
                }

                _registerModuleTypes.Add(options.BaseModule);
            }
        }

        public static Bootstrapper Create<TStartupModule>([CanBeNull] Action<BootstrapperOptions> optionsAction = null)
            where TStartupModule : IModule
        {
            return new Bootstrapper(typeof(TStartupModule), optionsAction);
        }

        public static Bootstrapper Create([NotNull] Type startupModule, [CanBeNull] Action<BootstrapperOptions> optionsAction = null)
        {
            return new Bootstrapper(startupModule, optionsAction);
        }

        public virtual void Initialize()
        {
            var builder = new ContainerBuilder();

            foreach (var moduleType in _registerModuleTypes)
            {
                var assembly = Assembly.GetAssembly(moduleType);
                builder.RegisterAssemblyModules(moduleType, assembly);
            }

            Container = builder.Build();
        }
        
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            Container?.Dispose();
        }
    }
}
