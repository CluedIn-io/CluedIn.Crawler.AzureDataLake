using CluedIn.Crawling.AzureDataLake.Core;

namespace CluedIn.Crawling.AzureDataLake.Infrastructure.Factories
{
    public interface IAzureDataLakeClientFactory
    {
        AzureDataLakeClient CreateNew(AzureDataLakeCrawlJobData azuredatalakeCrawlJobData);
    }
}
