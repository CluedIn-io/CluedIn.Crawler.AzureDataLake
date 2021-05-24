using System.Collections.Generic;

using CluedIn.Core.Crawling;
using CluedIn.Crawling.AzureDataLake.Core;
using CluedIn.Crawling.AzureDataLake.Infrastructure.Factories;

namespace CluedIn.Crawling.AzureDataLake
{
    public class AzureDataLakeCrawler : ICrawlerDataGenerator
    {
        private readonly IAzureDataLakeClientFactory clientFactory;
        public AzureDataLakeCrawler(IAzureDataLakeClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public IEnumerable<object> GetData(CrawlJobData jobData)
        {
            if (!(jobData is AzureDataLakeCrawlJobData azuredatalakecrawlJobData))
            {
                yield break;
            }

            var client = clientFactory.CreateNew(azuredatalakecrawlJobData);

            foreach (var actor in client.GetActors())
                yield return actor;

        }
    }
}
