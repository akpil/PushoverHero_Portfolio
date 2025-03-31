using System.Collections;
using UnityEngine;
using Utility;

namespace UI.Components
{
    class AlphaBreathe : MonoBehaviour
    {
        private IColorable _colorable;
        [SerializeField]
        private float _speed = 1;
        [SerializeField]
        private bool _playOnce = false;

        private void Awake()
        {
            _colorable = GetComponent<IColorable>();
        }

        private void OnEnable()
        {
            if (_playOnce)
            {
                StartCoroutine(PlayOnce());
            }
            else
            {
                StartCoroutine(PlayLoop());
            }
        }

        private IEnumerator PlayOnce()
        {
            float halfTime = _speed / 2;
            float currentTime = 0;
            while (currentTime < _speed)
            {
                Color color = _colorable.Color;
                color = new Color(color.r, color.g, color.b, Mathf.Abs(1 - currentTime / halfTime));
                _colorable.Color = color;
                yield return CoroutineManager.FixedUpdate;
                currentTime+= Time.fixedDeltaTime;
            }
        }

        private IEnumerator PlayLoop()
        {
            float halfTime = _speed / 2;
            float currentTime = 0;
            while (true)
            {
                Color color = _colorable.Color;
                color = new Color(color.r, color.g, color.b, Mathf.Abs(1 - currentTime / halfTime));
                _colorable.Color = color;
                yield return CoroutineManager.FixedUpdate;
                currentTime = (currentTime + Time.fixedDeltaTime) % _speed;
            }
        }
    }
}
