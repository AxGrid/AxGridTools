using System.Collections.Generic;
using UnityEngine;

namespace AxGrid.Tools
{
    [CreateAssetMenu(fileName = "New Sprite Collection", menuName = "AxSprite", order = 3)]

    public class AxSpriteObjectCollection : ScriptableObject
    {
        [SerializeField]
        private List<AxSpriteObject> spriteObjects = new List<AxSpriteObject>();

        public Sprite getSprite(long spriteId, Sprite def = null)
        {
            var res = spriteObjects.Find(item => item.SpriteId == spriteId).Sprite;
            return res == null ? def : res;
        }

        public Sprite getSprite(string name, Sprite def = null)
        {
            var res = spriteObjects.Find(item => item.SpriteName == name).Sprite;
            return res == null ? def : res;
        }


    }
}