#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Utility
{
    public class AssetGuidContainer : MonoBehaviour
    {
        [field: SerializeField]
        public string AssetGuid { get; private set; }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(AssetGuid))
            {
                ResetAssetGuid();
            }
        }

        [ContextMenu("Reset AssetGUID")]
        private void ResetAssetGuid()
        {
            var path = AssetDatabase.GetAssetPath(gameObject);
            AssetGuid = AssetDatabase.AssetPathToGUID(path);
        }
        
        [CustomEditor(typeof(AssetGuidContainer))]
        public class AssetGuidContainerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                AssetGuidContainer script = (AssetGuidContainer)target;
                
                if (GUILayout.Button("Reset AssetGUID"))
                {
                    script.ResetAssetGuid();
                }
            }
        }
#endif
    }
}