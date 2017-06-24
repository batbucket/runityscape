using Scripts.View.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Model.Characters;
using System;

namespace Scripts.View.Portraits {

    /// <summary>
    /// This class represents a "side" of the screen,
    /// where Character portraits may appear.
    /// </summary>
    public class PortraitHolderView : PooledBehaviour {
        public IDictionary<Character, PortraitBundle> CharacterViews;

        [SerializeField]
        private PortraitView portraitPrefab;

        /// <summary>
        /// Selectively update Portraits by only
        /// removing those whose Characters no longer exist.
        /// This allows us to use the shake effect.
        /// </summary>
        /// <param name="characters"></param>
        public void AddPortraits(ICollection<Character> characters) {
            //Set all existing isSets to false.
            List<Character> keys = new List<Character>(CharacterViews.Keys); //Can't modify Dictionary in foreach loop
            foreach (Character key in keys) {
                CharacterViews[key] = new PortraitBundle { portraitView = key.Presenter.PortraitView, isSet = false };
            }

            // Add or possibly replace new PortraitViews.
            foreach (Character c in characters) {
                PortraitView pv;
                if (!CharacterViews.ContainsKey(c)) {
                    pv = ObjectPoolManager.Instance.Get(portraitPrefab);
                    Util.Parent(pv.gameObject, gameObject);
                } else {
                    pv = CharacterViews[c].portraitView;
                }
                pv.PortraitName = c.Look.DisplayName;
                CharacterViews[c] = new PortraitBundle { portraitView = pv, isSet = true };
            }

            // Check if any isSets are false, if so, remove them and Destroy their gameObjects.
            // We can use same keys list as before since newly added keys cannot be false
            foreach (Character key in keys) {
                if (CharacterViews.ContainsKey(key) && !CharacterViews[key].isSet) {
                    ObjectPoolManager.Instance.Return(CharacterViews[key].portraitView);
                    CharacterViews.Remove(key);
                }
            }
        }

        public override void Reset() {
        }

        private void Start() {
            this.CharacterViews = new Dictionary<Character, PortraitBundle>(new IdNumberEqualityComparer<Character>());
            ObjectPoolManager.Instance.Register(portraitPrefab, 10);
        }

        public struct PortraitBundle {
            public bool isSet;
            public PortraitView portraitView;
        }
    }
}