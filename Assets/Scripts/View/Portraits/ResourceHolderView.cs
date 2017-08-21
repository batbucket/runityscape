using Scripts.Model.Stats;
using Scripts.View.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.View.Portraits {
    public class ResourceHolderView : MonoBehaviour, IHolderView<ResourceHolderView.ResourceContent> {
        public struct ResourceContent {
            public readonly int Id;
            public readonly Color NegativeColor;
            public readonly Color FillColor;
            public readonly Sprite Sprite;
            public readonly string BarText;
            public readonly int Numerator;
            public readonly int Denominator;
            public readonly string Title;
            public readonly string TypeDescription;

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

        [SerializeField]
        private ResourceView resourcePrefab;

        private IDictionary<int, ResourceView> previous;

        public void Awake() {
            ObjectPoolManager.Instance.Register(resourcePrefab);
            this.previous = new Dictionary<int, ResourceView>();
        }

        public void Reset() {
            ResourceView[] rvs = gameObject.GetComponentsInChildren<ResourceView>();
            for (int i = 0; i < rvs.Length; i++) {
                ObjectPoolManager.Instance.Return(rvs[i]);
            }
        }

        public void AddContents(IEnumerable<ResourceContent> contents) {
            PortraitUtil.GetDifferences(ref previous, contents, resourcePrefab, gameObject, c => c.Id, (v, c) => SetupViewFromContent(v, c));
        }

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