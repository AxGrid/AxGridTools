using AxGrid.Base;
using UnityEngine;

namespace AxGrid.Tools
{
    public class AxSprite : MonoBehaviourExt
    {
        [SerializeField]
        private string collection = "";
        [SerializeField]
        private long spriteId = 0;
        
        [SerializeField]
        private string spriteName = "";

        public string Collection
        {
            get => collection;
            set
            {
                collection = value;
            }
        }
        
        public string SpriteName
        {
            get => spriteName;
            set
            {
                spriteName = value;
                spriteId = AxSpriteCollection.GetSpriteId(collection, spriteName);
                _sprite.sprite = AxSpriteCollection.GetSprite(collection, spriteName);
            }
        }
        
        public long SpriteId
        {
            get => spriteId;
            set
            {
                spriteId = value;
                _sprite.sprite = AxSpriteCollection.GetSprite(collection, spriteId);
            }
        }

        private SpriteRenderer _sprite;
        
        [OnAwake]
        public void __Init()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }
        
        [OnStart]
        public void __SetSprite()
        {
            _sprite.sprite = string.IsNullOrEmpty(spriteName) ? 
                AxSpriteCollection.GetSprite(collection, spriteId) : 
                AxSpriteCollection.GetSprite(collection, spriteName);
        }
        
        
    }
}