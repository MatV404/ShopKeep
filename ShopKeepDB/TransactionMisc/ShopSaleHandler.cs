using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Create;
using ShopKeepDB.Operations.Delete;

namespace ShopKeepDB.TransactionMisc
{
    public class ShopSaleHandler
    {
        private List<UserItem> _toDelete;
        private List<SaleItem> _updateFailed;
        private List<SaleItem> _successfulSales;
        private Shop _shop;
        private ObservableCollection<SaleItem> _toSell;
        private CoinTracker _tracker;

        private Task DeductFailedTransactionsFromCoin()
        {
            foreach (var failed in _updateFailed)
            {
                _tracker.UpdateValues(failed.TotalPriceGold * -1,
                                      failed.TotalPriceSilver * -1,
                                      failed.TotalPriceCopper * -1);
            }

            return Task.CompletedTask;
        }
        public async Task<BulkTransactionResult> HandleSaleAsync()
        {
            bool allSuccessful = true;
            foreach (var item in _toSell)
            {
                var result = await item.SellAsync();
                if (result != SingleTransactionResult.TransactionFailure)
                {
                    _successfulSales.Add(item);
                    if (result == SingleTransactionResult.ItemSoldOut)
                    {
                        _toDelete.Add(item.OriginalUserItem);
                    }
                }
                else
                {
                    _updateFailed.Add(item);
                    allSuccessful = false;
                }
            }

            var transferResult = await ShopStockCreator.CreateOrUpdateShopStock(_successfulSales, _shop.Id);
            if (!transferResult)
            {
                return BulkTransactionResult.Failure;
            }

            await DeductFailedTransactionsFromCoin();
            var deleteResult = await UserItemRemover.DeleteUserItemsAsync(_toDelete);
            if (!deleteResult)
            {
                return BulkTransactionResult.Failure;
            }

            return (allSuccessful) ? BulkTransactionResult.Success : BulkTransactionResult.PartialSuccess;
        }

        public ShopSaleHandler(Shop shop, ObservableCollection<SaleItem> toSell, CoinTracker tracker)
        {
            _toDelete = new List<UserItem>();
            _updateFailed = new List<SaleItem>();
            _successfulSales = new List<SaleItem>();
            _shop = shop;
            _tracker = tracker;
            _toSell = toSell;
        }
    }
}
