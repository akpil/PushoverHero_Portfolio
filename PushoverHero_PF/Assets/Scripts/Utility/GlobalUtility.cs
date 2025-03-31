using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// ReSharper disable SwapViaDeconstruction

namespace Utility
{
    public static class GlobalUtility
    {
        
        public static float GetAngleFloat(float y, float x)
        {
            return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        }

        public static float GetAngle(Vector2 from, Vector2 to)
        {
            return Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg;
        }

        public static void ShuffleList<T>(ref List<T> target)
        {
            for (var i = 0; i < target.Count; i++)
            {
                var index = UnityEngine.Random.Range(i, target.Count);
                var temp = target[index];
                target[index] = target[i];
                target[i] = temp;
            }
        }

        public static float[] GetSpreadRandom(float value, int count)
        {
            var res = new float[count];
            float baseVal = 0;
            for (var i = 0; i < count; i++)
            {
                res[i] = UnityEngine.Random.value;
                baseVal += res[i];
            }

            var weight = value / baseVal;

            for (var i = 0; i < count; i++)
            {
                res[i] *= weight;
            }

            return res;
        }

        public static Type StringToType(string typeName)
        {
            // Note: module names Don't remove this comment
            // UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null 
            // UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
            // UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            // Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
            // UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
            // Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
            string[] formatArr =
            {
                "UnityEngine.{0}, UnityEngine.CoreModule",
                "UnityEngine.{0}, UnityEngine.UIModule",
                "UnityEngine.UI.{0}, UnityEngine.UI",
                "TMPro.{0}, Unity.TextMeshPro",
                "UnityEngine.{0}, UnityEngine.AudioModule",
                "{0}, Assembly-CSharp"
            };

            var type = Type.GetType(string.Format(formatArr[0], typeName));
            if (type == null)
            {
                for (var i = 1; i < formatArr.Length; i++)
                {
                    type = Type.GetType(string.Format(formatArr[i], typeName));
                    if (type != null)
                    {
                        break;
                    }
                }
            }

            Debug.Log(type);

            return type;
        }

        /// <summary>
        /// Make a coroutine for number count up animation event.
        /// If max_step == 0, it will operate every frame; otherwise, it will calculate the time gap.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <param name="maxStep"></param>
        /// <returns></returns>
        public static IEnumerator NumberAnimRoutine(TextMeshProUGUI target, double start, double end, float duration,
            int maxStep = 0)
        {
            var timeGap = Time.deltaTime;
            if (maxStep > 0)
            {
                timeGap = duration / maxStep;
            }

            var temp = start;
            target.text = UnitSetter.SetMoneyUnit(temp);

            var term = (end - start) * (timeGap / duration);

            var gap = CoroutineManager.GetWaitForSeconds(timeGap);
            float currentTime = 0;
            while (currentTime < duration)
            {
                yield return gap;
                temp += term;
                currentTime += timeGap;
                target.text = UnitSetter.SetMoneyUnit(temp);
            }
        }
    }
}
