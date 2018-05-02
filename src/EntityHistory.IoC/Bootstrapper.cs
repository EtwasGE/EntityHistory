using System;
using System.Reflection;
using Autofac;
using Autofac.Core;
using EntityHistory.Core.Helpers;
using JetBrains.Annotations;

namespace EntityHistory.IoC
{
    public class Bootstrapper : IDisposable
    {
        public IContainer Container { get; set; }

        protected bool IsDisposed;

        private readonly Type _startupModuleType;

        private Bootstrapper([NotNull] Type startupModule, [CanBeNull] Action<BootstrapperOptions> optionsAction = null)
        {
            Check.NotNull(startupModule, nameof(startupModule));

            var options = new BootstrapperOptions();
            optionsAction?.Invoke(options);

            if (!typeof(IModule).GetTypeInfo().IsAssignableFrom(startupModule))
            {
                throw new ArgumentException($"{nameof(startupModule)} should be derived from {nameof(IModule)}.");
            }

            _startupModuleType = startupModule;
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

            builder.RegisterModule<BaseModule>();

            var startupModuleAssembly = Assembly.GetAssembly(_startupModuleType);
            builder.RegisterAssemblyModules(_startupModuleType, startupModuleAssembly);
            
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
