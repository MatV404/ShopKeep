using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Create;
using ShopKeepDB.Operations.Delete;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShopKeepDB.TransactionMisc
{
    public class ShopPurchaseHandler
    {
        private readonly List<ShopStock> _toDelete;
        private readonly List<BuyStock> _updateFailed;
        private readonly List<BuyStock> _successfulPurchases;
        private readonly User _customer;
        private readonly ObservableCollection<BuyStock> _stock;
        private readonly CoinTracker _tracker;

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

        /// <summary>
        /// This method handles the purchase of items from a shop, indicating whether the item was
        /// completely bought out, or some stock remains in the shop.
        /// </summary>
        /// <returns>Success on success, PartialSuccess if some transactions failed, Failure if a fatal error occurred.</returns>
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
