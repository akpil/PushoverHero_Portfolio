using UnityEngine;

namespace Utility
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance 
        {
            get
            {
                if (_instance != null) return _instance;
                
                var go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }
        private static T _instance;
    }
}
