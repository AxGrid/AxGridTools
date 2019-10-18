// Decompiled with JetBrains decompiler
// Type: Easing
// Assembly: UnityTools, Version=1.0.5674.17904, Culture=neutral, PublicKeyToken=null
// MVID: 16E1B122-2138-4FE7-A958-BAE06A9BFE75
// Assembly location: /Users/zed/src/unity/slots2017.4/Assets/lib/UnityTools.dll

using System;
using UnityEngine;

public class Easing
{
  public static double Linear(double t, double b, double c, double d)
  {
    return c * t / d + b;
  }

  public static double ExpoEaseOut(double t, double b, double c, double d)
  {
    return t != d ? c * (-Math.Pow(2.0, -10.0 * t / d) + 1.0) + b : b + c;
  }

  public static double ExpoEaseIn(double t, double b, double c, double d)
  {
    return t != 0.0 ? c * Math.Pow(2.0, 10.0 * (t / d - 1.0)) + b : b;
  }

  public static double ExpoEaseInOut(double t, double b, double c, double d)
  {
    if (t == 0.0)
      return b;
    if (t == d)
      return b + c;
    if ((t /= d / 2.0) < 1.0)
      return c / 2.0 * Math.Pow(2.0, 10.0 * (t - 1.0)) + b;
    return c / 2.0 * (-Math.Pow(2.0, -10.0 * --t) + 2.0) + b;
  }

  public static double ExpoEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.ExpoEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.ExpoEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double CircEaseOut(double t, double b, double c, double d)
  {
    return c * Math.Sqrt(1.0 - (t = t / d - 1.0) * t) + b;
  }

  public static double CircEaseIn(double t, double b, double c, double d)
  {
    return -c * (Math.Sqrt(1.0 - (t /= d) * t) - 1.0) + b;
  }

  public static double CircEaseInOut(double t, double b, double c, double d)
  {
    if ((t /= d / 2.0) < 1.0)
      return -c / 2.0 * (Math.Sqrt(1.0 - t * t) - 1.0) + b;
    return c / 2.0 * (Math.Sqrt(1.0 - (t -= 2.0) * t) + 1.0) + b;
  }

  public static double CircEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.CircEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.CircEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double QuadEaseOut(double t, double b, double c, double d)
  {
    return -c * (t /= d) * (t - 2.0) + b;
  }

  public static double QuadEaseIn(double t, double b, double c, double d)
  {
    return c * (t /= d) * t + b;
  }

  public static double QuadEaseInOut(double t, double b, double c, double d)
  {
    if ((t /= d / 2.0) < 1.0)
      return c / 2.0 * t * t + b;
    return -c / 2.0 * (--t * (t - 2.0) - 1.0) + b;
  }

  public static double QuadEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.QuadEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.QuadEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double SineEaseOut(double t, double b, double c, double d)
  {
    return c * Math.Sin(t / d * (Math.PI / 2.0)) + b;
  }

  public static double SineEaseIn(double t, double b, double c, double d)
  {
    return -c * Math.Cos(t / d * (Math.PI / 2.0)) + c + b;
  }

  public static double SineEaseInOut(double t, double b, double c, double d)
  {
    if ((t /= d / 2.0) < 1.0)
      return c / 2.0 * Math.Sin(Math.PI * t / 2.0) + b;
    return -c / 2.0 * (Math.Cos(Math.PI * --t / 2.0) - 2.0) + b;
  }

  public static double SineEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.SineEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.SineEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double CubicEaseOut(double t, double b, double c, double d)
  {
    return c * ((t = t / d - 1.0) * t * t + 1.0) + b;
  }

  public static double CubicEaseIn(double t, double b, double c, double d)
  {
    return c * (t /= d) * t * t + b;
  }

  public static double CubicEaseInOut(double t, double b, double c, double d)
  {
    if ((t /= d / 2.0) < 1.0)
      return c / 2.0 * t * t * t + b;
    return c / 2.0 * ((t -= 2.0) * t * t + 2.0) + b;
  }

  public static double CubicEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.CubicEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.CubicEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double QuartEaseOut(double t, double b, double c, double d)
  {
    return -c * ((t = t / d - 1.0) * t * t * t - 1.0) + b;
  }

  public static double QuartEaseIn(double t, double b, double c, double d)
  {
    return c * (t /= d) * t * t * t + b;
  }

  public static double QuartEaseInOut(double t, double b, double c, double d)
  {
    if ((t /= d / 2.0) < 1.0)
      return c / 2.0 * t * t * t * t + b;
    return -c / 2.0 * ((t -= 2.0) * t * t * t - 2.0) + b;
  }

  public static double QuartEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.QuartEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.QuartEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double QuintEaseOut(double t, double b, double c, double d)
  {
    return c * ((t = t / d - 1.0) * t * t * t * t + 1.0) + b;
  }

  public static double QuintEaseIn(double t, double b, double c, double d)
  {
    return c * (t /= d) * t * t * t * t + b;
  }

  public static double QuintEaseInOut(double t, double b, double c, double d)
  {
    if ((t /= d / 2.0) < 1.0)
      return c / 2.0 * t * t * t * t * t + b;
    return c / 2.0 * ((t -= 2.0) * t * t * t * t + 2.0) + b;
  }

  public static double QuintEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.QuintEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.QuintEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double ElasticEaseOut(double t, double b, double c, double d)
  {
    if ((t /= d) == 1.0)
      return b + c;
    double num1 = d * 0.3;
    double num2 = num1 / (2.0 * Math.PI) * Math.Asin(1.0);
    return c * Math.Pow(2.0, -10.0 * t) * Math.Sin((t * d - num2) * (2.0 * Math.PI) / num1) + c + b;
  }

  public static double ElasticEaseIn(double t, double b, double c, double d)
  {
    if ((t /= d) == 1.0)
      return b + c;
    double num1 = d * 0.3;
    double num2 = num1 / 4.0;
    return -(c * Math.Pow(2.0, 10.0 * --t) * Math.Sin((t * d - num2) * (2.0 * Math.PI) / num1)) + b;
  }

  public static double ElasticEaseInOut(double t, double b, double c, double d)
  {
    if ((t /= d / 2.0) == 2.0)
      return b + c;
    double num1 = d * (9.0 / 20.0);
    double num2 = num1 / 4.0;
    if (t < 1.0)
      return -0.5 * (c * Math.Pow(2.0, 10.0 * --t) * Math.Sin((t * d - num2) * (2.0 * Math.PI) / num1)) + b;
    return c * Math.Pow(2.0, -10.0 * --t) * Math.Sin((t * d - num2) * (2.0 * Math.PI) / num1) * 0.5 + c + b;
  }

  public static double ElasticEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.ElasticEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.ElasticEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double BounceEaseOut(double t, double b, double c, double d)
  {
    if ((t /= d) < 4.0 / 11.0)
      return c * (121.0 / 16.0 * t * t) + b;
    if (t < 8.0 / 11.0)
      return c * (121.0 / 16.0 * (t -= 6.0 / 11.0) * t + 0.75) + b;
    if (t < 10.0 / 11.0)
      return c * (121.0 / 16.0 * (t -= 9.0 / 11.0) * t + 15.0 / 16.0) + b;
    return c * (121.0 / 16.0 * (t -= 21.0 / 22.0) * t + 63.0 / 64.0) + b;
  }

  public static double BounceEaseIn(double t, double b, double c, double d)
  {
    return c - Easing.BounceEaseOut(d - t, 0.0, c, d) + b;
  }

  public static double BounceEaseInOut(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.BounceEaseIn(t * 2.0, 0.0, c, d) * 0.5 + b;
    return Easing.BounceEaseOut(t * 2.0 - d, 0.0, c, d) * 0.5 + c * 0.5 + b;
  }

  public static double BounceEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.BounceEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.BounceEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double BackEaseOut(double t, double b, double c, double d)
  {
    return c * ((t = t / d - 1.0) * t * (2.70158 * t + 1.70158) + 1.0) + b;
  }

  public static double BackEaseIn(double t, double b, double c, double d)
  {
    return c * (t /= d) * t * (2.70158 * t - 1.70158) + b;
  }

  public static double BackEaseInOut(double t, double b, double c, double d)
  {
    double num1 = 1.70158;
    if ((t /= d / 2.0) < 1.0)
    {
      double num2;
      return c / 2.0 * (t * t * (((num2 = num1 * 1.525) + 1.0) * t - num2)) + b;
    }
    double num3;
    return c / 2.0 * ((t -= 2.0) * t * (((num3 = num1 * 1.525) + 1.0) * t + num3) + 2.0) + b;
  }

  public static double BackEaseOutIn(double t, double b, double c, double d)
  {
    if (t < d / 2.0)
      return Easing.BackEaseOut(t * 2.0, b, c / 2.0, d);
    return Easing.BackEaseIn(t * 2.0 - d, b + c / 2.0, c / 2.0, d);
  }

  public static double FeedbackEaseOut(
    double t,
    double b,
    double c,
    double d,
    double tl,
    int fs)
  {
    if (t / d < 1.0)
      return (c - (double) fs) * t / d + b;
    t -= d;
    t /= tl / 1.2;
    return b + c - 2.0 * Math.Cos(t * 3.0) / (10.0 * t + 2.0) * (double) fs;
  }

  public static float Linear(float t, float b, float c, float d)
  {
    return c * t / d + b;
  }

  public static float ExpoEaseOut(float t, float b, float c, float d)
  {
    return (double) t != (double) d ? c * (float) (-(double) Mathf.Pow(2f, -10f * t / d) + 1.0) + b : b + c;
  }

  public static float ExpoEaseIn(float t, float b, float c, float d)
  {
    return (double) t != 0.0 ? c * Mathf.Pow(2f, (float) (10.0 * ((double) t / (double) d - 1.0))) + b : b;
  }

  public static float ExpoEaseInOut(float t, float b, float c, float d)
  {
    if ((double) t == 0.0)
      return b;
    if ((double) t == (double) d)
      return b + c;
    if ((double) (t /= d / 2f) < 1.0)
      return c / 2f * Mathf.Pow(2f, (float) (10.0 * ((double) t - 1.0))) + b;
    return (float) ((double) c / 2.0 * (-(double) Mathf.Pow(2f, -10f * --t) + 2.0)) + b;
  }

  public static float ExpoEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.ExpoEaseOut(t * 2f, b, c / 2f, d);
    return Easing.ExpoEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float CircEaseOut(float t, float b, float c, float d)
  {
    return c * Mathf.Sqrt((float) (1.0 - (double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t)) + b;
  }

  public static float CircEaseIn(float t, float b, float c, float d)
  {
    return (float) (-(double) c * ((double) Mathf.Sqrt((float) (1.0 - (double) (t /= d) * (double) t)) - 1.0)) + b;
  }

  public static float CircEaseInOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d / 2f) < 1.0)
      return (float) (-(double) c / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) t * (double) t)) - 1.0)) + b;
    return (float) ((double) c / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) (t -= 2f) * (double) t)) + 1.0)) + b;
  }

  public static float CircEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.CircEaseOut(t * 2f, b, c / 2f, d);
    return Easing.CircEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float QuadEaseOut(float t, float b, float c, float d)
  {
    return (float) (-(double) c * (double) (t /= d) * ((double) t - 2.0)) + b;
  }

  public static float QuadEaseIn(float t, float b, float c, float d)
  {
    return c * (t /= d) * t + b;
  }

  public static float QuadEaseInOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d / 2f) < 1.0)
      return c / 2f * t * t + b;
    return (float) (-(double) c / 2.0 * ((double) --t * ((double) t - 2.0) - 1.0)) + b;
  }

  public static float QuadEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.QuadEaseOut(t * 2f, b, c / 2f, d);
    return Easing.QuadEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float SineEaseOut(float t, float b, float c, float d)
  {
    return c * Mathf.Sin((float) ((double) t / (double) d * 1.57079637050629)) + b;
  }

  public static float SineEaseIn(float t, float b, float c, float d)
  {
    return -c * Mathf.Cos((float) ((double) t / (double) d * 1.57079637050629)) + c + b;
  }

  public static float SineEaseInOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d / 2f) < 1.0)
      return c / 2f * Mathf.Sin((float) (3.14159274101257 * (double) t / 2.0)) + b;
    return (float) (-(double) c / 2.0 * ((double) Mathf.Cos((float) (3.14159274101257 * (double) --t / 2.0)) - 2.0)) + b;
  }

  public static float SineEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.SineEaseOut(t * 2f, b, c / 2f, d);
    return Easing.SineEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float CubicEaseOut(float t, float b, float c, float d)
  {
    return c * (float) ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (double) t + 1.0) + b;
  }

  public static float CubicEaseIn(float t, float b, float c, float d)
  {
    return c * (t /= d) * t * t + b;
  }

  public static float CubicEaseInOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d / 2f) < 1.0)
      return c / 2f * t * t * t + b;
    return (float) ((double) c / 2.0 * ((double) (t -= 2f) * (double) t * (double) t + 2.0)) + b;
  }

  public static float CubicEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.CubicEaseOut(t * 2f, b, c / 2f, d);
    return Easing.CubicEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float QuartEaseOut(float t, float b, float c, float d)
  {
    return (float) (-(double) c * ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (double) t * (double) t - 1.0)) + b;
  }

  public static float QuartEaseIn(float t, float b, float c, float d)
  {
    return c * (t /= d) * t * t * t + b;
  }

  public static float QuartEaseInOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d / 2f) < 1.0)
      return c / 2f * t * t * t * t + b;
    return (float) (-(double) c / 2.0 * ((double) (t -= 2f) * (double) t * (double) t * (double) t - 2.0)) + b;
  }

  public static float QuartEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.QuartEaseOut(t * 2f, b, c / 2f, d);
    return Easing.QuartEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float QuintEaseOut(float t, float b, float c, float d)
  {
    return c * (float) ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (double) t * (double) t * (double) t + 1.0) + b;
  }

  public static float QuintEaseIn(float t, float b, float c, float d)
  {
    return c * (t /= d) * t * t * t * t + b;
  }

  public static float QuintEaseInOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d / 2f) < 1.0)
      return c / 2f * t * t * t * t * t + b;
    return (float) ((double) c / 2.0 * ((double) (t -= 2f) * (double) t * (double) t * (double) t * (double) t + 2.0)) + b;
  }

  public static float QuintEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.QuintEaseOut(t * 2f, b, c / 2f, d);
    return Easing.QuintEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float ElasticEaseOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d) == 1.0)
      return b + c;
    float num1 = d * 0.3f;
    float num2 = num1 / 6.283185f * Mathf.Asin(1f);
    return c * Mathf.Pow(2f, -10f * t) * Mathf.Sin((float) (((double) t * (double) d - (double) num2) * 6.28318548202515) / num1) + c + b;
  }

  public static float ElasticEaseIn(float t, float b, float c, float d)
  {
    if ((double) (t /= d) == 1.0)
      return b + c;
    float num1 = d * 0.3f;
    float num2 = num1 / 4f;
    return (float) -((double) c * (double) Mathf.Pow(2f, 10f * --t) * (double) Mathf.Sin((float) (((double) t * (double) d - (double) num2) * 6.28318548202515) / num1)) + b;
  }

  public static float ElasticEaseInOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d / 2f) == 2.0)
      return b + c;
    float num1 = d * 0.45f;
    float num2 = num1 / 4f;
    if ((double) t < 1.0)
      return (float) (-0.5 * ((double) c * (double) Mathf.Pow(2f, 10f * --t) * (double) Mathf.Sin((float) (((double) t * (double) d - (double) num2) * 6.28318548202515) / num1))) + b;
    return (float) ((double) c * (double) Mathf.Pow(2f, -10f * --t) * (double) Mathf.Sin((float) (((double) t * (double) d - (double) num2) * 6.28318548202515) / num1) * 0.5) + c + b;
  }

  public static float ElasticEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.ElasticEaseOut(t * 2f, b, c / 2f, d);
    return Easing.ElasticEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float BounceEaseOut(float t, float b, float c, float d)
  {
    if ((double) (t /= d) < 0.363636374473572)
      return c * (121f / 16f * t * t) + b;
    if ((double) t < 0.727272748947144)
      return c * (float) (121.0 / 16.0 * (double) (t -= 0.5454546f) * (double) t + 0.75) + b;
    if ((double) t < 0.909090936183929)
      return c * (float) (121.0 / 16.0 * (double) (t -= 0.8181818f) * (double) t + 15.0 / 16.0) + b;
    return c * (float) (121.0 / 16.0 * (double) (t -= 0.9545454f) * (double) t + 63.0 / 64.0) + b;
  }

  public static float BounceEaseIn(float t, float b, float c, float d)
  {
    return c - Easing.BounceEaseOut(d - t, 0.0f, c, d) + b;
  }

  public static float BounceEaseInOut(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.BounceEaseIn(t * 2f, 0.0f, c, d) * 0.5f + b;
    return (float) ((double) Easing.BounceEaseOut(t * 2f - d, 0.0f, c, d) * 0.5 + (double) c * 0.5) + b;
  }

  public static float BounceEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.BounceEaseOut(t * 2f, b, c / 2f, d);
    return Easing.BounceEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float BackEaseOut(float t, float b, float c, float d)
  {
    return c * (float) ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (2.70158004760742 * (double) t + 1.70158004760742) + 1.0) + b;
  }

  public static float BackEaseIn(float t, float b, float c, float d)
  {
    return (float) ((double) c * (double) (t /= d) * (double) t * (2.70158004760742 * (double) t - 1.70158004760742)) + b;
  }

  public static float BackEaseInOut(float t, float b, float c, float d)
  {
    float num1 = 1.70158f;
    if ((double) (t /= d / 2f) < 1.0)
    {
      float num2;
      return (float) ((double) c / 2.0 * ((double) t * (double) t * (((double) (num2 = num1 * 1.525f) + 1.0) * (double) t - (double) num2))) + b;
    }
    float num3;
    return (float) ((double) c / 2.0 * ((double) (t -= 2f) * (double) t * (((double) (num3 = num1 * 1.525f) + 1.0) * (double) t + (double) num3) + 2.0)) + b;
  }

  public static float BackEaseOutIn(float t, float b, float c, float d)
  {
    if ((double) t < (double) d / 2.0)
      return Easing.BackEaseOut(t * 2f, b, c / 2f, d);
    return Easing.BackEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
  }

  public static float FeedbackEaseOut(float t, float b, float c, float d, float tl, int fs)
  {
    if ((double) t / (double) d < 1.0)
      return (c - (float) fs) * t / d + b;
    t -= d;
    t /= tl / 1.2f;
    return (float) ((double) b + (double) c - 2.0 * (double) Mathf.Cos(t * 3f) / (10.0 * (double) t + 2.0) * (double) fs);
  }
}
