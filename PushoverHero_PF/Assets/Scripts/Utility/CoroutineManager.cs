using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class CoroutineManager
    {
        public static readonly WaitForFixedUpdate FixedUpdate = new();
        public static readonly WaitForEndOfFrame EndOfFrame = new();

        private static readonly Dictionary<float, WaitForSeconds> _waitForSecDic = new();
        public static WaitForSeconds GetWaitForSeconds(float time)
        {
            if (_waitForSecDic.TryGetValue(time, out var sec)) return sec;
            
            sec = new WaitForSeconds(time);
            _waitForSecDic[time] = sec;
            return sec;
        }
        
        private static readonly Dictionary<float, WaitForSecondsRealtime> _waitForRealSecDic = new();
        public static WaitForSecondsRealtime GetWaitForRealSeconds(float time)
        {
            if (_waitForRealSecDic.TryGetValue(time, out var sec)) return sec;
            
            sec = new WaitForSecondsRealtime(time);
            _waitForRealSecDic[time] = sec;
            return sec;
        }
    }
}
