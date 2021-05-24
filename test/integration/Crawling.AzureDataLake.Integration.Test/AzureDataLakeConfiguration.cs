using System.Collections.Generic;
using CluedIn.Crawling.AzureDataLake.Core;

namespace CluedIn.Crawling.AzureDataLake.Integration.Test
{
  public static class AzureDataLakeConfiguration
  {
    public static Dictionary<string, object> Create()
    {
      return new Dictionary<string, object>
            {
                { AzureDataLakeConstants.KeyName.ApiKey, "demo" }
            };
    }
  }
}
