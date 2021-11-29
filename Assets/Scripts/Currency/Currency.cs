using System;

namespace TowerDefense.Currency
{
    public class Currency
    {
        public int CurrentCurrency { get; private set; }
        public event Action<CurrencyChangeInfo> CurrencyChanged;

        public Currency(int startingCurrency)
        {
            ChangeCurrency(startingCurrency);
        }

        public void AddCurrency(int increment)
        {
            ChangeCurrency(increment);
        }
        
        public bool TryPurchase(int cost)
        {
            if (!CanAfford(cost))
            {
                return false;
            }

            ChangeCurrency(-cost);
            return true;
        }

        public bool CanAfford(int cost)
        {
            return CurrentCurrency >= cost;
        }

        private void ChangeCurrency(int increment)
        {
            if (increment != 0)
            {
                var previousCurrency = CurrentCurrency;
                CurrentCurrency += increment;
                var currentCurrency = CurrentCurrency;
                var info = new CurrencyChangeInfo(previousCurrency, currentCurrency);
                CurrencyChanged?.Invoke(info);
            }
        }
    }
}