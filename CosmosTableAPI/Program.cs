using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace CosmosTableAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString =
                @"DefaultEndpointsProtocol=https;AccountName=table-api-audit;AccountKey=RL2GOiD6lvPLmrHmkmtgPNG9211G1jFd7wyfGEXlyNb3GJHJaON5MIIoWWwzGobyRCsPy3BaYauwn2AVfRo6Lw==;TableEndpoint=https://table-api-audit.table.cosmosdb.azure.com:443/;";

            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);

                CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();

                CloudTable table = tableClient.GetTableReference("Audit");

                // delete table if it exists
                table.Delete();

                table.CreateIfNotExists();

                List<AuditEvent> auditEvents = AuditEventFactory.GeneratAuditEvents(20, 20, 10);

                TableBatchOperation tableBatchOperation = new TableBatchOperation();

                auditEvents.ForEach(a => tableBatchOperation.Insert(a));

                table.ExecuteBatch(tableBatchOperation);
                
                // read all records
                TableQuery<AuditEvent> query = new TableQuery<AuditEvent>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "BOSHost"));

                var results = table.ExecuteQuery(query);

                foreach (AuditEvent audit in results)
                {
                    Console.WriteLine(audit.ToString());
                }

                Console.WriteLine("Table API Finished!");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

           
        }
    }
}

