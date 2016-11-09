using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Communication.Interfaces;
using CommunicationDevices.Infrastructure;


namespace CommunicationDevices.DI
{
    public class WindsorConfig : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
              .Register(Component.For<IWindsorContainer>().Instance(container).LifeStyle.Singleton);
          
             //.Register(Component.For<IExchangeDataProvider<Mg6587Input, Mg6587Output>>().ImplementedBy<PanelMg6587WriteDataProvider>().LifeStyle.Transient)
        }
    }
}