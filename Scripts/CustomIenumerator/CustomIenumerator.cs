using System;
using System.Collections;
using UnityEngine;

public static class CustomIenumerator
{
    #region Coroutine Image Alfa
    public static IEnumerator IEImageAlphaCor(float startVal, float endVal, float speed, AnimationCurve curve,
       Action<float> feedback, Action endFeedback = null)
    {
        for (float i = 0; i < 1f; i += Time.deltaTime * speed)
        {
            feedback?.Invoke(LerpUnclamped(startVal, endVal, curve.Evaluate(i)));
            yield return null;
        }

        feedback?.Invoke(endVal);
        endFeedback?.Invoke();
    }

    public static IEnumerator IEImageAlphaCor(Coroutine cor, float startVal, float endVal, float speed, AnimationCurve curve,
      Action<float> feedback, Action endFeedback = null)
    {
        for (float i = 0; i < 1f; i += Time.deltaTime * speed)
        {
            feedback?.Invoke(LerpUnclamped(startVal, endVal, curve.Evaluate(i)));
            yield return null;
        }
        feedback?.Invoke(endVal);
        endFeedback?.Invoke();
        cor = null;
    }

    public static float LerpUnclamped(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
    #endregion

    #region IERot
    public static IEnumerator IERot(Quaternion startVal, Quaternion endVal, float speed, AnimationCurve curve,
       Action<Quaternion> feedback, Action endFeedback = null)
    {
        for (float i = 0; i < 1f; i += Time.deltaTime * speed)
        {
            feedback?.Invoke(LerpUnclampedRot(startVal, endVal, curve.Evaluate(i)));
            yield return null;
        }

        feedback?.Invoke(endVal);
        endFeedback?.Invoke();
    }

    private static Quaternion LerpUnclampedRot(Quaternion a, Quaternion b, float t)
    {
        return new Quaternion(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
    }
    #endregion end IERot
}
