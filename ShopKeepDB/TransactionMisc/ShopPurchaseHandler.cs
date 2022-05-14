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
    public class ShopPurchaseHandler
    {
        private List<ShopStock> _toDelete;
        private List<BuyStock> _updateFailed;
        private List<BuyStock> _successfulPurchases;
        private User _customer;
        private ObservableCollection<BuyStock> _stock;
        private CoinTracker _tracker;

        private async Task<bool> TransferItemsAsync()
        {
            var newItems = new List<UserItem>();
            foreach (var item in _successfulPurchases)
            {
                newItems.Add(new UserItem(item.OriginalShopStock.ItemId, _customer.Name, item.Amount));
            }

            return await UserItemCreator.CreateOrUpdateUserItems(newItems);
        }

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

        public async Task<BulkTransactionResult> HandlePurchaseAsync()
        {
            bool allSuccessful = true;
            foreach (var item in _stock)
            {
                var result = await item.BuyAsync();
                if (result != SingleTransactionResult.TransactionFailure)
                {
                    _successfulPurchases.Add(item);
                    if (result == SingleTransactionResult.ItemSoldOut)
                    {
                        _toDelete.Add(item.OriginalShopStock);
                    }
                }
                else
                {
                    _updateFailed.Add(item);
                    allSuccessful = false;
                }
            }

            var transferResult = await TransferItemsAsync();
            if (!transferResult)
            {
                return BulkTransactionResult.Failure;
            }
            await DeductFailedTransactionsFromCoin();
            var deleteResult = await ShopStockRemover.RemoveShopStockAsync(_toDelete);
            if (!deleteResult)
            {
                return BulkTransactionResult.Failure;
            }

            return (allSuccessful) ? BulkTransactionResult.Success : BulkTransactionResult.PartialSuccess;
        }

        public ShopPurchaseHandler(User customer, ObservableCollection<BuyStock> stock, CoinTracker tracker)
        {
            _toDelete = new List<ShopStock>();
            _updateFailed = new List<BuyStock>();
            _successfulPurchases = new List<BuyStock>();
            _customer = customer;
            _tracker = tracker;
            _stock = stock;
        }
    }
}
