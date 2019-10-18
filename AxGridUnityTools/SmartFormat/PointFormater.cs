using System;
using System.Collections.Generic;
using System.Globalization;
using SmartFormat;
using System.Linq;
using AxGrid.Model;
using SmartFormat.Core.Extensions;
using AxGrid.Utils;

namespace AxGrid.SmartFormat
{
    /// <summary>
    /// Форматирует вывод long/int в настройку форматирования валюты
    /// {0:p(cur,100,true)}
    /// </summary>
    public class PointFormater : IFormatter
    {
        private string[] names = {"pt", "points"};
        public string[] Names { get { return names; } set { names = value; } }
        private static DynamicModel Model => Settings.Model;

        public readonly static Dictionary<int, string> ra = new Dictionary<int, string>
        {   { 10000, "ↂ"}, { 5000, "ↁ"},
            { 1000, "M" },  { 900, "CM" },  { 500, "D" },  { 400, "CD" },  { 100, "C" },
            { 90 , "XC" },  { 50 , "L" },  { 40 , "XL" },  { 10 , "X" },
            { 9  , "IX" },  { 5  , "V" },  { 4  , "IV" },  { 1  , "I" } };


        public static string ToRoman(int number)
        {
            return ra.Where(d => number >= d.Key)
                .Select(d => d.Value + ToRoman(number - d.Key))
                .FirstOrDefault();
        }

        private double tryParseDouble(string str)
        {
            return GetDouble(str, 1);
        }
        
        private bool tryParseBool(string str)
        {
            bool d;
            return bool.TryParse(str, out d) && d;
        }
        
        private int tryParseInt(string str)
        {
            int d;
            return int.TryParse(str, out d) ? d : 0;
        }
        
        public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
        {
            var iCanHandleThisInput = formattingInfo.CurrentValue is int || 
                                      formattingInfo.CurrentValue is long || 
                                      formattingInfo.CurrentValue is double ||
                                      formattingInfo.CurrentValue is float;
            if (!iCanHandleThisInput)
                return false;
            
            double val = Convert.ToInt64(formattingInfo.CurrentValue);

            var opts = formattingInfo.FormatterOptions != null
                ? formattingInfo.FormatterOptions.Split(',')
                : new string[0];
            
            var currencyOptions  = opts.Length > 0 && opts[0] != null ? opts[0] : null;
            var multiplyOptions  = opts.Length > 1 && opts[1] != null ? tryParseDouble(opts[1]) : 1;
            var positiveOptions  = opts.Length > 2 && opts[2] != null && tryParseBool(opts[2]);
            var numerologyStep = opts.Length > 3 && opts[3] != null ? tryParseInt(opts[3]) : 1000;
            var numerologyZeroPadding = opts.Length > 4 && opts[4] != null ? tryParseInt(opts[4]) : 2;
            val = (val * multiplyOptions);
            
            if (positiveOptions)
                val = Math.Abs(val);
            switch (currencyOptions)
            {
                default:
                    return false;
                case "":
                case null:
                case "null":
                case "val":
                    formattingInfo.Write(val.ToString());
                    return true;
                case "int":
                    formattingInfo.Write(Smart.Format("{0:0}", val));
                    return true;
                case "cur":
                    formattingInfo.Write(Smart.Format("{0:0.00}", val));
                    return true;
                case "rom":
                    formattingInfo.Write(ToRoman((int)val));
                    return true;
                case "num":
                    formattingInfo.Write(val.GetNumerology(numerologyStep, numerologyZeroPadding));
                    return true;
            }   
        }

        private static double GetDouble(string value, double defaultValue)
        {
            switch (value)
            {
                case "den":
                case "/den":
                    return 1.0 / Model?.GetInt("Denomination", 1) ?? 1;
                case "*den":
                    return Model?.GetInt("Denomination", 1) ?? 1;
            }

            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}