using Scripts.Model.Spells;
using Scripts.Presenter;
using Scripts.View.Effects;
using Scripts.View.ObjectPool;
using Scripts.View.Sounds;
using System.Collections;
using UnityEngine;

namespace Scripts.Game.Defined.SFXs {

    public static class SFX {
        public static IEnumerator HitSplat(GameObject parent, string s, Color c, Sprite sprite = null) {
            HitsplatView hp = ObjectPoolManager.Instance.Get(EffectsManager.Instance.Hitsplat);
            Util.Parent(hp.gameObject, parent);
            return hp.Animation(s, c, sprite);
        }

        public static IEnumerator PlaySound(string soundLoc) {
            Main.Instance.Sound.PlaySound(soundLoc);
            yield break;
        }

        public static IEnumerator HitSplat(GameObject parent, SplatDetails splat) {
            return HitSplat(parent, splat.Text, splat.Color, splat.Sprite);
        }

        public static IEnumerator Melee(GameObject mover, GameObject destination, float duration, string soundLoc) {
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

        public static IEnumerator Death(GameObject parent) {
            return null;
        }

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

        private static IEnumerator MoveBack(GameObject mover, Vector2 destination, float duration) {
            float timer = 0;

            // Move towards destination
            while ((timer += Time.deltaTime) < duration) {
                mover.transform.position = Vector2.Lerp(mover.transform.position, destination, Mathf.SmoothStep(0, 1, timer / duration));
                yield return null;
            }
        }
    }
}