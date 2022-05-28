using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Update;
using System.Threading.Tasks;

namespace ShopKeepDB.TransactionMisc
{
    /// <summary>
    /// Represents an UserItem ready to be sold.
    /// </summary>
    public class SaleItem : TransactionItem
    {
        private readonly UserItem _originalUserItem;
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
