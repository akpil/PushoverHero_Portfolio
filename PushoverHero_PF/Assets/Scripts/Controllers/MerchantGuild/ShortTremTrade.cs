using System;
using System.Collections;
using Config.Data;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Controllers.MerchantGuild
{
    public class ShortTremTrade : MonoBehaviourSingleton<ShortTremTrade>
    {
        [SerializeField]
        private ChartData _chartData;
        public float CurrentPrice => _chartData.Stock[^1].y;
        
        public int HoldingAmount { get; private set; }
        public double ProfitLose { get; private set; }

        public Action OnRefresh { get; set; }
        public Action<ChartData> OnChartRefresh { get; set; }
        
        public Action<float, float> OnMainTimeTick { get; set; }
        public Action<float, float> OnNewPriceTimeTick { get; set; }

        private Coroutine _tradeRoutine;

        private double _ShortStartEstate;

        public void Initialize(ref ChartData data, float totalTime, float newPriceTime)
        {
            _ShortStartEstate = UserDataController.Instance.Currency;
            HoldingAmount = 0;
            _chartData = data.GetClone();
            _chartData.Stock = data.Stock.GetRange(data.Stock.Count - 2, 2);
            
            OnChartRefresh?.Invoke(_chartData);
            
            _tradeRoutine = StartCoroutine(Trade(totalTime, newPriceTime));
        }
        public void ForceRefresh()
        {
            OnChartRefresh?.Invoke(_chartData);
        }
        private void UpdateProfitLoss()
        {
            var price = _chartData.Stock[^1].y;
            UnityEngine.Debug.Log(price);
            ProfitLose = (price * HoldingAmount + UserDataController.Instance.Currency) - _ShortStartEstate;
        }

        public void Purchase(int amount)
        {
            var price = _chartData.Stock[^1].y;
            var totalPrice = price * amount;
            UnityEngine.Debug.Log(price);

            UserDataController.Instance.TransactionCurrency += (value) =>
            {
                HoldingAmount += amount;
                UpdateProfitLoss();
                OnRefresh?.Invoke();
            };
            UserDataController.Instance.Currency -= totalPrice;
        }



        public void Sell(int amount)
        {
            if (HoldingAmount < amount)
            {
                UnityEngine.Debug.LogError($"Wrong on short sell! holding {HoldingAmount}/ try sell {amount}");
                return;
            }
            var price = _chartData.Stock[^1].y;
            var totalPrice = price * amount;

            UserDataController.Instance.TransactionCurrency += (value) =>
            {
                HoldingAmount -= amount;
                UpdateProfitLoss();
                OnRefresh?.Invoke();
            };
            UserDataController.Instance.Currency += totalPrice;
        }

        private void UpdateChart()
        {
            var weight = Random.Range(-0.2f, 0.2f); // -20% ~ 20% simple random weight
            var lastData = _chartData.Stock[^1];
            var newPrice = lastData.y * (1 + weight);
            _chartData.Stock.Add(new Vector2(lastData.x + 1, newPrice));

            UpdateProfitLoss();
            OnChartRefresh?.Invoke(_chartData);
            OnRefresh?.Invoke();
        }
        
        private IEnumerator Trade(float totalTime, float newPriceTime)
        {
            float time = 0, 
                  subTime = 0;
            
            
            while (time < totalTime)
            {
                time += Time.fixedDeltaTime;
                subTime += Time.fixedDeltaTime;

                if (subTime >= newPriceTime)
                {
                    UpdateChart();
                    subTime -= newPriceTime;
                }
                
                OnMainTimeTick?.Invoke(time, totalTime);
                OnNewPriceTimeTick?.Invoke(subTime, newPriceTime);
                yield return CoroutineManager.FixedUpdate;
            }
        }
    }
}