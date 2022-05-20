using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Delete
{
    public static class UserDeleter
    {
        /// <summary>
        /// Deletes the user along with their Coins and UserItem entries.
        /// </summary>
        /// <param name="user">The user that will be deleted.</param>
        /// <returns>True if all went well, else false.</returns>
        public static async Task<bool> DeleteUser(User user)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.User.Remove(user);
                database.Coins.Remove(user.Coins);
                var userItems = database.UserItem.Where(item => item.UserName == user.Name);
                database.UserItem.RemoveRange(userItems);
                await database.SaveChangesAsync();
                return true;
            }
            catch (Exception e) when (e is DbUpdateException
                                          or DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
