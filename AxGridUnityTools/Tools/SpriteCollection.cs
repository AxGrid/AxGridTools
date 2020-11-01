using System;
using System.Collections.Generic;
using AxGrid.Base;
using UnityEngine;

namespace AxGrid.Tools
{
    /**
     * Коллекция спрайта
     */
    public class SpriteCollection : MonoBehaviourExt
    {
        public string collectionName = "";
        [Serializable]
        public class SpriteData
        {
            public string name = "";
            public long index = -1;
            public Sprite sprite;
        }

        public List<SpriteData> sprites = new List<SpriteData>();
        protected static long lastIndex = 0;
        public static Dictionary<string, Dictionary<long, Sprite>> spriteIndexes = new Dictionary<string, Dictionary<long, Sprite>>();
        public static Dictionary<string, Dictionary<string, Sprite>> spriteNames = new Dictionary<string, Dictionary<string, Sprite>>();
        public static HashSet<string> loadedCollections = new HashSet<string>();

        [OnAwake]
        public void Init()
        {
            
            if (string.IsNullOrEmpty(collectionName)) collectionName = this.name;
            if (loadedCollections.Contains(collectionName)) return;
            loadedCollections.Add(collectionName);
            if (!spriteIndexes.ContainsKey(collectionName)) spriteIndexes.Add(collectionName, new Dictionary<long, Sprite>());
            if (!spriteNames.ContainsKey(collectionName)) spriteNames.Add(collectionName, new Dictionary<string, Sprite>());
            foreach (var data in sprites)
            {
                if (data.index == -1) data.index = lastIndex++;
                if (spriteIndexes[collectionName].ContainsKey(data.index))
                    spriteIndexes[collectionName][data.index] = data.sprite;
                else
                    spriteIndexes[collectionName].Add(data.index, data.sprite);

                if (!string.IsNullOrEmpty(data.name))
                    data.name = data.sprite.name;
                
                if (spriteNames[collectionName].ContainsKey(data.name))
                    spriteNames[collectionName][data.name] = data.sprite;
                else
                    spriteNames[collectionName].Add(data.name, data.sprite);
            }
        }

        public static Sprite getSprite(string collection, long index)
        {
            if (!spriteIndexes.ContainsKey(collection)) return null;
            if (!spriteIndexes[collection].ContainsKey(index)) return null;
            return spriteIndexes[collection][index];
        }

        public static Sprite getSprite(string collection, string name)
        {
            if (!spriteNames.ContainsKey(collection)) return null;
            if (!spriteNames[collection].ContainsKey(name)) return null;
            return spriteNames[collection][name];
        }
        
        

    }
    
}