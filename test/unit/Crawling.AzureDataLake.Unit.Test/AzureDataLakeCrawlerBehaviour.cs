using CluedIn.Core.Crawling;
using CluedIn.Crawling;
using CluedIn.Crawling.AzureDataLake;
using CluedIn.Crawling.AzureDataLake.Infrastructure.Factories;
using Moq;
using Shouldly;
using Xunit;

namespace Crawling.AzureDataLake.Unit.Test
{
    public class AzureDataLakeCrawlerBehaviour
    {
        private readonly ICrawlerDataGenerator _sut;

        public AzureDataLakeCrawlerBehaviour()
        {
            var nameClientFactory = new Mock<IAzureDataLakeClientFactory>();

            _sut = new AzureDataLakeCrawler(nameClientFactory.Object);
        }

        [Fact]
        public void GetDataReturnsData()
        {
            var jobData = new CrawlJobData();

            _sut.GetData(jobData)
                .ShouldNotBeNull();
        }
    }
}
