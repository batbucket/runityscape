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
    public class PortraitHolderView : MonoBehaviour, IHolderView<PortraitHolderView.PortraitContent> {
        public struct PortraitContent {
            public readonly int Id;
            public readonly string Name;
            public readonly string Tip;
            public readonly Sprite Sprite;
            public readonly IEnumerable<ResourceHolderView.ResourceContent> Resources;
            public readonly IEnumerable<BuffHolderView.BuffContent> Buffs;
            public readonly bool IsRevealed;

            public PortraitContent(int id, string name, string tip, Sprite sprite, IEnumerable<ResourceHolderView.ResourceContent> resources, IEnumerable<BuffHolderView.BuffContent> buffs, bool isRevealed) {
                Id = id;
                Name = name;
                Tip = tip;
                Sprite = sprite;
                Resources = resources;
                Buffs = buffs;
                IsRevealed = isRevealed;
            }
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
        public void AddContents(IEnumerable<PortraitContent> contents) {
            PortraitUtil.GetDifferences(ref previous,
                contents,
                portraitPrefab,
                this.gameObject,
                c => c.Id,
                (v, c) => SetupViewFromContent(v, c));
        }

        public PortraitView GetPortrait(int id) {
            return previous[id];
        }

        private void SetupViewFromContent(PortraitView view, PortraitContent content) {
            view.Setup(
                content.Sprite,
                content.Name,
                content.Tip,
                content.Resources,
                content.Buffs,
                content.IsRevealed
                );
        }

        private void Start() {
            previous = new Dictionary<int, PortraitView>();
            ObjectPoolManager.Instance.Register(portraitPrefab, 10);
        }
    }
}