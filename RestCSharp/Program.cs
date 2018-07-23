using System;
using System.Collections.Generic;
using System.Configuration;
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
            var searchServiceName = "";
            var apiKey = "";

            var dataSourceName = "";
            var indexName = "";
            var indexerName = "";

            var azureBlobConnectionString = "";
            var azureBlobTableName = "";

            using (var httpClient = new HttpClient())
            {
                var dataSourceDefinition = AzureBlobDatasourceDefinition(azureBlobConnectionString, azureBlobTableName);
                var putDataSourceRequest = PutDataSourceRequest(searchServiceName, apiKey, dataSourceName, dataSourceDefinition);
                Console.WriteLine($"Put data source {putDataSourceRequest.RequestUri}");
                Console.WriteLine();
                var putDataSourceResponse = httpClient.SendAsync(putDataSourceRequest).Result;
                var putDataSourceResponseContent = putDataSourceResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(putDataSourceResponseContent);
                Console.WriteLine();

                var indexDefinition = IndexDefinition();
                var putIndexRequest = PutIndexRequest(searchServiceName, apiKey, indexName, indexDefinition);
                Console.WriteLine($"Put index {putIndexRequest.RequestUri}");
                Console.WriteLine();
                var putIndexResponse = httpClient.SendAsync(putIndexRequest).Result;
                var putIndexResponseContent = putIndexResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(putIndexResponseContent);
                Console.WriteLine();

                var indexerDefinition = IndexerDefinition(dataSourceName, indexName);
                var putIndexerRequest = PutIndexerRequest(searchServiceName, apiKey, indexerName, indexerDefinition);
                Console.WriteLine($"Put indexer {putIndexerRequest.RequestUri}");
                Console.WriteLine();
                var putIndexerResponse = httpClient.SendAsync(putIndexerRequest).Result;
                var putIndexerResponseContent = putIndexerResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(putIndexerResponseContent);
                Console.WriteLine();

                var runIndexerRequest = RunIndexerRequest(searchServiceName, apiKey, indexerName);
                Console.WriteLine($"Run indexer {runIndexerRequest.RequestUri}");
                Console.WriteLine();
                var runIndexerResponse = httpClient.SendAsync(runIndexerRequest).Result;
                Console.WriteLine($"Success: {runIndexerResponse.IsSuccessStatusCode}");
                Console.ReadLine();
            }
        }

        static HttpRequestMessage PutDataSourceRequest(string searchServiceName, string apiKey, string dataSourceName,
            string datasourceDefinition)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
                $"https://{searchServiceName}.search.windows.net/datasources/{dataSourceName}?api-version=2015-02-28-Preview");
            request.Headers.Add("api-key", apiKey);
            var body = new StringContent(datasourceDefinition, Encoding.UTF8, "application/json");
            request.Content = body;
            return request;
        }

        static HttpRequestMessage PutIndexRequest(string searchServiceName, string apiKey, string indexName,
            string indexDefinition)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
                $"https://{searchServiceName}.search.windows.net/indexes/{indexName}?api-version=2015-02-28-Preview");
            request.Headers.Add("api-key", apiKey);
            var body = new StringContent(indexDefinition, Encoding.UTF8, "application/json");
            request.Content = body;
            return request;
        }

        static HttpRequestMessage PutIndexerRequest(string searchServiceName, string apiKey, string indexerName,
            string indexerDefinition)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
                $"https://{searchServiceName}.search.windows.net/indexers/{indexerName}?api-version=2015-02-28-Preview");
            request.Headers.Add("api-key", apiKey);
            var body = new StringContent(indexerDefinition, Encoding.UTF8, "application/json");
            request.Content = body;
            return request;
        }

        static HttpRequestMessage RunIndexerRequest(string searchServiceName, string apiKey, string indexerName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://{searchServiceName}.search.windows.net/indexers/{indexerName}/run?api-version=2015-02-28-Preview");
            request.Headers.Add("api-key", apiKey);
            return request;
        }

        static string AzureBlobDatasourceDefinition(string connectionString, string tableName)
        {
            return @"
                        {
                          ""description"": ""azure blob datasource"",
                          ""type"": ""azureblob"",
                          ""credentials"": { ""connectionString"": """ + connectionString + @""" },
                          ""container"": { ""name"": """ + tableName + @""" },
                          ""dataChangeDetectionPolicy"": {
                            ""@odata.type"": ""#Microsoft.Azure.Search.HighWaterMarkChangeDetectionPolicy"",
                            ""highWaterMarkColumnName"": ""highwatermark""
                          },
                          ""dataDeletionDetectionPolicy"": {
                            ""@odata.type"": ""#Microsoft.Azure.Search.SoftDeleteColumnDeletionDetectionPolicy"",
                          ""softDeleteColumnName"": ""deleted"",
                          ""softDeleteMarkerValue"": ""true""
                          }
                        }
                        ";
        }

        static string IndexDefinition()
        {
            return @"
                        {
                                ""fields"": [
                                        {
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
                                        }, {
	                                        ""name"": ""company_id"",
	                                        ""type"": ""Edm.Int32"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""name"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""displayName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": true
                                        }, {
	                                        ""name"": ""salutation"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""firstName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""middleName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""lastName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""nickName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""suffix"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""gender_id"",
	                                        ""type"": ""Edm.Int32"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""businessUnit_id"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""dateOfBirth"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""contactTypes"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""accountingReference"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""referenceId"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""languageMappings"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""rating"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""numbers"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""lat"",
	                                        ""type"": ""Edm.GeographyPoint"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""lng"",
	                                        ""type"": ""Edm.GeographyPoint"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""addresses"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""emails"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""qualifications"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""stateDateSince"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""stateDateUntil"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""notes"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""documents"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""criteriaType"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""language_id"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""languageLabel"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""languageCode"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""supportingInformation"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""eligibilities"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""criteriaHierachy"",
	                                        ""type"": ""Collection(Edm.String)"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""hasTransportation"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""hasChildren"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""companyName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""website"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""region"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""countryOfOrigin"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""countryOfResidence"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""countryOfNationality"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""active"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""activeNote"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""availability"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""experience"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""registeredTaxId"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""bankAccount"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""sortCode"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""iban"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""swift"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""eft_id"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""eft_name"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""paymentMethod_id"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""paymentMethod_name"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""paymentAccount"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""registeredTax"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""registeredTaxIdDescription"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""employmentCategoryId"",
	                                        ""type"": ""Edm.Int32"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""assignmentTier_id"",
	                                        ""type"": ""Edm.Int32"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""timeZone"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""ethnicity"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""document"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""imagePath"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""outOfOffice"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""disableUpcomingReminder"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""disableCloseReminder"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""disableConfirmReminder"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""bankAccountDescription"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""timeWorked"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""activationDate"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""originalStartDate"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""datePhotoSentToPrinter"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""datePhotoSentToInterpreter"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""inductionDate"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""reActivationDate"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""iolNrcpdNumber"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": false,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""referralSource"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""refereeSourceName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""recruiterName"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""taleoId"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""bankAccountReference"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""disableConfimationEmails"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""disableOfferEmails"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""disableAutoOffers"",
	                                        ""type"": ""Edm.Boolean"",
	                                        ""searchable"": false,
	                                        ""filterable"": false,
	                                        ""retrievable"": false,
	                                        ""sortable"": false,
	                                        ""facetable"": false,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""currencyCode_id"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""currencySymbol"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }, {
	                                        ""name"": ""bankBranch"",
	                                        ""type"": ""Edm.String"",
	                                        ""searchable"": true,
	                                        ""filterable"": true,
	                                        ""retrievable"": true,
	                                        ""sortable"": true,
	                                        ""facetable"": true,
	                                        ""key"": false
                                        }],
                                    ""corsOptions"": 
                                                    {
                                                        ""allowedOrigins"": [""*""],
                                                        ""maxAgeInSeconds"": 300
                                                    }
                        }";
        }

        static string IndexerDefinition(string dataSourceName, string indexName)
        {
            return @"
                    {
                        ""description"": ""indexer for azure blob storage database"",
                        ""dataSourceName"":  """ + dataSourceName + @""",
                        ""targetIndexName"": """ + indexName + @""",
                        ""schedule"": { ""interval"": ""P1D""}
                    }
                    ";
        }
    }
}
