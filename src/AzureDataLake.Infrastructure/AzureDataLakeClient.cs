using System;
using System.Net;
using System.Threading.Tasks;
using CluedIn.Core.Providers;
using CluedIn.Crawling.AzureDataLake.Core;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using Azure.Storage;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;
using Azure;
using System.IO;
using System.Globalization;
using CsvHelper;
using CluedIn.Crawling.AzureDataLake.Core.Models;
using System.Linq;
using CsvHelper.Configuration;
using System.Text;

namespace CluedIn.Crawling.AzureDataLake.Infrastructure
{
    // TODO - This class should act as a client to retrieve the data to be crawled.
    // It should provide the appropriate methods to get the data
    // according to the type of data source (e.g. for AD, GetUsers, GetRoles, etc.)
    // It can receive a IRestClient as a dependency to talk to a RestAPI endpoint.
    // This class should not contain crawling logic (i.e. in which order things are retrieved)
    public class AzureDataLakeClient
    {
        private const string BaseUri = "http://sample.com";

        private readonly ILogger<AzureDataLakeClient> log;

        private readonly IRestClient client;

        private AzureDataLakeCrawlJobData _azuredatalakeCrawlJobData;

        public AzureDataLakeClient(ILogger<AzureDataLakeClient> log, AzureDataLakeCrawlJobData azuredatalakeCrawlJobData, IRestClient client) // TODO: pass on any extra dependencies
        {
            if (azuredatalakeCrawlJobData == null)
            {
                throw new ArgumentNullException(nameof(azuredatalakeCrawlJobData));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.client = client ?? throw new ArgumentNullException(nameof(client));

            // TODO use info from azuredatalakeCrawlJobData to instantiate the connection
            client.BaseUrl = new Uri(BaseUri);
            client.AddDefaultParameter("api_key", azuredatalakeCrawlJobData.ApiKey, ParameterType.QueryString);
            _azuredatalakeCrawlJobData = azuredatalakeCrawlJobData;
        }

        public IEnumerable<Actor> GetActors()
        {          

            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(_azuredatalakeCrawlJobData.AccountName, _azuredatalakeCrawlJobData.ApiKey);
      
            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(new Uri(_azuredatalakeCrawlJobData.BaseUrl), sharedKeyCredential);

            // Create a DataLake Filesystem
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient(_azuredatalakeCrawlJobData.FolderName); //Get me a folder
            //filesystem.Create();

            DataLakeFileClient file = filesystem.GetFileClient(_azuredatalakeCrawlJobData.FileName); //Get me a file

            Response<FileDownloadInfo> fileContents = file.Read();
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = true, Delimiter = ";", Encoding = Encoding.UTF8 };

            using (StreamReader sr = new StreamReader(fileContents.Value.Content))
            using (var csv = new CsvReader(sr, config))
            {
                //This allows you to do one Read operation.
                return csv.GetRecords<Actor>().ToList();
            }

        }
       
        public AccountInformation GetAccountInformation()
        {
            //TODO - return some unique information about the remote data source
            // that uniquely identifies the account
            return new AccountInformation("", "");
        }
    }
}
