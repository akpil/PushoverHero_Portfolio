using System.Collections;
using UnityEngine;
using Utility;

namespace SubComponent
{
    public class OneShotEffect : Effect
    {
        [SerializeField]
        private float _activeTime;

        private void OnEnable()
        {
            StartCoroutine(TimeOut());
        }

        private IEnumerator TimeOut()
        {
            yield return CoroutineManager.GetWaitForRealSeconds(_activeTime);

            Return();
        }
    }
}
