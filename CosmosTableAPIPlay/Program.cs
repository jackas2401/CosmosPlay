using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace CosmosTableAPIPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString =
                @"DefaultEndpointsProtocol=https;AccountName=table-api-audit;AccountKey=RL2GOiD6lvPLmrHmkmtgPNG9211G1jFd7wyfGEXlyNb3GJHJaON5MIIoWWwzGobyRCsPy3BaYauwn2AVfRo6Lw==;TableEndpoint=https://table-api-audit.table.cosmosdb.azure.com:443/;";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);

            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("")



            Console.WriteLine("Table API Finished!");
            Console.ReadLine();
        }
    }
}
