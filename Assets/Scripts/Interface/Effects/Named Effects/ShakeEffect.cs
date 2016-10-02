using UnityEngine;
using System.Collections;
using System;

public class ShakeEffect : CharacterEffect {
    private Vector3 originalPosition;

    private float duration;
    private bool isIndefinite;
    private float startIntensityPercentage;
    private float endIntensityPercentage;

    private void Init(PortraitView target, float startIntensityPercentage, float endIntensityPercentage) {
        this.originalPosition = target.Image.rectTransform.localPosition;
        this.startIntensityPercentage = startIntensityPercentage;
        this.endIntensityPercentage = endIntensityPercentage;
    }

    public ShakeEffect(PortraitView target, float duration, float startIntensityPercentage, float endIntensityPercentage) : base(target) {
        this.duration = duration;
        this.isIndefinite = false;
        Init(target, startIntensityPercentage, endIntensityPercentage);
    }

    public ShakeEffect(PortraitView target, float startIntensityPercentage, float endIntensityPercentage) : base(target) {
        this.isIndefinite = true;
        Init(target, startIntensityPercentage, endIntensityPercentage);
    }

    public override void CancelEffect() {
        target.Image.rectTransform.localPosition = originalPosition;
    }

    const float BASE_INTENSITY = 50;
    const float DURATION = 1.0f;

    protected override IEnumerator EffectRoutine() {
        return ShakeIcon(target, DURATION, startIntensityPercentage * BASE_INTENSITY, endIntensityPercentage * BASE_INTENSITY);
    }

    private const float NEAR_ZERO = 0.0001f;
    IEnumerator ShakeIcon(PortraitView target, float duration, float startIntensity, float endIntensity) {
        RectTransform icon = target.Image.rectTransform;
        Vector2 originalPos = icon.localPosition;
        float currentIntensity = startIntensity;
        while (icon != null || currentIntensity > NEAR_ZERO) {
            Mathf.SmoothDamp(currentIntensity, endIntensity, ref currentIntensity, duration);
            icon.localPosition = new Vector2(originalPos.x + Mathf.Sin(UnityEngine.Random.Range(0, 5)) * currentIntensity, originalPos.y + Mathf.Sin(UnityEngine.Random.Range(0, 5)) * currentIntensity);
            yield return null;
        }
    }
}
