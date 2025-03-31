using Config.Enums;
using UnityEngine;

namespace UI
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] 
        private eUIId _uiID;
        public eUIId UIId => _uiID;

        [SerializeField] 
        private eUIType _uiType;
        public eUIType UIType => _uiType;
    }
}
