using Autofac;
using EntityHistory.Abstractions.Session;
using EntityHistory.Core.Session;

namespace EntityHistory.TestBase
{
    internal sealed class BaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultPrincipalAccessor>()
                .As<IPrincipalAccessor>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(ClaimsSession<>))
                .As(typeof(ISession<>))
                .SingleInstance();
        }
    }
}
