using CluedIn.Core.Crawling;

namespace CluedIn.Crawling.AzureDataLake.Core
{
    public class AzureDataLakeCrawlJobData : CrawlJobData
    {
        public string ApiKey { get; set; }

        public string AccountName { get; set; }

        public string BaseUrl { get; set; }

        public string FolderName { get; set; }

        public string FileName { get; set; }
    }
}
