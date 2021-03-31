using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AxGrid.Utils;
using UnityEngine;
using YamlDotNet.Serialization;

namespace AxGrid.Text {
    public class TextRepository : ITextRepository {
        
        public Dictionary<string, string> Translations { get; set; }

        public string Get(string key, string def = null)
        {
            return Translations.ContainsKey(key) ? Translations[key] : def;
        }
        
        private TextRepository()
        {
            Translations = new Dictionary<string, string>();
        }
        
        /**
         * Fallback languages codes
         */
        public TextRepository(IEnumerable<string> languageYamlSources)
        {
            var d = new Dictionary<string, string>();
            foreach (var text in languageYamlSources.Reverse())
            {
                var deserializer = new Deserializer();
                var obj = (Dictionary<object, object>) deserializer.Deserialize(
                    new StringReader(text),
                    typeof(Dictionary<object, object>)
                );
                d = DictionaryHelper.UnionDictionaries(d, obj.FlattenKeys());
            }

            Translations = d;
        }

        public static TextRepository FromResources(IEnumerable<string> languageCodes)
        {
            var texts = languageCodes.Select(languageCode => $"Translations/{languageCode}_out.yml")
                .Select(name =>
                {
                     Log.Debug($"Load language resource from {name} ...");
                     return name;
                })
                    .Select(file =>
                {
                    try
                    {
                        return Resources.Load(file) as TextAsset;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                })
                .Where(item => item != null)
                .Select(t => t.text)
                .ToList();
            return new TextRepository(texts);
        }
        
        public bool HasKey(string key)
        {
            return Translations.ContainsKey(key);
        }
        
    }
}