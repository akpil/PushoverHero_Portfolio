using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Window
{
    public class TitleWindow : UIBase
    {
        [SerializeField]
        private Button _startButton;
        private void Start()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            TitleController.Instance.OnStartGame();
        }
    }
}
