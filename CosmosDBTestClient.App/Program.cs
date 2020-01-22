using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using CosmosDBTestClient.Core.Utils;

namespace CosmosDBTestClient.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var client = new DocumentClient(new Uri(DatabaseSettings.endpointUri), DatabaseSettings.authKey))
            {
                await AppMenu.RenderMenuAsync(client);
            }
            Console.Clear();
        }
    }
}
