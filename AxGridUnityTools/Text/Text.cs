using System;
using System.Collections.Generic;
using System.IO;
using AxGrid.Utils;
using SmartFormat;
using SmartFormat.Core.Settings;
using UnityEngine;
using YamlDotNet.Serialization;


namespace AxGrid.Text
{
    /// <summary>
    /// Тектовая утилита
    /// </summary>
    public static class Text
    {

        private static SmartFormatter sf;

        public static SmartFormatter Sf
        {
            get
            {
                if (sf == null) Init(new []{"ru"});
                return sf;
            }
        }

        public static ITextRepository Repository { get; set; }

        public static void Init(IEnumerable<string> languageCodes)
        {
            Repository = new TextRepository(languageCodes);
            sf = Smart.CreateDefaultSmartFormat();
            sf.Settings.FormatErrorAction = ErrorAction.ThrowError;
        }

        public static string GetFormat(string format)
        {
            if (!format.StartsWith("app."))
                return format;
            try
            {
                return Sf.Format("{" + format + "}", Repository.Translations);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static string Get(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format)) return format;
            return args.Length == 0 ? _Get(format) : Smart.Format(_Get(format), args);
        }

        private static string _Get(string var)
        {
            if (!var.StartsWith("app."))
                return var;
            try
            {
                return Sf.Format("{" + var + "}", Repository.Translations);
            }
            catch (Exception)
            {
                return var;
            }
        }

    }
}