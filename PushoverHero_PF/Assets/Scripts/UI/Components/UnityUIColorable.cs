using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class UnityUIColorable : MonoBehaviour, IColorable
    {
        public Color Color
        {
            get => _graphic.color;
            set => _graphic.color = value;
        }

        [SerializeField, HideInInspector]
        private Graphic _graphic;

        private void OnValidate()
        {
            if (_graphic == null)
            {
                _graphic = GetComponent<Graphic>();
            }
        }
    }
}
