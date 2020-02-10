using System.Collections.Generic;
using System.IO;
using AxGrid.Utils;
using UnityEngine;
using YamlDotNet.Serialization;

namespace AxGrid.Text {
    public class TextRepository : ITextRepository {
        
        public Dictionary<string, object> Translations { get; set; }
        
        public TextRepository(IEnumerable<string> languageCodes)
        {
            Translations = new Dictionary<string, object>();
            foreach (var languageCode in languageCodes)
            {
                var file = $"Translations/{languageCode}_out.yml";
                var t = Resources.Load(file) as TextAsset;
                Log.Debug($"Load translations file {file}");
                var deserializer = new Deserializer();
                
                var obj = (Dictionary<string, object>) deserializer.Deserialize(
                    new StringReader(t.text),
                    typeof(Dictionary<string, object>)
                );
                Translations = StaticUtils.UnionDictionaries(Translations, obj);
            }
        }
        
        public bool HasKey(string key)
        {
            return Translations.ContainsKey(key);
        }
        
    }
}