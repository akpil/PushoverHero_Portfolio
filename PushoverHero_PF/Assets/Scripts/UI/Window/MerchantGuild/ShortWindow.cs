using System;
using Config.Data;
using Controllers;
using Controllers.MerchantGuild;
using TMPro;
// using UI.Components.GraphDrawing;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Window.MerchantGuild
{
    public class ShortWindow : UIBase
    {
        // [SerializeField]
        // private UIGraphDrawer _graph;

        [SerializeField]
        private Button _purchaseButton,
                       _purchase10Button,
                       _sellButton,
                       _sell10Button;

        [SerializeField]
        private TextMeshProUGUI _mainTimeText,
                                _profitLoseText,
                                _holdingAmountText,
                                _currencyText;

        [SerializeField]
        private Image _newTickGauge;
        
        private void Awake()
        {
            ShortTremTrade.Instance.OnRefresh += RefreshButtonStatus;
            ShortTremTrade.Instance.OnChartRefresh += ChartUpdate;
            ShortTremTrade.Instance.OnMainTimeTick += MainTimeTick;
            ShortTremTrade.Instance.OnNewPriceTimeTick += SubTimeTick;

            _purchaseButton.onClick.AddListener(() => ShortTremTrade.Instance.Purchase(1));
            _purchase10Button.onClick.AddListener(() => ShortTremTrade.Instance.Purchase(10));
            _sellButton.onClick.AddListener(() => ShortTremTrade.Instance.Sell(1));
            _sell10Button.onClick.AddListener(() => ShortTremTrade.Instance.Sell(10));
        }

        private void Start()
        {
            ShortTremTrade.Instance.ForceRefresh();
        }

        private void RefreshButtonStatus()
        {
            _sellButton.interactable = ShortTremTrade.Instance.HoldingAmount >= 1;
            _sell10Button.interactable = ShortTremTrade.Instance.HoldingAmount >= 10;

            var price = ShortTremTrade.Instance.CurrentPrice;
            var currency = UserDataController.Instance.Currency;
            _purchaseButton.interactable = price <= currency;
            _purchase10Button.interactable = price * 10 <= currency;


            float profitLose = (float)ShortTremTrade.Instance.ProfitLose;
            char plusMinus = profitLose > 0 ? '+' : '-';
            _profitLoseText.text = $"{plusMinus} {UnitSetter.SetMoneyUnit(Mathf.Abs(profitLose))}";
            _holdingAmountText.text = UnitSetter.SetMoneyUnit(ShortTremTrade.Instance.HoldingAmount);
            _currencyText.text = UnitSetter.SetMoneyUnit(currency);
        }

        private void ChartUpdate(ChartData data)
        {
            // _graph.DrawGraph(data.Stock, 8, data.Stock.Count - 8, eGraphAnimationType.LastPointAnim);
        }

        private void MainTimeTick(float currentTime, float maxTime)
        {
            _mainTimeText.text = UnitSetter.FloatToTimeS(maxTime - currentTime);
        }

        private void SubTimeTick(float currentTime, float maxTime)
        {
            _newTickGauge.fillAmount = currentTime / maxTime;
        }

        private void OnDestroy()
        {
            ShortTremTrade.Instance.OnRefresh -= RefreshButtonStatus;
            ShortTremTrade.Instance.OnChartRefresh -= ChartUpdate;
            ShortTremTrade.Instance.OnMainTimeTick -= MainTimeTick;
            ShortTremTrade.Instance.OnNewPriceTimeTick -= SubTimeTick;
        }
    }
}