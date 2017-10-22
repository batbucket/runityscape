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

            /// <summary>
            /// The identifier
            /// </summary>
            public readonly int Id;

            /// <summary>
            /// The name
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// The tip
            /// </summary>
            public readonly string Tip;

            /// <summary>
            /// The sprite
            /// </summary>
            public readonly Sprite Sprite;

            /// <summary>
            /// The resources
            /// </summary>
            public readonly List<ResourceHolderView.ResourceContent> Resources;

            /// <summary>
            /// The buffs
            /// </summary>
            public readonly IList<BuffHolderView.BuffContent> Buffs;

            /// <summary>
            /// The is revealed
            /// </summary>
            public readonly bool IsRevealed;

            /// <summary>
            /// Initializes a new instance of the <see cref="PortraitContent"/> struct.
            /// </summary>
            /// <param name="id">The identifier.</param>
            /// <param name="name">The name of the portrait.</param>
            /// <param name="tip">The hoverover tooltip.</param>
            /// <param name="sprite">The sprite.</param>
            /// <param name="resources">The resources.</param>
            /// <param name="buffs">The buffs.</param>
            /// <param name="isRevealed">if set to <c>true</c> resources and buffs are revealed.</param>
            public PortraitContent(int id, string name, string tip, Sprite sprite, List<ResourceHolderView.ResourceContent> resources, IList<BuffHolderView.BuffContent> buffs, bool isRevealed) {
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
        public void AddContents(IList<PortraitContent> contents) {
            PortraitUtil.GetDifferences(ref previous,
                contents,
                portraitPrefab,
                this.gameObject,
                c => c.Id,
                (v, c) => SetupViewFromContent(v, c));
        }

        /// <summary>
        /// Gets the portrait.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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
            ObjectPoolManager.Instance.Register(portraitPrefab);
        }
    }
}