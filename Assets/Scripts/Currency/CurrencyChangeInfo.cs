using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Currency
{
    public struct CurrencyChangeInfo
    {
        public readonly int previousCurrency;
        public readonly int currentCurrency;
        public readonly int difference;
        public readonly int absoluteDifference;

        public CurrencyChangeInfo(int previous, int current)
        {
            previousCurrency = previous;
            currentCurrency = current;
            difference = currentCurrency - previousCurrency;
            absoluteDifference = Mathf.Abs(difference);
        }
    }
}
