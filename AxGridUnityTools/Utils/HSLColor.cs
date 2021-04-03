using System;
using UnityEngine;

namespace AxGrid.Utils
{
    public class HSLColor
{
    public float Hue;
    public float Saturation;
    public float Luminosity;
    public float Alfa;

    public HSLColor(float H, float S, float L, float A)
    {
        Hue = H;
        Saturation = S;
        Luminosity = L;
        Alfa = A;
    }

    public static HSLColor FromRGB(Color Clr)
    {
        return FromRGB(Clr.r, Clr.g, Clr.b, Clr.a);
    }

    
    public static HSLColor FromRGB(float _R, float _G, float _B, float _A)
    {
        
        var _Min = Math.Min(Math.Min(_R, _G), _B);
        var _Max = Math.Max(Math.Max(_R, _G), _B);
        var _Delta = _Max - _Min;

        float H = 0;
        float S = 0;
        var L = (_Max + _Min) / 2.0f;

        if (_Delta != 0)
        {
            if (L < 0.5f)
            {
                S = _Delta / (_Max + _Min);
            }
            else
            {
                S = _Delta / (2.0f - _Max - _Min);
            }

            var _Delta_R = ((_Max - _R) / 6.0f + (_Delta / 2.0f)) / _Delta;
            var _Delta_G = ((_Max - _G) / 6.0f + (_Delta / 2.0f)) / _Delta;
            var _Delta_B = ((_Max - _B) / 6.0f + (_Delta / 2.0f)) / _Delta;

            if (_R == _Max)
            {
                H = _Delta_B - _Delta_G;
            }
            else if (_G == _Max)
            {
                H = (1.0f / 3.0f) + _Delta_R - _Delta_B;
            }
            else if (_B == _Max)
            {
                H = (2.0f / 3.0f) + _Delta_G - _Delta_R;
            }

            if (H < 0) H += 1.0f;
            if (H > 1) H -= 1.0f;
        }

        return new HSLColor(H, S, L, _A);
    }

    private float Hue_2_RGB(float v1, float v2, float vH)
    {
        if (vH < 0) vH += 1;
        if (vH > 1) vH -= 1;
        if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
        if ((2 * vH) < 1) return (v2);
        if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2 / 3) - vH) * 6);
        return (v1);
    }

    public Color ToRGB()
    {
        var Clr = new Color();
        float var_1, var_2;

        if (Saturation == 0)
        {
            Clr.r = (byte)(Luminosity);
            Clr.g = (byte)(Luminosity);
            Clr.b = (byte)(Luminosity);
        }
        else
        {
            if (Luminosity < 0.5) var_2 = Luminosity * (1 + Saturation);
            else var_2 = (Luminosity + Saturation) - (Saturation * Luminosity);

            var_1 = 2 * Luminosity - var_2;

            Clr.r = (Hue_2_RGB(var_1, var_2, Hue + (1 / 3)));
            Clr.g = (Hue_2_RGB(var_1, var_2, Hue));
            Clr.b = (Hue_2_RGB(var_1, var_2, Hue - (1 / 3)));
        }

        Clr.a = Alfa;
        return Clr;
    }
}
}