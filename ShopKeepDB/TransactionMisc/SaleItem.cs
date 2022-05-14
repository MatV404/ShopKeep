using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Update;

namespace ShopKeepDB.TransactionMisc
{
    public class SaleItem : TransactionItem
    {
        private UserItem _originalUserItem;
        public string OriginalUserItemName => _originalUserItem.Item.Name;
        public UserItem OriginalUserItem => _originalUserItem;

        public SaleItem(UserItem originalItem, int amount) : base(amount,
                                                                  originalItem.Item.BaseItemPrice.Gold,
                                                                  originalItem.Item.BaseItemPrice.Silver,
                                                                  originalItem.Item.BaseItemPrice.Copper)
        {
            _originalUserItem = originalItem;
        }

        public async Task<SingleTransactionResult> SellAsync()
        {
            if (_originalUserItem.Amount - Amount <= 0)
            {
                return SingleTransactionResult.ItemSoldOut;
            }

            if (!await UserItemUpdate.ChangeUserItemAmountAsync(_originalUserItem, _originalUserItem.Amount - Amount))
            {
                return SingleTransactionResult.TransactionFailure;
            }

            return SingleTransactionResult.ItemUpdated;
        }
    }
}
