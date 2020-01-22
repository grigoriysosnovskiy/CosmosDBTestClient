using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using CosmosDBTestClient.Core.Utils;
using CosmosDBTestClient.Core.Models;
using System.Linq;

namespace CosmosDBTestClient.Core.Handlers
{
    public static class DatabaseHandler
    {
        #region CreateDatabaseItemsIfNotExists
        public static async Task CreateDatabaseItemsIfNotExists(DocumentClient client)
        {
            // Create folders DB and info collection if not exists
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseSettings.foldersDB });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseSettings.foldersDB), new DocumentCollection { Id = DatabaseSettings.foldersCollection });

            // Create assets DB and info collection if not exists
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseSettings.assetsDB });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseSettings.assetsDB), new DocumentCollection { Id = DatabaseSettings.assetsCollection });
        }
        #endregion

        #region CreateFolder
        private static dynamic CreateFolder()
        {
            object folder = null;

            Console.Write("Enter name -> ");
            string name = Console.ReadLine();

            folder = new FolderModel
            {
                Name = name,
                Path = DatabaseSettings.folderPath
            };

            return folder;
        }
        #endregion

        #region CreateFolderAsync
        public static async Task CreateFolderAsync(DocumentClient client)
        {
            while (true)
            {
                Console.Write("Create new folder, y/n -> ");
                var key = Console.ReadKey().Key;
                Console.WriteLine();
                if (key == ConsoleKey.Y)
                {
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.foldersDB, DatabaseSettings.foldersCollection), CreateFolder());
                }
                else
                {
                    break;
                }
            }
        }
        #endregion
        
        #region RenderCreateModelMenu
        private static string RenderCreateModelMenu()
        {
            List<string> menuItems = new List<string>()
            {
                "Create volcano"
            };

            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i] = menuItems[i].Trim().Insert(0, $"{i + 1}. ");
            }

            return string.Join("\n", menuItems.ToArray());
        }
        #endregion

        #region CreateModel
        private static dynamic CreateModel(DocumentClient client)
        {
            object model = null;

            Console.WriteLine(RenderCreateModelMenu());
            Console.Write(">> ");
            string number = Console.ReadLine();
            switch (number)
            {
                case "1": // Create volcano
                    Console.Clear();

                    Console.Write("Enter folder name to place asset there -> ");
                    string folderName = Console.ReadLine();
                    string query = $"SELECT * FROM c WHERE c.Name = \"{folderName}\"";
                    Document folder = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.foldersDB, DatabaseSettings.foldersCollection), query).AsEnumerable().FirstOrDefault();

                    Console.WriteLine("Randomize coordinates... ");
                    double[] coordinates = new double[3] { Math.Round(new Random().NextDouble() * (180 - 1) + 1, 2), Math.Round(new Random().NextDouble() * (180 - 1) + 1, 2), Math.Round(new Random().NextDouble() * (180 - 1) + 1, 2) };
                    System.Threading.Thread.Sleep(500);

                    Console.Write("Enter name -> ");
                    string name = Console.ReadLine();

                    Console.Write("Enter country -> ");
                    string country = Console.ReadLine();

                    Console.Write("Enter region -> ");
                    string region = Console.ReadLine();

                    Console.Write("Enter elevation -> ");
                    double elevation = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Enter status -> ");
                    string status = Console.ReadLine();

                    Console.Write("Enter last eruption -> ");
                    short lastEruption = Convert.ToInt16(Console.ReadLine());

                    model = new VolcanoModel
                    {
                        FolderId = folder.Id,
                        Coordinates = coordinates,
                        Name = name,
                        Country = country,
                        Region = region,
                        Elevation = Math.Round(elevation, 2),
                        Status = status,
                        LastEruption = lastEruption
                    };
                    break;
                default:
                    break;
            }

            return model;
        }
        #endregion
        
        #region CreateAssetAsync
        public static async Task CreateAssetAsync(DocumentClient client)
        {
            while (true)
            {
                Console.Write("Create new asset, y/n -> ");
                var key = Console.ReadKey().Key;
                Console.WriteLine();
                if (key == ConsoleKey.Y)
                {
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.assetsDB, DatabaseSettings.assetsCollection), CreateModel(client));
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        #region ShowAsset
        public static void ShowAsset(DocumentClient client)
        {
            while (true)
            {
                Console.Clear();

                Console.Write("Enter volcano name, y/n -> ");
                //var volcano = client.CreateDocumentQuery<VolcanoModel>(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.dbName, DatabaseSettings.collectionName)).Where(v => v.Name == Console.ReadLine()).AsEnumerable().FirstOrDefault();
                //Console.WriteLine(volcano.ToString());

                Console.Write("Show another volcano, y/n -> ");
                var key = Console.ReadKey().Key;
                Console.WriteLine();
                if (key != ConsoleKey.Y)
                {
                    break;
                }
            }
        }
        #endregion

        #region DeleteDatabaseAsync
        public static async Task DeleteDatabaseAsync(DocumentClient client)
        {
            Console.Write("Enter database name -> ");
            string nameDB = Console.ReadLine();
            Console.Write("Delete database, y/n -> ");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                await client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(nameDB));
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}
