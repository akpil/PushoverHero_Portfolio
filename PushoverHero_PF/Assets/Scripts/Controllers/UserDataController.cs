using System;
using UnityEngine;
using Utility;

namespace Controllers
{
    public class UserDataController : Singleton<UserDataController>
    {
        class UserData
        {
            public double Currency;
        }

        private UserData _data = new()
        {
            Currency = 10000
        };

        public Action<double, double> OnCurrencyChanged { get; set; }
        public Action<bool> TransactionCurrency { get; set; }
        public double Currency 
        {
            get => _data.Currency;
            set
            {
                if (value > 0)
                {
                    OnCurrencyChanged?.Invoke(_data.Currency, value);
                    _data.Currency = value;
                }

                TransactionCurrency?.Invoke(value > 0);
                TransactionCurrency = null;
            }
        }
    }
}