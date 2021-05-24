using CluedIn.Core;
using CluedIn.Crawling.AzureDataLake.Core;

using ComponentHost;

namespace CluedIn.Crawling.AzureDataLake
{
    [Component(AzureDataLakeConstants.CrawlerComponentName, "Crawlers", ComponentType.Service, Components.Server, Components.ContentExtractors, Isolation = ComponentIsolation.NotIsolated)]
    public class AzureDataLakeCrawlerComponent : CrawlerComponentBase
    {
        public AzureDataLakeCrawlerComponent([NotNull] ComponentInfo componentInfo)
            : base(componentInfo)
        {
        }
    }
}

