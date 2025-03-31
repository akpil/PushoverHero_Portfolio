using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI.Components
{
    public class GifAnimator : MonoBehaviour {
        [SerializeField]
        private Image _frame;
        [SerializeField]
        private RectTransform _rect;
        public RectTransform RectTransform => _rect;

        [SerializeField]
        private Sprite[] _sprites;
        
        [SerializeField]
        private float _fPS;

        [SerializeField]
        private bool _isLoop;
        [SerializeField]
        private UnityEvent _onAnimationFinish;

        private float _timeGap;
        private int _spriteIndex;
        private Coroutine _currentRoutine;

        private void OnValidate()
        {
            _frame = GetComponent<Image>();
            _rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _timeGap = 1 / _fPS;
            
            _spriteIndex = 0;
            _frame.sprite = _sprites[_spriteIndex];
            _currentRoutine = StartCoroutine(_isLoop ? PlayLoop() : PlayOnce());
        }

        private void OnDisable()
        {
            _spriteIndex = 0;
            _frame.sprite = _sprites[_spriteIndex];
        }

        public void AddOnAnimationFinishListener(UnityAction callback)
        {
            _onAnimationFinish.AddListener(callback);
        }

        public void RewindAnimation()
        {
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
            }
            _currentRoutine = StartCoroutine(Rewind());
        }

        public void SkipAnimation()
        {
            if (_currentRoutine == null) return;
            
            StopCoroutine(_currentRoutine);
            _currentRoutine = null;
            _frame.sprite = _sprites[^1];
        }

        private IEnumerator Rewind()
        {
            _spriteIndex = _sprites.Length - 1;
            while (_spriteIndex >= 0)
            {
                _frame.sprite = _sprites[_spriteIndex];
                yield return CoroutineManager.GetWaitForSeconds(_timeGap);
                _spriteIndex--;
            }
            _currentRoutine = null;
        }

        private IEnumerator PlayOnce()
        {
            while (_spriteIndex < _sprites.Length)
            {
                _frame.sprite = _sprites[_spriteIndex];
                yield return CoroutineManager.GetWaitForSeconds(_timeGap);
                _spriteIndex++;
            }
            _onAnimationFinish.Invoke();
            _currentRoutine = null;
        }

        private IEnumerator PlayLoop()
        {
            while (true)
            {
                _frame.sprite = _sprites[_spriteIndex];
                yield return CoroutineManager.GetWaitForSeconds(_timeGap);
                _spriteIndex++;
                
                if (_spriteIndex < _sprites.Length) continue;
                
                _spriteIndex = 0;
                _onAnimationFinish.Invoke();
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
