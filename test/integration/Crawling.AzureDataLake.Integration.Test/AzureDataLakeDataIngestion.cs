using System.Linq;
using CrawlerIntegrationTesting.Clues;
using Xunit;
using Xunit.Abstractions;

namespace CluedIn.Crawling.AzureDataLake.Integration.Test
{
    public class DataIngestion : IClassFixture<AzureDataLakeTestFixture>
    {
        private readonly AzureDataLakeTestFixture fixture;
        private readonly ITestOutputHelper output;

        public DataIngestion(AzureDataLakeTestFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Theory]
        [InlineData("/Actor", 2)]
        public void CorrectNumberOfEntityTypes(string entityType, int expectedCount)
        {
            var foundCount = fixture.ClueStorage.CountOfType(entityType);

            //You could use this method to output the logs inside the test case
            fixture.PrintLogs(output);

            Assert.Equal(expectedCount, foundCount);
        }

        [Fact]
        public void EntityCodesAreUnique()
        {
            var count = fixture.ClueStorage.Clues.Count();
            var unique = fixture.ClueStorage.Clues.Distinct(new ClueComparer()).Count();

            //You could use this method to output info of all clues
            fixture.PrintClues(output);

            Assert.Equal(unique, count);
        }

       
    }
}
