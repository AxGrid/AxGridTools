using System;
using UnityEngine;

namespace AxGrid.Tools
{
    [CreateAssetMenu(fileName = "AxSprite", menuName = "Ax/AxSprite", order = 52)]
    public class AxSpriteObject : ScriptableObject
    {
        [SerializeField]
        private long spriteId = 0;
        [SerializeField]
        private string spriteName = "";       

        [SerializeField]
        private Sprite sprite;

        public virtual long SpriteId => spriteId;
        public virtual string SpriteName => spriteName;
        public Sprite Sprite => sprite;

    }
}