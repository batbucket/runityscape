using Scripts.View.Portraits;
using System.Collections;
using UnityEngine;

namespace Scripts.View.Effects {

    public class ShakeEffect : CharacterEffect {
        private const float BASE_INTENSITY = 50;
        private const float DURATION = 1.0f;
        private const float NEAR_ZERO = 0.0001f;
        private float duration;
        private float endIntensityPercentage;
        private Vector3 originalPosition;
        private float startIntensityPercentage;

        public ShakeEffect(PortraitView target, float duration, float startIntensityPercentage, float endIntensityPercentage) : base(target) {
            this.duration = duration;
            Init(target, startIntensityPercentage, endIntensityPercentage);
        }

        public ShakeEffect(PortraitView target, float startIntensityPercentage, float endIntensityPercentage) : base(target) {
            Init(target, startIntensityPercentage, endIntensityPercentage);
        }

        public override void CancelEffect() {
            StopCoroutine();
            target.Image.rectTransform.localPosition = originalPosition;
        }

        protected override IEnumerator EffectRoutine() {
            return ShakeIcon(target, DURATION, startIntensityPercentage * BASE_INTENSITY, endIntensityPercentage * BASE_INTENSITY);
        }

        private void Init(PortraitView target, float startIntensityPercentage, float endIntensityPercentage) {
            this.originalPosition = target.Image.rectTransform.localPosition;
            this.startIntensityPercentage = startIntensityPercentage;
            this.endIntensityPercentage = endIntensityPercentage;
        }

        private IEnumerator ShakeIcon(PortraitView target, float duration, float startIntensity, float endIntensity) {
            yield return new WaitForSeconds(0.01f);
            RectTransform icon = target.Image.rectTransform;
            Vector2 originalPos = icon.localPosition;
            float currentIntensity = startIntensity;
            while (icon != null || currentIntensity > NEAR_ZERO) {
                Mathf.SmoothDamp(currentIntensity, endIntensity, ref currentIntensity, duration);
                icon.localPosition = new Vector2(originalPos.x + Mathf.Sin(UnityEngine.Random.Range(0, 5)) * currentIntensity, originalPos.y + Mathf.Sin(UnityEngine.Random.Range(0, 5)) * currentIntensity);
                yield return null;
            }
            target.Image.rectTransform.localPosition = originalPosition;
        }
    }
}