using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class TitleController : MonoBehaviour
    {
        public static TitleController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnStartGame()
        {
            SceneController.Instance.SceneShift(Config.Enums.eSceneType.Lobby);
        }
    }
}