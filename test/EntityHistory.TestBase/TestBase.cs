using System;
using Autofac;
using Autofac.Core;
using EntityHistory.Abstractions.Session;

namespace EntityHistory.TestBase
{
    /// <summary>
    /// This is the base class for all tests integrated to Entity History.
    /// </summary>
    public abstract class TestBase<TStartupModule> : IDisposable
        where TStartupModule : IModule
    {
        protected IContainer Container { get; set; }

        protected Bootstrapper Bootstrapper { get; }

        protected ISession<long> Session { get; set; }

        protected TestBase(bool initialize = true)
        {
            Bootstrapper = Bootstrapper.Create<TStartupModule>();

            if (initialize)
            {
                Initialize();
            }
        }

        protected void Initialize()
        {           
            Bootstrapper.Initialize();
            Container = Bootstrapper.Container;
            Session = Container.Resolve<ISession<long>>();
        }

        public virtual void Dispose()
        {
            Container?.Dispose();
            Bootstrapper?.Dispose();
        }
    }
}
