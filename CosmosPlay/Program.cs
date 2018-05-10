using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace CosmosPlay
{
    class Program
    {
        private const string EndpointUri = "https://jackman-audit.documents.azure.com:443/";
        private const string PrimaryKey = "YmwOBLjg3v9RBAfN61uOd82sJRlRNosEq6psQdmTa1zmRrnW9qQX2PyEc93XlSIEDv68LkgRmaRbzFOvzO1G1A==";
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

            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });

            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = CollectionName });

            List<AuditEvent> AuditEvents = AuditEventFactory.GeneratAuditEvents(100, 100, 100);

            await CreateAuditRecords(AuditEvents);
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
                //User user = new User()
                //{
                //    UserId = 1,
                //    Username = "Andrew"
                //};

                //await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), user);

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
            //try
            //{
            //    await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, family.Id));
            //    this.WriteToConsoleAndPromptToContinue("Found {0}", family.Id);
            //}
            //catch (DocumentClientException de)
            //{
            //    if (de.StatusCode == HttpStatusCode.NotFound)
            //    {
            //        await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), family);
            //        this.WriteToConsoleAndPromptToContinue("Created Family {0}", family.Id);
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
        }
    }
}
