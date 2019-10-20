using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace AxGrid.Utils
{
    public static class StaticUtils
    {
        
        public static bool nearlyEqual(float a, float b, float epsilon = float.Epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }
        
        public static bool FEquals(this float a, float b, float epsilon = float.Epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }
        
        public static bool FEquals(this float a, int b, float epsilon = float.Epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        public static bool FEquals(this float a, long b, float epsilon = float.Epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }


        public static int[][] ReverseInsides(this int[][] data) {
            var res = new int[data.Length][];
            for (var i = 0; i < res.Length; i++) 
                res[i] = data[i].Reverse().ToArray();
            return res;
        }


        public static int[] RotateArray(this int[] array, int offset, int count=-1)
        {
            int[] res = new int[count > -1 ? count : array.Length];
            for (int i = 0; i < (count > -1 ? count : array.Length); i++)
                res[i] = array[(i + offset) % array.Length];
            return res;
        }


        public static void RotateList<T>(this List<T> list)
        {
            if (list.Count == 0)
                return;
            var item = list[0];
            list.RemoveAt(0);
            list.Add(item);
        }


        public static List<U> SingleList<U>(U item)
        {
            var list = new List<U> {item};
            return list;
        }
        
        
        public static Dictionary<T1, T2> UnionDictionaries<T1, T2>(Dictionary<T1, T2> D2, Dictionary<T1, T2> D1)
        {
            var rd = new Dictionary<T1, T2>(D1);
            foreach (var key in D2.Keys)
            {
                if (!rd.ContainsKey(key))
                    rd.Add(key, D2[key]);
                else if(rd[key].GetType().IsGenericType)
                {
                    if (rd[key].GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var mBase = MethodBase.GetCurrentMethod();
                        MethodInfo info = mBase is MethodInfo ? (MethodInfo)mBase : typeof(DictionaryHelper).GetMethod("UnionDictionaries", BindingFlags.Public | BindingFlags.Static);
                        var genericMethod = info.MakeGenericMethod(rd[key].GetType().GetGenericArguments()[0], rd[key].GetType().GetGenericArguments()[1]);
                        var invocationResult = genericMethod.Invoke(null, new object[] { rd[key], D2[key] });
                        rd[key] = (T2)invocationResult;
                    }
                }
            }
            return rd;
        }

        public static string StringsAggregation(string a, string b) => a + "," + b;
        
        public static string GetOSName()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WebGLPlayer:
                    return "WGL";
                case RuntimePlatform.Android:
                    return "AND";
                //case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return "WIN";
                //case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    return "LIN";
                //case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS";
                default:
                    return "OSX";
            }
        }

        public static int IndexOf(this int[] array, int item)
        {
            return Array.IndexOf(array, item);
        }
        
        public static U GetValueByKeyOrNull<T, U>(this Dictionary<T, U> dict, T key, U def = null)
            where U : class //it's acceptable for me to have this constraint
        {
            return dict.ContainsKey(key) ? dict[key] : def;
        }

        public static Color ToColor(this string hex)
        {
            var res = Color.white;
            return ColorUtility.TryParseHtmlString(hex,out res) ? res : Color.white;
        }


        /// <summary>
        /// Разделить лист на N листов
        /// </summary>
        /// <param name="source">массив</param>
        /// <param name="count">количество листов</param>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns></returns>
        public static List<List<T>> SplitToLists<T>(this IEnumerable<T> source, int count) {
            return source
                   .Select((x, i) => new {Index = i, Value = x})
                   .GroupBy(x => x.Index / count)
                   .Select(x => x.Select(v => v.Value).ToList())
                   .ToList();
        }


        public static Queue<T> EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> array) {
            foreach (var x1 in array) 
                queue.Enqueue(x1);
            return queue;
        }
        
        /// <summary>
        /// IEnumerable ForEach
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(T item in enumeration)
            {
                action(item);
            }
        }


        public static void SwapArrays<T>(ref T[] a, ref T[] b) {
            var tmp = new T[a.Length];
            Array.Copy(a, tmp, a.Length);
            a = b;
            b = tmp;
        }


        public static Color SetR(this Color color, float value) {
            return new Color(value, color.g, color.b, color.a);
        }
        
        public static Color SetG(this Color color, float value) {
            return new Color(color.r, value,  color.b, color.a);
        }

        public static Color SetB(this Color color, float value) {
            return new Color(color.r, color.g, value, color.a);
        }

        public static Color SetA(this Color color, float value) {
            return new Color(color.r, color.g, color.b, value);
        }

        
        public static Vector3 SetX(this Vector3 vector, float value) {
            return new Vector3(value, vector.y, vector.z);
        }

        public static Vector3 SetY(this Vector3 vector, float value) {
            return new Vector3(vector.x, value, vector.z);
        }

        public static Vector3 SetZ(this Vector3 vector, float value) {
            return new Vector3(vector.x, vector.y, value);
        }
        
    }
}