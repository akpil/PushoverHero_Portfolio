using Config.Data;
using Controllers.MerchantGuild;
using UnityEngine;

namespace Controllers 
{ 
    public class DemoController : MonoBehaviour
    {
        [SerializeField]
        private ChartData _chartData;

        private void Start()
        {
            ShortTremTrade.Instance.Initialize(ref _chartData, 60, 4);
            SceneController.Instance.UnloadLoadingScreen();
        }
    }
}
