using UnityEngine;

namespace AxGrid.Tools
{
    
    public interface OKeySprite<out K>
    {
        K OKey { get; }
        Sprite OValue { get; }
    }
}