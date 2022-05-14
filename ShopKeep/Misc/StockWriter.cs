using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ShopKeepDB.Models;

namespace ShopKeep.Misc
{
    public static class StockWriter
    {
        private static string StockToString(List<ShopStock> shopStock)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ShopStock stock in shopStock)
            {
                builder.AppendLine($"{stock.Item.Name}      " +
                                   $"{stock.ShopStockPrice.Gold} Gold, " +
                                   $"{stock.ShopStockPrice.Silver} Silver, " +
                                   $"{stock.ShopStockPrice.Copper} Copper      " +
                                   $"In Stock: {stock.Amount}");
            }
            return builder.ToString();
        }

        public static async Task<string> StockToFile(Shop shop, List<ShopStock> stock)
        {
            try
            {
                string toWrite = StockToString(stock);
                string fileName = $"{shop.Name}_{shop.Id}.txt";
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile file;
                try
                {
                    file = await localFolder.GetFileAsync(fileName);
                }
                catch (FileNotFoundException)
                {
                    file = await localFolder.CreateFileAsync(fileName);
                }
                await FileIO.WriteTextAsync(file, toWrite);
                return Path.Combine(localFolder.Path, fileName);
            }
            catch (Exception e) when (e is UnauthorizedAccessException 
                                      || e is NotSupportedException
                                      || e is ArgumentException
                                      || e is DirectoryNotFoundException
                                      || e is IOException
                                      || e is InvalidOperationException)
            {
                return "";
            }
        }
    }
}
