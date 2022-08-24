using System.Linq;

namespace AxGrid.Utils
{
    public static class ConvertUtils
    {
        
        
        
        public static int[] ConvertToInt(object o)
        {
            if (o == null) return default;
            if (o is int[] intArr)
                return intArr;
            if (o is uint[] uintArr)
                return uintArr.Select(i => (int) i).ToArray();
            if (o is long[] longArr)
                return longArr.Select(i => (int) i).ToArray();
            if (o is ulong[] ulongArr)
                return ulongArr.Select(i => (int) i).ToArray();
            if (o is double[] doubleArr)
                return doubleArr.Select(i => (int) i).ToArray();
            if (o is float[] floatArr)
                return floatArr.Select(i => (int) i).ToArray();
            if (o is bool[] boolArr)
                return boolArr.Select(i => i ? 1 : 0).ToArray();
            if (o is string[] strArr)
                return strArr.Select(int.Parse).ToArray();
            return default;
        }
        
        public static uint[] ConvertToUInt(object o)
        {
            if (o == null) return default;
            if (o is int[] intArr)
                return intArr.Select(i => (uint) i).ToArray();
            if (o is uint[] uintArr)
                return uintArr;
            if (o is long[] longArr)
                return longArr.Select(i => (uint) i).ToArray();
            if (o is ulong[] ulongArr)
                return ulongArr.Select(i => (uint) i).ToArray();
            if (o is double[] doubleArr)
                return doubleArr.Select(i => (uint) i).ToArray();
            if (o is float[] floatArr)
                return floatArr.Select(i => (uint) i).ToArray();
            if (o is bool[] boolArr)
                return boolArr.Select(i => i ? (uint)1 : 0).ToArray();
            if (o is string[] strArr)
                return strArr.Select(uint.Parse).ToArray();
            return default;
        }
        
        public static long[] ConvertToLong(object o)
        {
            if (o == null) return default;
            if (o is int[] intArr)
                return intArr.Select(i => (long) i).ToArray();
            if (o is uint[] uintArr)
                return uintArr.Select(i => (long) i).ToArray();
            if (o is long[] longArr)
                return longArr;
            if (o is ulong[] ulongArr)
                return ulongArr.Select(i => (long) i).ToArray();
            if (o is double[] doubleArr)
                return doubleArr.Select(i => (long) i).ToArray();
            if (o is float[] floatArr)
                return floatArr.Select(i => (long) i).ToArray();
            if (o is bool[] boolArr)
                return boolArr.Select(i => i ? (long)1 : 0).ToArray();
            if (o is string[] strArr)
                return strArr.Select(long.Parse).ToArray();
            return default;
        }
        
        public static ulong[] ConvertToULong(object o)
        {
            if (o == null) return default;
            if (o is int[] intArr)
                return intArr.Select(i => (ulong) i).ToArray();
            if (o is uint[] uintArr)
                return uintArr.Select(i => (ulong) i).ToArray();
            if (o is long[] longArr)
                return longArr.Select(i => (ulong) i).ToArray();
            if (o is ulong[] ulongArr)
                return ulongArr;
            if (o is double[] doubleArr)
                return doubleArr.Select(i => (ulong) i).ToArray();
            if (o is float[] floatArr)
                return floatArr.Select(i => (ulong) i).ToArray();
            if (o is bool[] boolArr)
                return boolArr.Select(i => i ? (ulong)1 : 0).ToArray();
            if (o is string[] strArr)
                return strArr.Select(ulong.Parse).ToArray();
            return default;
        }
        
         
        public static double[] ConvertToDouble(object o)
        {
            if (o == null) return default;
            if (o is int[] intArr)
                return intArr.Select(i => (double) i).ToArray();
            if (o is uint[] uintArr)
                return uintArr.Select(i => (double) i).ToArray();
            if (o is long[] longArr)
                return longArr.Select(i => (double) i).ToArray();
            if (o is ulong[] ulongArr)
                return ulongArr.Select(i => (double) i).ToArray();
            if (o is double[] doubleArr)
                return doubleArr;
            if (o is float[] floatArr)
                return floatArr.Select(i => (double) i).ToArray();
            if (o is bool[] boolArr)
                return boolArr.Select(i => i ? (double)1 : 0).ToArray();
            if (o is string[] strArr)
                return strArr.Select(double.Parse).ToArray();
            return default;
        }
        
        public static float[] ConvertToFloat(object o)
        {
            if (o == null) return default;
            if (o is int[] intArr)
                return intArr.Select(i => (float) i).ToArray();
            if (o is uint[] uintArr)
                return uintArr.Select(i => (float) i).ToArray();
            if (o is long[] longArr)
                return longArr.Select(i => (float) i).ToArray();
            if (o is ulong[] ulongArr)
                return ulongArr.Select(i => (float) i).ToArray();
            if (o is double[] doubleArr)
                return doubleArr.Select(i => (float) i).ToArray();
            if (o is float[] floatArr)
                return floatArr;
            if (o is bool[] boolArr)
                return boolArr.Select(i => i ? (float)1 : 0).ToArray();
            if (o is string[] strArr)
                return strArr.Select(float.Parse).ToArray();
            return default;
        }
        
        public static string[] ConvertToString(object o)
        {
            if (o == null) return default;
            if (o is string[] strArr)
                return strArr;
            if (o is int[] intArr)
                return intArr.Select(i => i.ToString()).ToArray();
            if (o is uint[] uintArr)
                return uintArr.Select(i => i.ToString()).ToArray();
            if (o is long[] longArr)
                return longArr.Select(i => i.ToString()).ToArray();
            if (o is ulong[] ulongArr)
                return ulongArr.Select(i => i.ToString()).ToArray();
            if (o is double[] doubleArr)
                return doubleArr.Select(i => i.ToString()).ToArray();
            if (o is float[] floatArr)
                return floatArr.Select(i => i.ToString()).ToArray();
            if (o is bool[] boolArr)
                return boolArr.Select(i => i.ToString()).ToArray();
            return default;
        }
        
         
        public static bool[] ConvertToBool(object o)
        {
            if (o == null) return default;
            if (o is bool[] boolArr)
                return boolArr;
            if (o is int[] intArr)
                return intArr.Select(i => i > 0).ToArray();
            if (o is uint[] uintArr)
                return uintArr.Select(i => i > 0).ToArray();
            if (o is long[] longArr)
                return longArr.Select(i => i > 0).ToArray();
            if (o is ulong[] ulongArr)
                return ulongArr.Select(i => i > 0).ToArray();
            if (o is double[] doubleArr)
                return doubleArr.Select(i => i > 0).ToArray();
            if (o is float[] floatArr)
                return floatArr.Select(i => i > 0).ToArray();
            if (o is string[] strArr)
                return strArr.Select(i => i == "true").ToArray();
            return default;
        }
    }
}