using System.Collections.Generic;
using UnityEngine;

namespace AxGrid.Tools
{
    [CreateAssetMenu(fileName = "AxSpriteCollection", menuName = "Ax/AxSprite Collection", order = 53)]

    public class AxSpriteObjectCollection : ScriptableObject
    {
        public List<AxSpriteObject> spriteObjects = new List<AxSpriteObject>();

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