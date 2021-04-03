using System;
using UnityEngine;

namespace AxGrid.Tools
{
    [CreateAssetMenu(fileName = "New Sprite Object", menuName = "AxSprite", order = 2)]
    public class AxSpriteObject : ScriptableObject
    {
        [SerializeField]
        private long spriteId = 0;
        [SerializeField]
        private string spriteName = "";       

        [SerializeField]
        private Sprite sprite;

        public long SpriteId => spriteId;
        public string SpriteName => spriteName;
        public Sprite Sprite => sprite;

    }
}