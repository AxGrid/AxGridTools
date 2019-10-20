// Decompiled with JetBrains decompiler
// Type: EasingTo
// Assembly: UnityTools, Version=1.0.5674.17904, Culture=neutral, PublicKeyToken=null
// MVID: 16E1B122-2138-4FE7-A958-BAE06A9BFE75
// Assembly location: /Users/zed/src/unity/slots2017.4/Assets/lib/UnityTools.dll

namespace AxGrid.Utils {

  public class EasingTo {
    public static double Linear(double t, double b, double c, double d) {
      return Easing.Linear(t, b, c - b, d);
    }

    public static double ExpoEaseOut(double t, double b, double c, double d) {
      return Easing.ExpoEaseOut(t, b, c - b, d);
    }

    public static double ExpoEaseIn(double t, double b, double c, double d) {
      return Easing.ExpoEaseIn(t, b, c - b, d);
    }

    public static double ExpoEaseInOut(double t, double b, double c, double d) {
      return Easing.ExpoEaseInOut(t, b, c - b, d);
    }

    public static double ExpoEaseOutIn(double t, double b, double c, double d) {
      return Easing.ExpoEaseOutIn(t, b, c - b, d);
    }

    public static double CircEaseOut(double t, double b, double c, double d) {
      return Easing.CircEaseOut(t, b, c - b, d);
    }

    public static double CircEaseIn(double t, double b, double c, double d) {
      return Easing.CircEaseIn(t, b, c - b, d);
    }

    public static double CircEaseInOut(double t, double b, double c, double d) {
      return Easing.CircEaseInOut(t, b, c - b, d);
    }

    public static double CircEaseOutIn(double t, double b, double c, double d) {
      return Easing.CircEaseOutIn(t, b, c - b, d);
    }

    public static double QuadEaseOut(double t, double b, double c, double d) {
      return Easing.QuadEaseOut(t, b, c - b, d);
    }

    public static double QuadEaseIn(double t, double b, double c, double d) {
      return Easing.QuadEaseIn(t, b, c - b, d);
    }

    public static double QuadEaseInOut(double t, double b, double c, double d) {
      return Easing.QuadEaseInOut(t, b, c - b, d);
    }

    public static double QuadEaseOutIn(double t, double b, double c, double d) {
      return Easing.QuadEaseOutIn(t, b, c - b, d);
    }

    public static double SineEaseOut(double t, double b, double c, double d) {
      return Easing.SineEaseOut(t, b, c - b, d);
    }

    public static double SineEaseIn(double t, double b, double c, double d) {
      return Easing.SineEaseIn(t, b, c - b, d);
    }

    public static double SineEaseInOut(double t, double b, double c, double d) {
      return Easing.SineEaseInOut(t, b, c - b, d);
    }

    public static double SineEaseOutIn(double t, double b, double c, double d) {
      return Easing.SineEaseOutIn(t, b, c - b, d);
    }

    public static double CubicEaseOut(double t, double b, double c, double d) {
      return Easing.CubicEaseOut(t, b, c - b, d);
    }

    public static double CubicEaseIn(double t, double b, double c, double d) {
      return Easing.CubicEaseIn(t, b, c - b, d);
    }

    public static double CubicEaseInOut(double t, double b, double c, double d) {
      return Easing.CubicEaseInOut(t, b, c - b, d);
    }

    public static double CubicEaseOutIn(double t, double b, double c, double d) {
      return Easing.CubicEaseOutIn(t, b, c - b, d);
    }

    public static double QuartEaseOut(double t, double b, double c, double d) {
      return Easing.QuartEaseOut(t, b, c - b, d);
    }

    public static double QuartEaseIn(double t, double b, double c, double d) {
      return Easing.QuartEaseIn(t, b, c - b, d);
    }

    public static double QuartEaseInOut(double t, double b, double c, double d) {
      return Easing.QuartEaseInOut(t, b, c - b, d);
    }

    public static double QuartEaseOutIn(double t, double b, double c, double d) {
      return Easing.QuartEaseOutIn(t, b, c - b, d);
    }

    public static double QuintEaseOut(double t, double b, double c, double d) {
      return Easing.QuintEaseOut(t, b, c - b, d);
    }

    public static double QuintEaseIn(double t, double b, double c, double d) {
      return Easing.QuintEaseIn(t, b, c - b, d);
    }

    public static double QuintEaseInOut(double t, double b, double c, double d) {
      return Easing.QuintEaseInOut(t, b, c - b, d);
    }

    public static double QuintEaseOutIn(double t, double b, double c, double d) {
      return Easing.QuintEaseOutIn(t, b, c - b, d);
    }

    public static double ElasticEaseOut(double t, double b, double c, double d) {
      return Easing.ElasticEaseOut(t, b, c - b, d);
    }

    public static double ElasticEaseIn(double t, double b, double c, double d) {
      return Easing.ElasticEaseIn(t, b, c - b, d);
    }

    public static double ElasticEaseInOut(double t, double b, double c, double d) {
      return Easing.ElasticEaseInOut(t, b, c - b, d);
    }

    public static double ElasticEaseOutIn(double t, double b, double c, double d) {
      return Easing.ElasticEaseOutIn(t, b, c - b, d);
    }

    public static double BounceEaseOut(double t, double b, double c, double d) {
      return Easing.BounceEaseOut(t, b, c - b, d);
    }

    public static double BounceEaseIn(double t, double b, double c, double d) {
      return Easing.BounceEaseIn(t, b, c - b, d);
    }

    public static double BounceEaseInOut(double t, double b, double c, double d) {
      return Easing.BounceEaseInOut(t, b, c - b, d);
    }

    public static double BounceEaseOutIn(double t, double b, double c, double d) {
      return Easing.BounceEaseOutIn(t, b, c - b, d);
    }

    public static double BackEaseOut(double t, double b, double c, double d) {
      return Easing.BackEaseOut(t, b, c - b, d);
    }

    public static double BackEaseIn(double t, double b, double c, double d) {
      return Easing.BackEaseIn(t, b, c - b, d);
    }

    public static double BackEaseInOut(double t, double b, double c, double d) {
      return Easing.BackEaseInOut(t, b, c - b, d);
    }

    public static double BackEaseOutIn(double t, double b, double c, double d) {
      return Easing.BackEaseOutIn(t, b, c - b, d);
    }

    public static double FeedbackEaseOut(
      double t,
      double b,
      double c,
      double d,
      double tl,
      int fs) {
      return Easing.FeedbackEaseOut(t, b, c - b, d, tl, fs);
    }

    public static float Linear(float t, float b, float c, float d) {
      return Easing.Linear(t, b, c - b, d);
    }

    public static float ExpoEaseOut(float t, float b, float c, float d) {
      return Easing.ExpoEaseOut(t, b, c - b, d);
    }

    public static float ExpoEaseIn(float t, float b, float c, float d) {
      return Easing.ExpoEaseIn(t, b, c - b, d);
    }

    public static float ExpoEaseInOut(float t, float b, float c, float d) {
      return Easing.ExpoEaseInOut(t, b, c - b, d);
    }

    public static float ExpoEaseOutIn(float t, float b, float c, float d) {
      return Easing.ExpoEaseOutIn(t, b, c - b, d);
    }

    public static float CircEaseOut(float t, float b, float c, float d) {
      return Easing.CircEaseOut(t, b, c - b, d);
    }

    public static float CircEaseIn(float t, float b, float c, float d) {
      return Easing.CircEaseIn(t, b, c - b, d);
    }

    public static float CircEaseInOut(float t, float b, float c, float d) {
      return Easing.CircEaseInOut(t, b, c - b, d);
    }

    public static float CircEaseOutIn(float t, float b, float c, float d) {
      return Easing.CircEaseOutIn(t, b, c - b, d);
    }

    public static float QuadEaseOut(float t, float b, float c, float d) {
      return Easing.QuadEaseOut(t, b, c - b, d);
    }

    public static float QuadEaseIn(float t, float b, float c, float d) {
      return Easing.QuadEaseIn(t, b, c - b, d);
    }

    public static float QuadEaseInOut(float t, float b, float c, float d) {
      return Easing.QuadEaseInOut(t, b, c - b, d);
    }

    public static float QuadEaseOutIn(float t, float b, float c, float d) {
      return Easing.QuadEaseOutIn(t, b, c - b, d);
    }

    public static float SineEaseOut(float t, float b, float c, float d) {
      return Easing.SineEaseOut(t, b, c - b, d);
    }

    public static float SineEaseIn(float t, float b, float c, float d) {
      return Easing.SineEaseIn(t, b, c - b, d);
    }

    public static float SineEaseInOut(float t, float b, float c, float d) {
      return Easing.SineEaseInOut(t, b, c - b, d);
    }

    public static float SineEaseOutIn(float t, float b, float c, float d) {
      return Easing.SineEaseOutIn(t, b, c - b, d);
    }

    public static float CubicEaseOut(float t, float b, float c, float d) {
      return Easing.CubicEaseOut(t, b, c - b, d);
    }

    public static float CubicEaseIn(float t, float b, float c, float d) {
      return Easing.CubicEaseIn(t, b, c - b, d);
    }

    public static float CubicEaseInOut(float t, float b, float c, float d) {
      return Easing.CubicEaseInOut(t, b, c - b, d);
    }

    public static float CubicEaseOutIn(float t, float b, float c, float d) {
      return Easing.CubicEaseOutIn(t, b, c - b, d);
    }

    public static float QuartEaseOut(float t, float b, float c, float d) {
      return Easing.QuartEaseOut(t, b, c - b, d);
    }

    public static float QuartEaseIn(float t, float b, float c, float d) {
      return Easing.QuartEaseIn(t, b, c - b, d);
    }

    public static float QuartEaseInOut(float t, float b, float c, float d) {
      return Easing.QuartEaseInOut(t, b, c - b, d);
    }

    public static float QuartEaseOutIn(float t, float b, float c, float d) {
      return Easing.QuartEaseOutIn(t, b, c - b, d);
    }

    public static float QuintEaseOut(float t, float b, float c, float d) {
      return Easing.QuintEaseOut(t, b, c - b, d);
    }

    public static float QuintEaseIn(float t, float b, float c, float d) {
      return Easing.QuintEaseIn(t, b, c - b, d);
    }

    public static float QuintEaseInOut(float t, float b, float c, float d) {
      return Easing.QuintEaseInOut(t, b, c - b, d);
    }

    public static float QuintEaseOutIn(float t, float b, float c, float d) {
      return Easing.QuintEaseOutIn(t, b, c - b, d);
    }

    public static float ElasticEaseOut(float t, float b, float c, float d) {
      return Easing.ElasticEaseOut(t, b, c - b, d);
    }

    public static float ElasticEaseIn(float t, float b, float c, float d) {
      return Easing.ElasticEaseIn(t, b, c - b, d);
    }

    public static float ElasticEaseInOut(float t, float b, float c, float d) {
      return Easing.ElasticEaseInOut(t, b, c - b, d);
    }

    public static float ElasticEaseOutIn(float t, float b, float c, float d) {
      return Easing.ElasticEaseOutIn(t, b, c - b, d);
    }

    public static float BounceEaseOut(float t, float b, float c, float d) {
      return Easing.BounceEaseOut(t, b, c - b, d);
    }

    public static float BounceEaseIn(float t, float b, float c, float d) {
      return Easing.BounceEaseIn(t, b, c - b, d);
    }

    public static float BounceEaseInOut(float t, float b, float c, float d) {
      return Easing.BounceEaseInOut(t, b, c - b, d);
    }

    public static float BounceEaseOutIn(float t, float b, float c, float d) {
      return Easing.BounceEaseOutIn(t, b, c - b, d);
    }

    public static float BackEaseOut(float t, float b, float c, float d) {
      return Easing.BackEaseOut(t, b, c - b, d);
    }

    public static float BackEaseIn(float t, float b, float c, float d) {
      return Easing.BackEaseIn(t, b, c - b, d);
    }

    public static float BackEaseInOut(float t, float b, float c, float d) {
      return Easing.BackEaseInOut(t, b, c - b, d);
    }

    public static float BackEaseOutIn(float t, float b, float c, float d) {
      return Easing.BackEaseOutIn(t, b, c - b, d);
    }

    public static float FeedbackEaseOut(float t, float b, float c, float d, float tl, int fs) {
      return Easing.FeedbackEaseOut(t, b, c - b, d, tl, fs);
    }
  }

}