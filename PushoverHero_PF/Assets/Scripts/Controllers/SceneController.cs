using Utility;
using Config.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UI;


namespace Controllers
{
    public class SceneController : Singleton<SceneController>
    {
        private eSceneType _loadTarget;
        
        public SceneController()
        {
            
        }

        public void LoadLoadingScreen()
        {
            SceneManager.LoadSceneAsync((int)eSceneType.LoadingScreen, LoadSceneMode.Additive);
        }
        
        public void UnloadLoadingScreen()
        {
            SceneManager.UnloadSceneAsync((int)eSceneType.LoadingScreen);
        }
        public void SceneShift(eSceneType scene)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            AsyncOperation operation = SceneManager.LoadSceneAsync((int)eSceneType.LoadingScreen, LoadSceneMode.Additive);

            operation.completed += (operation) => UnloadScene(currentScene, scene);
        }

        private void UnloadScene(Scene currentScene, eSceneType scene)
        {
            //ResourceManager.Instance.CleanUpObjPools();

            var operation = SceneManager.UnloadSceneAsync(currentScene);
            operation.completed += (operation) => LoadScene(scene);
        }

        private void LoadScene(eSceneType scene)
        {
            var operation = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
            operation.completed += (operation) => AfterLoadScene(scene);
        }

        private void AfterLoadScene(eSceneType scene)
        {
            UIManager.Instance.CloseAllUI();

            switch (scene)
            {
                case eSceneType.Title:


                    break;
                case eSceneType.Lobby:

                    break;
                case eSceneType.Scenario:

                    break;
                case eSceneType.LoadingScreen:
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(scene), scene, null);
            }
        }
    }
}
