using System;
using UnityEditor;

namespace Utility.Editor
{
    public static class EditorSupportUtility
    {
        public static void DrawNormalTypeArray<T>(ref T[] target) where T : new()
        {
            EditorGUI.indentLevel++;
            int length = EditorGUILayout.DelayedIntField("Size", target.Length);
            if (length < 0)
            {
                length = 0;
            }
            else if (target == null)
            {
                target = new T[length];
            }
            else if (target.Length != length)
            {
                var temp = new T[length];
                for (var i = 0; i < length && i < target.Length; i++)
                {
                    temp[i] = target[i];
                }

                target = temp;
            }

            var t = typeof(T);
            for (var i = 0; i < length; i++)
            {

                if (t.IsEnum)
                {
                    target[i] = (T)(object)EditorGUILayout.EnumPopup("Index " + i.ToString(),
                        (Enum)Enum.Parse(t, target[i].ToString()));
                }
                else if (t == typeof(int))
                {
                    target[i] = (T)(object)EditorGUILayout.IntField("Index " + i.ToString(),
                        Convert.ToInt32(target[i]));
                }
                else if (t == typeof(double))
                {
                    target[i] = (T)(object)EditorGUILayout.DoubleField("Index " + i.ToString(),
                        Convert.ToDouble(target[i]));
                }
                else if (t == typeof(float))
                {
                    target[i] = (T)(object)EditorGUILayout.FloatField("Index " + i.ToString(),
                        (float)Convert.ToDouble(target[i]));
                }
                else // type of string
                {
                    target[i] = (T)(object)EditorGUILayout.TextField("Index " + i.ToString(),
                        Convert.ToString(target[i]));
                }
            }

            EditorGUI.indentLevel--;
        }

        public static void DrawObjectTypeArray<T>(ref T[] target) where T : UnityEngine.Object
        {
            EditorGUI.indentLevel++;
            int length = EditorGUILayout.DelayedIntField("Size", target.Length);
            if (length < 0)
            {
                length = 0;
            }
            else if (target == null)
            {
                target = new T[length];
            }
            else if (target.Length != length)
            {
                var temp = new T[length];
                for (var i = 0; i < length && i < target.Length; i++)
                {
                    temp[i] = target[i];
                }

                target = temp;
            }

            var t = typeof(T);
            for (var i = 0; i < length; i++)
            {
                target[i] = EditorGUILayout.ObjectField("Index " + i.ToString(), target[i], t, true) as T;
            }

            EditorGUI.indentLevel--;
        }
    }
}
