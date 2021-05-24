using Castle.Windsor;
using CluedIn.Core;
using CluedIn.Core.Providers;
using CluedIn.Crawling.AzureDataLake.Infrastructure.Factories;
using Moq;

namespace CluedIn.Provider.AzureDataLake.Unit.Test.AzureDataLakeProvider
{
    public abstract class AzureDataLakeProviderTest
    {
        protected readonly ProviderBase Sut;

        protected Mock<IAzureDataLakeClientFactory> NameClientFactory;
        protected Mock<IWindsorContainer> Container;

        protected AzureDataLakeProviderTest()
        {
            Container = new Mock<IWindsorContainer>();
            NameClientFactory = new Mock<IAzureDataLakeClientFactory>();
            var applicationContext = new ApplicationContext(Container.Object);
            Sut = new AzureDataLake.AzureDataLakeProvider(applicationContext, NameClientFactory.Object);
        }
    }
}
