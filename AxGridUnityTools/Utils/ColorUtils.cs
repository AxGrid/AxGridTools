using UnityEngine;

namespace AxGrid.Utils
{
    public static class ColorUtils
    {
        public static Color FromByte(int r, int g, int b, int a = 255)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }

        public static Color FromByte(byte r, byte g, byte b, byte a = 255)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }

        
        public static HSLColor ToHSL(this Color color)
        {
            return HSLColor.FromRGB(color);
        }
    }
}