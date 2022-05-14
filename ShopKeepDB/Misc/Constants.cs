using System;
using System.Collections.Generic;
using System.Text;

namespace ShopKeepDB.Misc
{
    public static class Constants
    {
        public static readonly string[] Rarities = {"Common", "Uncommon", "Rare", "Other"};
        public static readonly string[] ShopLocales = {"Other", "Rural", "Town", "City", "Metropolitan"};
        public const int SaltSize = 32;
        public const int HashSize = 32;
        public const int IterationCount = 1000;

        public static Dictionary<string, Dictionary<string, Tuple<int, int>>> ItemChances = new()
        {
            {
                "Other", new Dictionary<string, Tuple<int, int>>()
                {
                    {"Common", new Tuple<int, int>(0, 70)},
                    {"Uncommon", new Tuple<int, int>(71, 96)},
                    {"Rare", new Tuple<int, int>(97, 99)},
                    {"Other", new Tuple<int, int>(100, 100)},
                }
            },
            {
                "Rural", new Dictionary<string, Tuple<int, int>>()
                {
                    {"Common", new Tuple<int, int>(0, 65)},
                    {"Uncommon", new Tuple<int, int>(66, 96)},
                    {"Rare", new Tuple<int, int>(97, 99)},
                    {"Other", new Tuple<int, int>(100, 100)}
                }
            },
            {
                "Town", new Dictionary<string, Tuple<int, int>>()
                {
                    {"Common", new Tuple<int, int>(0, 62)},
                    {"Uncommon", new Tuple<int, int>(63, 92)},
                    {"Rare", new Tuple<int, int>(93, 98)},
                    {"Other", new Tuple<int, int>(99, 100)}
                }
            },
            {
                "City", new Dictionary<string, Tuple<int, int>>()
                {
                    {"Common", new Tuple<int, int>(0, 50)},
                    {"Uncommon", new Tuple<int, int>(51, 85)},
                    {"Rare", new Tuple<int, int>(86, 97)},
                    {"Other", new Tuple<int, int>(98, 100)}
                }
            },
            {
                "Metropolitan", new Dictionary<string, Tuple<int, int>>()
                {
                    {"Common", new Tuple<int, int>(0, 45)},
                    {"Uncommon", new Tuple<int, int>(46, 80)},
                    {"Rare", new Tuple<int, int>(81, 95)},
                    {"Other", new Tuple<int, int>(96, 100)}
                }
            }
        };

        public static Dictionary<string, Tuple<int, int>> PricePercentageDifferencePerLocale = new()
        {
            {"Other", new Tuple<int, int>(0, 15)},
            {"Rural", new Tuple<int, int>(0, 10)},
            {"Town", new Tuple<int, int>(5, 10)},
            {"City", new Tuple<int, int>(10, 10)},
            {"Metropolitan", new Tuple<int, int>(15, 10)},
        };
    }
}
