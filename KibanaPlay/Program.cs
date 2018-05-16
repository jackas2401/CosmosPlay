using CosmosPlay;
using System;
using System.Collections.Generic;
using Nest;

namespace KibanaPlay
{
    class Program
    {
        private ElasticClient Client;
        private List<AuditEvent> AuditData;
        private ISearchResponse<AuditEvent> AuditResults;
        private string IndexName = "jackman_audit";
        static void Main(string[] args)
       {
            Program p = new Program();
            p.SetupClient();
            //p.CreateNewIndex();
            //p.GenerateData();
            //p.IndexAuditEvents();
            //p.BulkIndexAuditEvents();
            Console.WriteLine("Enter search criteria:");
            p.SearchAuditEvents(Console.ReadLine());
            p.DisplayResults();
            Console.WriteLine("Program finished!");
            Console.ReadKey();
        }
        private void SetupClient()
        {
            var esNode = new Uri("https://0695f2603a6a0bd9efe4e05c40141ac6.eu-west-1.aws.found.io:9243/");

            var settings = new ConnectionSettings(esNode)
                .BasicAuthentication("elastic", "FvaimMkI3JBBwjpV2IHBjalQ")
                .DefaultIndex(IndexName);        

            Client = new ElasticClient(settings);
        }

        private void CreateNewIndex()
        {
            try
            {
                var createIndexResponse =
                    Client.CreateIndex(IndexName,
                        a => a.Mappings(ms => ms.Map<AuditEvent>(m => m.AutoMap())));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }

        private void GenerateData()
        {
            AuditData = AuditEventFactory.GeneratAuditEvents(0, 10, 0);
        }

        private void SearchAuditEvents(string searchString)
        {
            var searchResponse = Client.Search<AuditEvent>(
                a => a.From(0)
                    .Size(10)
                    .Query(q =>
                        q.Match(m => m                                
                            //.Field(audit => audit.NewObject)      // investigate how to do wildcard on nested object e.g. newObject.*                           
                            .Query(searchString)                            
                            .Lenient(true)
                            .Fuzziness(Fuzziness.Auto)
                        )
                    ));
            
            AuditResults = searchResponse;
        }

        private void DisplayResults()
        {
            Console.WriteLine("Records returned {0}", AuditResults.Documents.Count);

            Console.WriteLine(AuditResults.ToString());
        }

        private async void IndexAuditEvents()
        {
            try
            {
                foreach (var auditEvent in AuditData)
                {
                    var asyncIndexResponse = await Client.IndexDocumentAsync(auditEvent);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async void BulkIndexAuditEvents()
        {
            try
            {
                var asyncIndexResponse = await Client.IndexManyAsync(AuditData);                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
