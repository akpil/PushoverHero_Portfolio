using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MouseCursor : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        private RectTransform _rectTransform;
        
        private void Start()
        {
#if !UNITY_EDITOR
            Cursor.visible = false;
#endif
            _rectTransform = _canvas.GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            Vector2 mousePose = Mouse.current.position.ReadValue();
            
            transform.position = mousePose;
        }
    }
}
