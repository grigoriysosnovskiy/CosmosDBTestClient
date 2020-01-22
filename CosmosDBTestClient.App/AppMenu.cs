using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using CosmosDBTestClient.Core.Handlers;

namespace CosmosDBTestClient.App
{
    public static class AppMenu
    {
        #region RenderMenuItems
        private static string RenderMenuItems()
        {
            List<string> menuItems = new List<string>()
            {
                "Exit program",
                "Create folder(s)",
                "Create asset(s)",
                "Show asset",
                "Show assets in folder",
                "Copy folder with replace",
                "Copy folder with merge",
                "Copy folder with skip",
                "Delete database"
            };

            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i] = menuItems[i].Trim().Insert(0, $"{i}. ");
            }

            return string.Join("\n", menuItems.ToArray()); 
        }
        #endregion

        #region RenderMenuAsync
        public static async Task RenderMenuAsync(DocumentClient client)
        {
            Console.Write("Processing DB existence... ");
            await DatabaseHandler.CreateDatabaseItemsIfNotExists(client);

            // Render menu
            string number = "-1";
            do
            {
                Console.Clear();

                // TO DO

                // CopyFolder
                // replace/merge/skip. replace folder completely. merge content with existing folder and replace conflicting files. merge and skip conflicting files.

                // MoveFolder
                // merge/replace/skip. merge content with existing folder or replace existing folder (optional).

                Console.WriteLine(RenderMenuItems());
                Console.Write(">> ");
                number = Console.ReadLine();
                switch (number)
                {
                    case "0": // Exit program
                        Console.Clear();
                        Console.Write("Processing exit... ");
                        System.Threading.Thread.Sleep(500);
                        break;
                    case "1": // Create folder(s)
                        Console.Clear();
                        await DatabaseHandler.CreateDatabaseItemsIfNotExists(client);
                        await DatabaseHandler.CreateFolderAsync(client);
                        break;
                    case "2": // Create asset(s)
                        Console.Clear();
                        await DatabaseHandler.CreateDatabaseItemsIfNotExists(client);
                        await DatabaseHandler.CreateAssetAsync(client);
                        break;
                    case "3": // Show asset
                        Console.Clear();
                        DatabaseHandler.ShowAsset(client);
                        break;
                    case "4": // Show assets in folder
                        Console.Clear();
                        //DatabaseHandler.ShowAsset(client);
                        break;
                    case "5": // Copy folder with replace
                        Console.Clear();
                        //DatabaseHandler.ShowAsset(client);
                        break;
                    case "6": // Copy folder with merge
                        Console.Clear();
                        //DatabaseHandler.ShowAsset(client);
                        break;
                    case "7": // Copy folder with skip
                        Console.Clear();
                        //DatabaseHandler.ShowAsset(client);
                        break;
                    case "8": // Delete database
                        Console.Clear();
                        await DatabaseHandler.DeleteDatabaseAsync(client);
                        break;
                    default:
                        break;
                }
            } while (number != "0");
        }
        #endregion
    }
}
