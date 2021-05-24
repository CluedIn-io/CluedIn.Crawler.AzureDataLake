using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CluedIn.Core;
using CluedIn.Core.Crawling;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;
using CluedIn.Core.Webhooks;
using System.Configuration;
using System.Linq;
using CluedIn.Core.Configuration;
using CluedIn.Crawling.AzureDataLake.Core;
using CluedIn.Crawling.AzureDataLake.Infrastructure.Factories;
using CluedIn.Providers.Models;
using Newtonsoft.Json;
using Azure.Storage;
using Azure.Storage.Files.DataLake;

namespace CluedIn.Provider.AzureDataLake
{
    public class AzureDataLakeProvider : ProviderBase, IExtendedProviderMetadata
    {
        private readonly IAzureDataLakeClientFactory _azuredatalakeClientFactory;

        public AzureDataLakeProvider([NotNull] ApplicationContext appContext, IAzureDataLakeClientFactory azuredatalakeClientFactory)
            : base(appContext, AzureDataLakeConstants.CreateProviderMetadata())
        {
            _azuredatalakeClientFactory = azuredatalakeClientFactory;
        }

        public override async Task<CrawlJobData> GetCrawlJobData(
            ProviderUpdateContext context,
            IDictionary<string, object> configuration,
            Guid organizationId,
            Guid userId,
            Guid providerDefinitionId)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var azuredatalakeCrawlJobData = new AzureDataLakeCrawlJobData();
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.ApiKey))
            {
                azuredatalakeCrawlJobData.ApiKey = configuration[AzureDataLakeConstants.KeyName.ApiKey].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.AccountName))
            {
                azuredatalakeCrawlJobData.AccountName = configuration[AzureDataLakeConstants.KeyName.AccountName].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.BaseUrl))
            {
                azuredatalakeCrawlJobData.BaseUrl = configuration[AzureDataLakeConstants.KeyName.BaseUrl].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.FileName))
            {
                azuredatalakeCrawlJobData.FileName = configuration[AzureDataLakeConstants.KeyName.FileName].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.FolderName))
            {
                azuredatalakeCrawlJobData.FolderName = configuration[AzureDataLakeConstants.KeyName.FolderName].ToString();
            }

            return await Task.FromResult(azuredatalakeCrawlJobData);
        }

        public override Task<bool> TestAuthentication(
            ProviderUpdateContext context,
            IDictionary<string, object> configuration,
            Guid organizationId,
            Guid userId,
            Guid providerDefinitionId)
        {

            var _azuredatalakeCrawlJobData = new AzureDataLakeCrawlJobData();
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.ApiKey))
            {
                _azuredatalakeCrawlJobData.ApiKey = configuration[AzureDataLakeConstants.KeyName.ApiKey].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.AccountName))
            {
                _azuredatalakeCrawlJobData.AccountName = configuration[AzureDataLakeConstants.KeyName.AccountName].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.BaseUrl))
            {
                _azuredatalakeCrawlJobData.BaseUrl = configuration[AzureDataLakeConstants.KeyName.BaseUrl].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.FileName))
            {
                _azuredatalakeCrawlJobData.FileName = configuration[AzureDataLakeConstants.KeyName.FileName].ToString();
            }
            if (configuration.ContainsKey(AzureDataLakeConstants.KeyName.FolderName))
            {
                _azuredatalakeCrawlJobData.FolderName = configuration[AzureDataLakeConstants.KeyName.FolderName].ToString();
            }

            try
            {

                StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(_azuredatalakeCrawlJobData.AccountName, _azuredatalakeCrawlJobData.ApiKey);

                // Create DataLakeServiceClient using StorageSharedKeyCredentials
                DataLakeServiceClient serviceClient = new DataLakeServiceClient(new Uri(_azuredatalakeCrawlJobData.BaseUrl), sharedKeyCredential);

                // Create a DataLake Filesystem
                DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient(_azuredatalakeCrawlJobData.FolderName); //Get me a folder
                                                                                                                                //filesystem.Create();

                DataLakeFileClient file = filesystem.GetFileClient(_azuredatalakeCrawlJobData.FileName); //Get me a file

            }
            catch
            {
                return Task.Run(() => false);
            }

            return Task.Run(() => true);
        }

        public override Task<ExpectedStatistics> FetchUnSyncedEntityStatistics(ExecutionContext context, IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {

            return Task.Run(() => new ExpectedStatistics());
        }

        public override async Task<IDictionary<string, object>> GetHelperConfiguration(
            ProviderUpdateContext context,
            [NotNull] CrawlJobData jobData,
            Guid organizationId,
            Guid userId,
            Guid providerDefinitionId)
        {
            if (jobData == null)
                throw new ArgumentNullException(nameof(jobData));

            var dictionary = new Dictionary<string, object>();

            if (jobData is AzureDataLakeCrawlJobData azuredatalakeCrawlJobData)
            {
                //TODO add the transformations from specific CrawlJobData object to dictionary
                // add tests to GetHelperConfigurationBehaviour.cs
                dictionary.Add(AzureDataLakeConstants.KeyName.ApiKey, azuredatalakeCrawlJobData.ApiKey);
            }

            return await Task.FromResult(dictionary);
        }

        public override Task<IDictionary<string, object>> GetHelperConfiguration(
            ProviderUpdateContext context,
            CrawlJobData jobData,
            Guid organizationId,
            Guid userId,
            Guid providerDefinitionId,
            string folderId)
        {
            throw new NotImplementedException();
        }

        public override async Task<AccountInformation> GetAccountInformation(ExecutionContext context, [NotNull] CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            if (jobData == null)
                throw new ArgumentNullException(nameof(jobData));

            if (!(jobData is AzureDataLakeCrawlJobData azuredatalakeCrawlJobData))
            {
                throw new Exception("Wrong CrawlJobData type");
            }

            var client = _azuredatalakeClientFactory.CreateNew(azuredatalakeCrawlJobData);
            return await Task.FromResult(client.GetAccountInformation());
        }

        public override string Schedule(DateTimeOffset relativeDateTime, bool webHooksEnabled)
        {
            return webHooksEnabled && ConfigurationManager.AppSettings.GetFlag("Feature.Webhooks.Enabled", false) ? $"{relativeDateTime.Minute} 0/23 * * *"
                : $"{relativeDateTime.Minute} 0/4 * * *";
        }

        public override Task<IEnumerable<WebHookSignature>> CreateWebHook(ExecutionContext context, [NotNull] CrawlJobData jobData, [NotNull] IWebhookDefinition webhookDefinition, [NotNull] IDictionary<string, object> config)
        {
            if (jobData == null)
                throw new ArgumentNullException(nameof(jobData));
            if (webhookDefinition == null)
                throw new ArgumentNullException(nameof(webhookDefinition));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            throw new NotImplementedException();
        }

        public override Task<IEnumerable<WebhookDefinition>> GetWebHooks(ExecutionContext context)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteWebHook(ExecutionContext context, [NotNull] CrawlJobData jobData, [NotNull] IWebhookDefinition webhookDefinition)
        {
            if (jobData == null)
                throw new ArgumentNullException(nameof(jobData));
            if (webhookDefinition == null)
                throw new ArgumentNullException(nameof(webhookDefinition));

            throw new NotImplementedException();
        }

        public override IEnumerable<string> WebhookManagementEndpoints([NotNull] IEnumerable<string> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (!ids.Any())
            {
                throw new ArgumentException(nameof(ids));
            }

            throw new NotImplementedException();
        }

        public override async Task<CrawlLimit> GetRemainingApiAllowance(ExecutionContext context, [NotNull] CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            if (jobData == null)
                throw new ArgumentNullException(nameof(jobData));


            //There is no limit set, so you can pull as often and as much as you want.
            return await Task.FromResult(new CrawlLimit(-1, TimeSpan.Zero));
        }

        // TODO Please see https://cluedin-io.github.io/CluedIn.Documentation/docs/1-Integration/build-integration.html
        public string Icon => AzureDataLakeConstants.IconResourceName;
        public string Domain { get; } = AzureDataLakeConstants.Uri;
        public string About { get; } = AzureDataLakeConstants.CrawlerDescription;
        public AuthMethods AuthMethods { get; } = AzureDataLakeConstants.AuthMethods;
        public IEnumerable<Control> Properties => null;
        public string ServiceType { get; } = JsonConvert.SerializeObject(AzureDataLakeConstants.ServiceType);
        public string Aliases { get; } = JsonConvert.SerializeObject(AzureDataLakeConstants.Aliases);
        public Guide Guide { get; set; } = new Guide
        {
            Instructions = AzureDataLakeConstants.Instructions,
            Value = new List<string> { AzureDataLakeConstants.CrawlerDescription },
            Details = AzureDataLakeConstants.Details

        };

        public string Details { get; set; } = AzureDataLakeConstants.Details;
        public string Category { get; set; } = AzureDataLakeConstants.Category;
        public new IntegrationType Type { get; set; } = AzureDataLakeConstants.Type;
    }
}
