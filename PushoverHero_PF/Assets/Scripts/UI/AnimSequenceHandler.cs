using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace UI
{
    public class AnimSequenceHandler : MonoBehaviour
    {
        [SerializeField]
        private float _initialWaitingTime;
        [SerializeField]
        private float _interval;
        [SerializeField]
        private List<Animator> _list = new List<Animator>();

        [Header("OnFinished")]
        [SerializeField]
        private float _intervalAfterSequenceFinished;
        [SerializeField]
        private UnityEvent _onAnimFinished;
        
        public void AssignElement(Animator element)
        {
            _list.Add(element);
        }

        public void RemoveElement(Animator element)
        {
            _list.Remove(element);
        }

        private void OnEnable()
        {
            _list = _list.Where(elem => elem != null).ToList();
            StartCoroutine(StartAnimation());
        }

        // Wait for real second를 쓰려고 했는데 그러면 가끔씩 5초 이상 대기 걸리는 이슈가 있어 그냥 WaitForSecond 씀
        private IEnumerator StartAnimation()
        {
            yield return CoroutineManager.GetWaitForSeconds(_initialWaitingTime);
            
            foreach (var elem in _list)
            {
                if (!elem.enabled)
                {
                    elem.enabled = true;
                    yield return CoroutineManager.GetWaitForSeconds(_interval);
                }
            }
            
            yield return CoroutineManager.GetWaitForSeconds(_intervalAfterSequenceFinished);
            _onAnimFinished.Invoke();
        }
    }
}
