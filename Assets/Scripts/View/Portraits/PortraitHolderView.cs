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
        public struct PortraitContent {
            public int id;
            public string name;
            public Sprite sprite;
        }

        [SerializeField]
        private PortraitView portraitPrefab;

        private IDictionary<int, PortraitView> previous;

        /// <summary>
        /// Selectively update Portraits by only
        /// removing those whose Characters no longer exist.
        /// This allows us to use the shake effect.
        /// </summary>
        /// <param name="characters"></param>
        public void AddPortraits(IEnumerable<PortraitContent> contents) {
            IDictionary<int, PortraitView> current = new Dictionary<int, PortraitView>();
            foreach (PortraitContent pc in contents) {
                PortraitView pv = null;

                // New contents also has previous character -> pass on to this iteration of the dictionary
                if (previous.ContainsKey(pc.id)) {
                    pv = previous[pc.id];
                    previous.Remove(pc.id); // Remove from previous dict

                    // New contents has entirely new character -> make a new portraitview
                } else {
                    pv = ObjectPoolManager.Instance.Get(portraitPrefab);
                    Util.Parent(pv.gameObject, gameObject);
                }
                SetupPortrait(pv, pc);
                current.Add(new KeyValuePair<int, PortraitView>(pc.id, pv));
            }

            // Previous dict should only have unrefreshed characters, return these.
            foreach (PortraitView pv in previous.Values) {
                ObjectPoolManager.Instance.Return(pv);
            }
            previous = current;
        }

        public PortraitView GetPortrait(int id) {
            return previous[id];
        }

        public override void Reset() {

        }

        private void SetupPortrait(PortraitView view, PortraitContent content) {
            view.PortraitName = content.name;
            view.Sprite = content.sprite;
        }

        private void Start() {
            previous = new Dictionary<int, PortraitView>();
            ObjectPoolManager.Instance.Register(portraitPrefab, 10);
        }
    }
}