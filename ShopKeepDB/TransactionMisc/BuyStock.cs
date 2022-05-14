using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Update;
using ShopKeepDB.TransactionMisc;

namespace ShopKeepDB.TransactionMisc
{
    public class BuyStock : TransactionItem
    {
        public ShopStock OriginalShopStock { get; }

        public BuyStock(ShopStock original, int amount) : base(amount, 
                                                               original.ShopStockPrice.Gold, 
                                                               original.ShopStockPrice.Silver, 
                                                               original.ShopStockPrice.Copper)
        {
            OriginalShopStock = original;
        }

        public async Task<SingleTransactionResult> BuyAsync()
        {
            if (OriginalShopStock.Amount - Amount <= 0)
            {
                return SingleTransactionResult.ItemSoldOut;
            }

            if (!await ShopStockUpdate.ChangeShopStockAmountAsync(OriginalShopStock,
                    OriginalShopStock.Amount - Amount))
            {
                return SingleTransactionResult.TransactionFailure;
            }

            return SingleTransactionResult.ItemUpdated;
        }
    }
}
