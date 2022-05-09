using System;
using System.Collections.Generic;
using System.Text;

namespace ShopKeepDB.Misc
{
    public enum LoginResults
    {
        Invalid,
        User,
        Admin,
        Banned,
        DbError,
    }
}
