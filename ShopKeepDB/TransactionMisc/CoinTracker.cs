using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ShopKeepDB.TransactionMisc
{
    public class CoinTracker : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _gold = 0;
        private int _silver = 0;
        private int _copper = 0;

        public int Gold
        {
            get => _gold;
            set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public int Silver
        {
            get => _silver;
            set
            {
                _silver = value;
                OnPropertyChanged();
            }
        }

        public int Copper
        {
            get => _copper;
            set
            {
                _copper = value;
                OnPropertyChanged();
            }
        }

        public void UpdateValues(int gold, int silver, int copper)
        {
            int copperChange = Copper;
            int silverChange = Silver;
            int goldChange = Gold;
            copperChange += copper;
            if (copperChange > 10)
            {
                silverChange += copperChange / 10;
                copperChange %= 10;
            }

            if (copperChange < 0)
            {
                silverChange -= (-1 * copperChange) / 10;
                copperChange = 10 - copperChange % 10;
            }

            silverChange += silver;

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
