using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OverflowException = System.OverflowException;

namespace ShopKeepDB.TransactionMisc
{
    public class CoinTracker
    {
        private int _gold = 0;
        private int _silver = 0;
        private int _copper = 0;

        public int Gold
        {
            get => _gold;
            set => _gold = value;
        }

        public int Silver
        {
            get => _silver;
            set => _silver = value;
        }

        public int Copper
        {
            get => _copper;
            set => _copper = value;
        }

        public void UpdateValues(int gold, int silver, int copper)
        {
            var copperChange = Copper;
            var silverChange = Silver;
            var goldChange = Gold;
            copperChange += copper;
            if (copperChange > 10)
            {
                silverChange += copperChange / 10;
                copperChange %= 10;
            }

            try
            {
                checked
                {

                    if (copperChange < 0)
                    {
                        silverChange -= (-1 * copperChange) / 10;
                        copperChange = 10 - copperChange % 10;
                    }
                }
            }
            catch (OverflowException)
            {
                copperChange = 0;
                silverChange = 0;
            }

            silverChange += silver;

            try
            {
                checked
                {
                    if (silverChange > 10)
                    {
                        goldChange += silver / 10;
                        silverChange %= 10;
                    }

                    if (silverChange < 0)
                    {
                        goldChange -= (-1 * silverChange) / 10;
                        silverChange = 10 - silverChange % 10;
                    }

                    goldChange += gold;

                    if (goldChange < 0)
                    {
                        goldChange = 0;
                    }
                }
            }
            catch (OverflowException)
            {
                silverChange = 0;
                goldChange = int.MaxValue;
            }

            if (copperChange != _copper)
            {
                Copper = copperChange;
            }
            if (silverChange != _silver)
            {
                Silver = silverChange;
            }

            if (goldChange != _gold)
            {
                Gold = goldChange;
            }
        }
    }
}
