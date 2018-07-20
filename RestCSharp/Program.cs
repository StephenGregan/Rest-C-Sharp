using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var searchSerivceName = "";
            var apiKey = "";

            var dataSourceName = "";
            var indexName = "";
            var indexerName = "";

            var azureBlobConnectionString = "";
            var azureBlobTableName = "";

            using (var httpClient = new HttpClient())
            {
                var dataSourceDefinition = AzureBlobDataSourceDefinition(azureBlobConnectionString, azureBlobTableName);
                var putDataSourceRequest = PutDataSourceRequest(searchSerivceName, apiKey, dataSourceName, dataSourceDefinition);
                Console.WriteLine($"Put data source: {putDataSourceRequest.RequestUri}\r\n");

                var putDataSourceResponse = httpClient.SendAsync(putDataSourceRequest).Result;
                var putDataSourceResponseContent = putDataSourceResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(putDataSourceResponseContent + "\r\n");

                var indexerDefinition = IndexerDefinition(dataSourceName, indexName);
                var putIndexerRequest = PutIndexerRequest(searchSerivceName, apiKey, indexerName, indexerDefinition);
                Console.WriteLine($"Put indexer {putDataSourceRequest.RequestUri}\r\n");

                var putIndexerResponse = httpClient.SendAsync(putIndexerRequest).Result;
                var putIndexerResponseContent = putIndexerResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(putIndexerResponseContent);

                var runIndexerRequest = RunIndexerRequest(searchSerivceName, apiKey, indexerName);
                Console.WriteLine($"Run indexer {runIndexerRequest.RequestUri}\r\n");
                var runIndexerResponse = httpClient.SendAsync(runIndexerRequest).Result;
                Console.WriteLine($"Success: {putDataSourceResponse.IsSuccessStatusCode}");

                Console.ReadLine();
            }
        }

        static HttpRequestMessage PutDataSourceRequest(string searchServiceName, string apiKey, string dataSourceName, string dataSourceDefinition)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://ronansearch.search.windows.net/datasources/allcontacts?api-version=2015-02-28-Preview");
            request.Headers.Add("api-key", apiKey);
            var body = new StringContent(dataSourceDefinition, Encoding.UTF8, "application/json");
            request.Content = body;
            return request;
        }

        static HttpRequestMessage PutIndexRequest(string searchServiceName, string apiKey, string indexName, string indexDefinition)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://ronansearch.search.windows.net/indexes/index?api-version=2016-09-Preview");
            request.Headers.Add("api-key", apiKey);
            var body = new StringContent(indexDefinition, Encoding.UTF8, "application/json");
            request.Content = body;
            return request;
        }

        static HttpRequestMessage PutIndexerRequest(string searchServiceName, string apiKey, string indexerName, string indexerDefinition)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://ronansearch.search.window.net/indexers/indexer?api-version=2016-09-01");
            request.Headers.Add("api-key", apiKey);
            var body = new StringContent(indexerDefinition, Encoding.UTF8, "application/json");
            request.Content = body;
            return request;
        }

        static HttpRequestMessage RunIndexerRequest(string searchServiceName, string apiKey, string indexerName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://ronansearch.search.windows.net/indexers/indexer/run?api-version=2015-02-28-Preview");
            request.Headers.Add("api-key", apiKey);
            return request;
        }

        static string AzureBlobDataSourceDefinition(string connectionString, string tableName)
        {
            return @"
                        {
                            ""description"": ""azure blob storage datasource"",
                            ""type"": ""azurebob"",
                            ""credentials"": ""connectionString"": """ + connectionString + @""",
                            ""container"": { ""name"": """ + tableName + @""" },
                            ""dataChangeDetectionpolicy"": {
                                                                ""@odata.type"": ""#Microsoft.Azure.Search.HighWaterMarkChangeDetectionPolicy"",
                                                                ""highWaterMarkColumnName"": ""highwatermark""
                                                           },
                                                            ""dataDeletionDetectionPolicy"": {
                                                                                                ""@odata.type"": ""#Micsoft.Azure.Search.SoftDeleteColumnDeletionDetectionPolicy"",
                                                                                                ""softDeleteColumnName"": ""deleted"",
                                                                                                ""softDeleteMarkerValue"": ""true""
                                                                                             }
                        }";
        }

        static string IndexDefinition()
        {
            return @"
                        {
                            ""fields"": [{

                                        ""name"": ""id"",
		                                ""type"": ""Edm.Int32"",
		                                ""searchable"": false,
		                                ""filterable"": false,
		                                ""retrievable"": true,
		                                ""sortable"": false,
		                                ""facetable"": false,
		                                ""key"": false

                                    }, {
		                                ""name"": ""versionValue"",
		                                ""type"": ""Edm.Int32"",
		                                ""searchable"": false,
		                                ""filterable"": true,
		                                ""retrievable"": true,
		                                ""sortable"": true,
		                                ""facetable"": true,
		                                ""key"": false
		                                }, {
		                                ""name"": ""uuid"",
		                                ""type"": ""Edm.String"",
		                                ""searchable"": true,
		                                ""filterable"": true,
		                                ""retrievable"": true,
		                                ""sortable"": true,
		                                ""facetable"": true,
		                                ""key"": false
	                                }, {
		                                ""name"": ""createdBy"",
		                                ""type"": ""Edm.String"",
		                                ""searchable"": true,
		                                ""filterable"": true,
		                                ""retrievable"": true,
		                                ""sortable"": true,
		                                ""facetable"": true,
		                                ""key"": false
	                                }, {
		                                ""name"": ""createdDate"",
		                                ""type"": ""Edm.String"",
		                                ""searchable"": true,
		                                ""filterable"": true,
		                                ""retrievable"": true,
		                                ""sortable"": true,
		                                ""facetable"": true,
		                                ""key"": false
	                                }, {
		                                ""name"": ""lastModifiedBy"",
		                                ""type"": ""Edm.String"",
		                                ""searchable"": true,
		                                ""filterable"": true,
		                                ""retrievable"": true,
		                                ""sortable"": true,
		                                ""facetable"": true,
		                                ""key"": false
	                                }, {
		                                ""name"": ""lastModifiedDate"",
		                                ""type"": ""Edm.String"",
		                                ""searchable"": true,
		                                ""filterable"": true,
		                                ""retrievable"": true,
		                                ""sortable"": true,
		                                ""facetable"": true,
		                                ""key"": false
	                                }],
                                    ""corsoptions"": 
                                                    {
                                                        ""allowedorigins"": [""*""],
                                                        ""maxAgeInSeconds"": 300
                                                    }
                        }";
        }

        static string IndexerDefinition(string dataSourceName, string indexName)
        {
            return @"
                        {
                            ""description"": ""indexer for azure blob storage"",
                            ""dataSourceName"": """ + dataSourceName + @""",
                            ""targetIndexName"": """ + indexName + @""",
                            ""schedule"": { ""interval"": ""P1D"" }
                        }";
        }
    }
}
