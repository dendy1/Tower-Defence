using System;
using Core.Utilities.Timer;

namespace TowerDefense.Currency
{	
    [Serializable]
    public class CurrencyGainer
    {
        public int constantCurrencyAddition;
        public float constantCurrencyGainRate;
        public event Action<CurrencyChangeInfo> CurrencyChanged;
        
        public Currency Currency { get; set; }
        
        private RepeatingTimer _gainTimer;
        
        public void Initialize(Currency currencyController, int gainAddition, float gainRate)
        {
            constantCurrencyAddition = gainAddition;
            constantCurrencyGainRate = gainRate;
            Initialize(currencyController);
        }
        
        public void Initialize(Currency currencyController)
        {
            Currency = currencyController;
            UpdateGainRate(constantCurrencyGainRate);
        }
        
        public void Tick(float deltaTime)
        {
            if (_gainTimer == null)
            {
                return;
            }
            _gainTimer.Tick(deltaTime);
        }
        
        public void UpdateGainRate(float currencyGainRate)
        {
            constantCurrencyGainRate = currencyGainRate;
            if (currencyGainRate < 0)
            {
                throw new ArgumentOutOfRangeException("currencyGainRate");
            }
            if (_gainTimer == null)
            {
                _gainTimer = new RepeatingTimer(1 / constantCurrencyGainRate, ConstantGain);
            }
            else
            {
                _gainTimer.SetTime(1 / constantCurrencyGainRate);
            }
        }
        
        protected void ConstantGain()
        {
            var previousCurrency = Currency.CurrentCurrency;
            Currency.AddCurrency(constantCurrencyAddition);
            var currentCurrency = Currency.CurrentCurrency;
            var info = new CurrencyChangeInfo(previousCurrency, currentCurrency);
            CurrencyChanged?.Invoke(info);
        }
    }
}
