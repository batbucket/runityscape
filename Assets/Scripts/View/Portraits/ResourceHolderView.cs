using Scripts.Model.Stats;
using Scripts.View.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.Portraits {

    /// <summary>
    /// Holds any number of resources for a particular portrait.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="Scripts.View.Portraits.IHolderView{Scripts.View.Portraits.ResourceHolderView.ResourceContent}" />
    public class ResourceHolderView : MonoBehaviour, IHolderView<ResourceHolderView.ResourceContent> {

        /// <summary>
        ///
        /// </summary>
        public struct ResourceContent {

            /// <summary>
            /// The identifier
            /// </summary>
            public readonly int Id;

            /// <summary>
            /// The negative color seen under the fill color
            /// </summary>
            public readonly Color NegativeColor;

            /// <summary>
            /// The fill color seen above the negative color
            /// </summary>
            public readonly Color FillColor;

            /// <summary>
            /// The sprite
            /// </summary>
            public readonly Sprite Sprite;

            /// <summary>
            /// The bar text
            /// </summary>
            public readonly string BarText;

            /// <summary>
            /// The numerator
            /// </summary>
            public readonly int Numerator;

            /// <summary>
            /// The denominator
            /// </summary>
            public readonly int Denominator;

            /// <summary>
            /// The title
            /// </summary>
            public readonly string Title;

            /// <summary>
            /// The type description
            /// </summary>
            public readonly string TypeDescription;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResourceContent"/> struct.
            /// </summary>
            /// <param name="id">The identifier.</param>
            /// <param name="negativeColor">Color of the negative.</param>
            /// <param name="fillColor">Color of the fill.</param>
            /// <param name="sprite">The sprite.</param>
            /// <param name="barText">The bar text.</param>
            /// <param name="numerator">The numerator.</param>
            /// <param name="denominator">The denominator.</param>
            /// <param name="title">The title.</param>
            /// <param name="typeDescription">The type description.</param>
            public ResourceContent(int id, Color negativeColor, Color fillColor, Sprite sprite, string barText, int numerator, int denominator, string title, string typeDescription) {
                Id = id;
                NegativeColor = negativeColor;
                FillColor = fillColor;
                Sprite = sprite;
                BarText = barText;
                Numerator = numerator;
                Denominator = denominator;
                Title = title;
                TypeDescription = typeDescription;
            }
        }

        /// <summary>
        /// The resource prefab
        /// </summary>
        [SerializeField]
        private ResourceView resourcePrefab;

        /// <summary>
        /// The previous
        /// </summary>
        private IDictionary<int, ResourceView> previous;

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        public void Awake() {
            ObjectPoolManager.Instance.Register(resourcePrefab);
            this.previous = new Dictionary<int, ResourceView>();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset() {
            ResourceView[] rvs = gameObject.GetComponentsInChildren<ResourceView>();
            for (int i = 0; i < rvs.Length; i++) {
                ObjectPoolManager.Instance.Return(rvs[i]);
            }
        }

        /// <summary>
        /// Adds the contents.
        /// </summary>
        /// <param name="contents">The contents.</param>
        public void AddContents(IList<ResourceContent> contents) {
            PortraitUtil.GetDifferences(ref previous, contents, resourcePrefab, gameObject, c => c.Id, (v, c) => SetupViewFromContent(v, c));
        }

        /// <summary>
        /// Setups the content of the view from.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="content">The content.</param>
        private void SetupViewFromContent(ResourceView view, ResourceContent content) {
            view.Setup(
                content.NegativeColor,
                content.FillColor,
                content.Sprite,
                content.BarText,
                content.Numerator,
                content.Denominator,
                content.Title,
                content.TypeDescription);
        }
    }
}