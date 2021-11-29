using System;
using DG.Tweening;
using TMPro;

public static class DOTweenExtensions
{
    public static Tweener DOTextInt(this TMP_Text text, int initialValue, int finalValue, float duration, Func<int, string> convertor)
    {
        return DOTween.To(
            () => initialValue,
            it => text.text = convertor(it),
            finalValue,
            duration
        );
    }
    
    public static Tweener DOTextInt(this TMP_Text text, int initialValue, int finalValue, float duration)
    {
        return DOTextInt(text, initialValue, finalValue, duration, it => it.ToString());
    }
    
    public static Tweener DOTextFloat(this TMP_Text text, float initialValue, float finalValue, float duration, Func<float, string> convertor)
    {
        return DOTween.To(
            () => initialValue,
            it => text.text = convertor(it),
            finalValue,
            duration
        );
    }


    public static Tweener DOTextFloat(this TMP_Text text, float initialValue, float finalValue, float duration)
    {
        return DOTextFloat(text, initialValue, finalValue, duration, it => it.ToString());
    }
    
    public static Tweener DOTextLong(this TMP_Text text, long initialValue, long finalValue, float duration, Func<long, string> convertor)
    {
        return DOTween.To(
            () => initialValue,
            it => text.text = convertor(it),
            finalValue,
            duration
        );
    }

    public static Tweener DOTextLong(this TMP_Text text, long initialValue, long finalValue, float duration)
    {
        return DOTextLong(text, initialValue, finalValue, duration, it => it.ToString());
    }
    
    public static Tweener DOTextDouble(this TMP_Text text, double initialValue, double finalValue, float duration, Func<double, string> convertor)
    {
        return DOTween.To(
            () => initialValue,
            it => text.text = convertor(it),
            finalValue,
            duration
        );
    }
    
    public static Tweener DOTextDouble(this TMP_Text text, double initialValue, double finalValue, float duration)
    {
        return DOTextDouble(text, initialValue, finalValue, duration, it => it.ToString());
    }
}