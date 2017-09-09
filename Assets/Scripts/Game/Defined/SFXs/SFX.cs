using Scripts.Model.Spells;
using Scripts.Presenter;
using Scripts.View.Effects;
using Scripts.View.ObjectPool;
using Scripts.View.Sounds;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Defined.SFXs {

    /// <summary>
    /// Various special effects for battle
    /// </summary>
    public static class SFX {
        /// <summary>
        /// Creates a hitsplat.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="splatText">The splat text.</param>
        /// <param name="splatColor">The splat color.</param>
        /// <param name="sprite">The sprite.</param>
        /// <returns></returns>
        public static IEnumerator DoHitSplat(GameObject parent, string splatText, Color splatColor, Sprite sprite = null) {
            HitsplatView hp = ObjectPoolManager.Instance.Get(EffectsManager.Instance.Hitsplat);
            return hp.Animation(parent, splatText, splatColor, sprite);
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <param name="soundLoc">The sound loc.</param>
        /// <returns></returns>
        public static IEnumerator PlaySound(string soundLoc) {
            Main.Instance.Sound.PlaySound(soundLoc);
            yield break;
        }

        /// <summary>
        /// Creates a hitsplat.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="splat">The splat.</param>
        /// <returns></returns>
        public static IEnumerator DoHitSplat(GameObject parent, SplatDetails splat) {
            return DoHitSplat(parent, splat.Text, splat.Color, splat.Sprite);
        }

        /// <summary>
        /// Waits for the specified duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        public static IEnumerator Wait(float duration) {
            yield return new WaitForSeconds(duration);
        }

        /// <summary>
        /// Effect where portrait runs up to another portrait, hits it, and retreats
        /// </summary>
        /// <param name="mover">The moving portrait.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="soundLoc">The sound loc.</param>
        /// <returns></returns>
        public static IEnumerator DoMeleeEffect(GameObject mover, GameObject destination, float duration, string soundLoc) {
            // Move mover to upper layer so it is on top of all elements
            int index = mover.transform.GetSiblingIndex();
            GameObject parent = mover.transform.parent.gameObject;
            Util.Parent(mover, EffectsManager.Instance.Foreground);

            Vector2 moverOriginalPos = mover.transform.position;
            yield return MoveTowards(mover, destination, duration / 3);
            Presenter.Main.Instance.Sound.PlaySound(soundLoc);
            yield return Shake(destination, 100, duration / 3);
            yield return MoveBack(mover, moverOriginalPos, duration / 3);

            Util.Parent(mover, parent);
            mover.transform.SetSiblingIndex(index);
        }

        /// <summary>
        /// Death effect for a portrait.
        /// </summary>
        /// <param name="portrait">The portrait.</param>
        /// <param name="effectHolder">The effect holder.</param>
        /// <param name="fadeDuration">Duration of the fade.</param>
        /// <returns></returns>
        public static IEnumerator DoDeathEffect(GameObject portrait, GameObject effectHolder, float fadeDuration) {
            Rect rect = effectHolder.GetComponent<RectTransform>().rect;
            Vector2 dimensions = new Vector2(rect.width, rect.height);

            ExplosionView ev = ObjectPoolManager.Instance.Get(EffectsManager.Instance.Explosion);
            ev.Dimensions = dimensions;
            Util.Parent(ev.gameObject, effectHolder);
            ev.Play();
            yield return DoHitSplat(effectHolder, "DEFEAT", Color.red, Util.GetSprite("skull-crossed-bones")); // this one is causing issues
            yield return new WaitUntil(() => ev.IsDone);
            ObjectPoolManager.Instance.Return(ev);
        }

        /// <summary>
        /// Does a page transition
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static IEnumerator DoPageTransition(Sprite sprite, string text) {
            Main.Instance.Title.Play(sprite, text);
            yield return new WaitUntil(() => Main.Instance.Title.IsDone);
        }

        /// <summary>
        /// Moves mover towards a destination.
        /// </summary>
        /// <param name="mover">The mover.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        private static IEnumerator MoveTowards(GameObject mover, GameObject destination, float duration) {
            float timer = 0;

            Vector3 destinationPos = destination.transform.position;
            float width = destination.GetComponent<RectTransform>().rect.width;

            // Do position corrections so we end up next to the icon instead of on top of it, but the mover needs to be on the inner side always

            // Mover ---- Destination,
            if (mover.transform.position.x < destinationPos.x) {
                destinationPos -= new Vector3(width, 0, 0);
            } else { // Destination ---- Mover
                destinationPos += new Vector3(width, 0, 0);
            }
            while ((timer += Time.deltaTime) < duration) {
                mover.transform.position = Vector2.Lerp(mover.transform.position, destinationPos, Mathf.SmoothStep(0, 1, timer / duration));
                yield return null;
            }
        }

        /// <summary>
        /// Shakes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="maxIntensity">The maximum intensity.</param>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        private static IEnumerator Shake(GameObject item, float maxIntensity, float duration) {
            Vector2 original = item.transform.position;
            float timer = 0;
            while ((timer += Time.deltaTime) < duration) {
                float intensity = Mathf.SmoothStep(maxIntensity, 0, timer / duration);
                item.transform.position = new Vector2(Mathf.Sin(Random.value) * intensity + original.x, Mathf.Sin(Random.value) * intensity + original.y);
                yield return null;
            }
            item.transform.position = original;
        }

        /// <summary>
        /// Moves the mover back to a destination.
        /// </summary>
        /// <param name="mover">The mover.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        private static IEnumerator MoveBack(GameObject mover, Vector2 destination, float duration) {
            float timer = 0;

            // Move towards destination
            while ((timer += Time.deltaTime) < duration) {
                mover.transform.position = Vector2.Lerp(mover.transform.position, destination, Mathf.SmoothStep(0, 1, timer / duration));
                yield return null;
            }
            mover.transform.position = destination;
        }
    }
}