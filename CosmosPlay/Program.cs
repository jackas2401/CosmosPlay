using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.SystemFunctions;
using Newtonsoft.Json;

namespace CosmosPlay
{
    class Program
    {
        //private const string EndpointUri = "https://jackman-audit.documents.azure.com:443/";
        //private const string PrimaryKey = "YmwOBLjg3v9RBAfN61uOd82sJRlRNosEq6psQdmTa1zmRrnW9qQX2PyEc93XlSIEDv68LkgRmaRbzFOvzO1G1A==";
        //private const string DatabaseName = "JackmanAudit";
        //private const string CollectionName = "AuditCollection";
        
        private const string EndpointUri = "https://localhost:8081/";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DatabaseName = "JackmanAudit";
        private const string CollectionName = "AuditCollection";

        private DocumentClient client;
        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        private async Task GetStartedDemo()
        {
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);

            try
            {
                await this.client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseName));
            }
            catch (DocumentClientException dce)
            {
                Console.WriteLine("Database not found!");                
            }
            

            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });

            DocumentCollection collectionDefinition = new DocumentCollection
            {
                Id = CollectionName
            };

            collectionDefinition.IndexingPolicy.Automatic = true;

            collectionDefinition.DefaultTimeToLive = -1;        // switch on TTL but no default expiry time
            collectionDefinition.PartitionKey.Paths.Add("/moduleId");

            // performance / load testing on indexes
            collectionDefinition.IndexingPolicy.IncludedPaths.Add(
                new IncludedPath
                {
                    Path = "/*",
                    Indexes = new Collection<Index>
                    {
                        new RangeIndex(DataType.Number) { Precision = -1, DataType = DataType.Number},
                        new RangeIndex(DataType.String) { Precision = -1, DataType = DataType.String}
                    }
                });

            collectionDefinition.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath {Path = "/newObject/?"});
            collectionDefinition.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath { Path = "/previousObject/?" });

            DocumentCollection auditCollection = await this.client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DatabaseName), 
                collectionDefinition,
                new RequestOptions { OfferThroughput = 20000 } );

            List<AuditEvent> AuditEvents = AuditEventFactory.GeneratAuditEvents(5, 5, 3);

            await CreateAuditRecords(AuditEvents);

            ExecuteSimpleQuery();
            ExecuteTextSearch();
            ExecuteSqlStringSearch();
            ExecuteDateSearch();
        }
        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private async Task CreateAuditRecords(List<AuditEvent> AuditEvents)
        {
            try
            {
                foreach (var auditEvent in AuditEvents)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), auditEvent);
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }

        private void ExecuteSimpleQuery()
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions
            {
                MaxItemCount = 10,  // more page size than max results
                PartitionKey = new PartitionKey("Profiles"),
                EnableScanInQuery = false,
                EnableCrossPartitionQuery = false
            };

            IQueryable<AuditEvent> auditQuery = this.client.CreateDocumentQuery<AuditEvent>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions)
                .Where(a => a.IpAddress == "127.0.0.1" &&
                            a.Action == AuditEvent.ActionType.Update);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");

            Console.WriteLine("No. of records found: {0}", auditQuery.Count());

            foreach (AuditEvent audit in auditQuery)
            {
                Console.WriteLine("\tRead {0}", audit);
            }            
        }

        private void ExecuteTextSearch()
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions
            {
                MaxItemCount = 10,  // more page size than max results
                PartitionKey = new PartitionKey("Profiles"),
                EnableScanInQuery = false,
                EnableCrossPartitionQuery = false
            };

            IQueryable<AuditEvent> auditQuery = this.client.CreateDocumentQuery<AuditEvent>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions)
                .Where(a => a.NewObject.Contains("Andrew") ||
                            a.PreviousObject.Contains("Andrew"));

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");

            Console.WriteLine("No. of records found: {0}", auditQuery.Count());

            foreach (AuditEvent audit in auditQuery)
            {
                Console.WriteLine("\tRead {0}", audit);
            }
        }

        private void ExecuteSqlStringSearch()
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions
            {
                MaxItemCount = 10,  // more page size than max results
                PartitionKey = new PartitionKey("Profiles"),
                EnableScanInQuery = false,
                EnableCrossPartitionQuery = false
            };

            // Now execute the same query via direct SQL
            IQueryable<AuditEvent> auditQueryInSql = this.client.CreateDocumentQuery<AuditEvent>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName),
                "SELECT * FROM c WHERE CONTAINS(c.PreviousObject, 'Andrew')",
                queryOptions);

            Console.WriteLine("Running direct SQL query...");

            foreach (AuditEvent audit in auditQueryInSql)
            {
                Console.WriteLine("\tRead {0}", audit);
            }
        }

        private void ExecuteDateSearch()
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions
            {
                MaxItemCount = 10,  // more page size than max results
                PartitionKey = new PartitionKey("Profiles"),
                EnableScanInQuery = false,
                EnableCrossPartitionQuery = false,
                EnableLowPrecisionOrderBy = true
            };

            DateTime starTime = DateTime.Now.AddHours(-1);
            DateTime endTime = DateTime.Now.AddHours(1);

            IQueryable<AuditEvent> auditQuery = this.client.CreateDocumentQuery<AuditEvent>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions)
                .Where(a => a.CreatedDate >= starTime &&
                            a.CreatedDate <= endTime)
                .OrderByDescending(a => a.CreatedDate);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");

            Console.WriteLine("No. of records found: {0}", auditQuery.Count());

            foreach (AuditEvent audit in auditQuery)
            {
                Console.WriteLine("\tRead {0}", audit);
            }
        }
    }
}
