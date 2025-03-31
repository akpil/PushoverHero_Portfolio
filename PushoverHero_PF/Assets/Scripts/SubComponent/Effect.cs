using Config.Enums;
using ObjPool;
using UnityEngine;
using Utility;

namespace SubComponent
{
    [RequireComponent(typeof(AssetGuidContainer))]
    public class Effect : MonoBehaviour, IPoolElement<string>
    {
        [SerializeField, HideInInspector]
        private AssetGuidContainer _guidContainer;
        public string PoolElemID => _guidContainer.AssetGuid;
        public string DebuggingName => gameObject.name;

        private void OnValidate()
        {
            if (_guidContainer == null)
            {
                _guidContainer = GetComponent<AssetGuidContainer>();
            }
        }
        
        private IPool<string> _pool;
        
        public void SetPool(IPool<string> p)
        {
            _pool = p;
        }

        public void Return()
        {
            gameObject.SetActive(false);
            _pool.Return(this);
        }
    }
}
