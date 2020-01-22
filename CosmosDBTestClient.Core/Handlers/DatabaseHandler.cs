using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using CosmosDBTestClient.Core.Models;
using CosmosDBTestClient.Core.Utils;

namespace CosmosDBTestClient.Core.Handlers
{
    public static class DatabaseHandler
    {
        #region CreateDatabaseItemsIfNotExists
        public static async Task CreateDatabaseItemsIfNotExists(DocumentClient client)
        {
            try
            {
                // Create folders DB and info collection if not exists
                await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseSettings.foldersDB });
                await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseSettings.foldersDB), new DocumentCollection { Id = DatabaseSettings.foldersCollection });

                // Create assets DB and info collection if not exists
                await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseSettings.assetsDB });
                await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseSettings.assetsDB), new DocumentCollection { Id = DatabaseSettings.assetsCollection });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(500);
            }
        }
        #endregion

        #region CreateFolder
        private static dynamic CreateFolder()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(500);
                throw;
            }
        }
        #endregion

        #region CreateFolderAsync
        public static async Task CreateFolderAsync(DocumentClient client)
        {
            while (true)
            {
                try
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    System.Threading.Thread.Sleep(500);
                }
            }
        }
        #endregion

        #region GetFolderIdByName
        private static Document GetFolderIdByName(DocumentClient client)
        {
            Console.Write("Enter folder name -> ");
            string folderName = Console.ReadLine();
            string query = $"SELECT * FROM c WHERE c.Name = \"{folderName}\"";

            return client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.foldersDB, DatabaseSettings.foldersCollection), query).AsEnumerable().FirstOrDefault();
        }
        #endregion

        #region RenderModelMenu
        private static string RenderModelMenu()
        {
            try
            {
                List<string> menuItems = new List<string>()
                {
                    "Volcano"
                };

                for (int i = 0; i < menuItems.Count; i++)
                {
                    menuItems[i] = menuItems[i].Trim().Insert(0, $"{i + 1}. ");
                }

                return string.Join("\n", menuItems.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(500);
                throw;
            }
        }
        #endregion

        #region CreateModel
        private static dynamic CreateModel(DocumentClient client)
        {
            try
            {
                object model = null;

                Console.WriteLine("Create:");
                Console.WriteLine(RenderModelMenu());
                Console.Write(">> ");
                string number = Console.ReadLine();
                switch (number)
                {
                    case "1": // Create volcano
                        Console.Clear();
                        model = new VolcanoModel(GetFolderIdByName(client).Id);
                        break;
                    default:
                        break;
                }

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(500);
                throw;
            }
        }
        #endregion

        #region CreateAssetAsync
        public static async Task CreateAssetAsync(DocumentClient client)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(500);
            }
        }
        #endregion

        #region ShowAsset
        public static void ShowAsset(DocumentClient client)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Show:");
                    Console.WriteLine(RenderModelMenu());
                    Console.Write(">> ");
                    string number = Console.ReadLine();
                    switch (number)
                    {
                        case "1": // Show volcano
                            Console.Clear();
                            Console.Write("Enter volcano name, y/n -> ");
                            var volcano = client.CreateDocumentQuery<VolcanoModel>(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.assetsDB, DatabaseSettings.assetsCollection)).Where(v => v.Name == Console.ReadLine()).AsEnumerable().FirstOrDefault();
                            Console.WriteLine(volcano.ToString());
                            break;
                        default:
                            break;
                    }

                    Console.Write("Show another asset, y/n -> ");
                    var key = Console.ReadKey().Key;
                    Console.WriteLine();
                    if (key != ConsoleKey.Y)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(500);
            }
        }
        #endregion

        #region ShowAssets
        public static void ShowAssets(DocumentClient client)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    var assets = client.CreateDocumentQuery<VolcanoModel>(UriFactory.CreateDocumentCollectionUri(DatabaseSettings.assetsDB, DatabaseSettings.assetsCollection)).Where(v => v.FolderId == GetFolderIdByName(client).Id).ToList();
                    foreach (var asset in assets)
                    {
                        Console.WriteLine(asset.ToString());
                    }

                    Console.Write("Show another assets, y/n -> ");
                    var key = Console.ReadKey().Key;
                    Console.WriteLine();
                    if (key != ConsoleKey.Y)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(3500);
            }
        }
        #endregion

        #region DeleteDatabaseAsync
        public static async Task DeleteDatabaseAsync(DocumentClient client)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Threading.Thread.Sleep(500);
            }
        }
        #endregion
    }
}
