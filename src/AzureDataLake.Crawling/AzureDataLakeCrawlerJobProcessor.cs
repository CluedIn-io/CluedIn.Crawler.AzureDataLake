using CluedIn.Crawling.AzureDataLake.Core;

namespace CluedIn.Crawling.AzureDataLake
{
    public class AzureDataLakeCrawlerJobProcessor : GenericCrawlerTemplateJobProcessor<AzureDataLakeCrawlJobData>
    {
        public AzureDataLakeCrawlerJobProcessor(AzureDataLakeCrawlerComponent component) : base(component)
        {
        }
    }
}
