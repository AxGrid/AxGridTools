using System;
using UnityEngine;

namespace AxGrid.Utils
{
    public static class Polar
    {
        public static PolarVector ToPolar(Vector2 dec)
        {
            var r = Mathf.Sqrt(dec.x * dec.x + dec.y * dec.y);
            if (dec.x > 0 && dec.y >= 0)
            {
                return new PolarVector(r, Mathf.Atan(dec.y / dec.x));
            }
            if (dec.x > 0 && dec.y < 0)
            {
                return new PolarVector(r, Mathf.Atan(dec.y/dec.x) + 2*Mathf.PI);
            }
            if (dec.x < 0)
            {
                return new PolarVector(r, Mathf.Atan(dec.y/dec.x) + Mathf.PI);
            }
            if (dec.x == 0 && dec.y > 0)
            {
                return new PolarVector(r, Mathf.PI / 2);
            }
            if (dec.x == 0 && dec.y < 0)
            {
                return new PolarVector(r, 3 * Mathf.PI / 2);
            }
            return new PolarVector();
        }

        public static Vector2 FromPolar(PolarVector p) => new Vector2(p.r * Mathf.Cos(p.t), p.r * Mathf.Sin(p.t));
        public static Vector2 FromPolar(float r, float t) => new Vector2(r * Mathf.Cos(t), r * Mathf.Sin(t));
        
        
        public static PolarVector ToPolarVector(this Vector2 dec) => ToPolar(dec);
    }

    [Serializable]
    public class PolarVector
    {
        public float r = 0;
        public float t = 0;

        public Vector2 ToVector2 => Polar.FromPolar(this);
        
        public PolarVector() {}
        public PolarVector(float r, float t)
        {
            this.r = r;
            this.t = t;
        }
    }
}