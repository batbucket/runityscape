using Scripts.Model.Stats;
using Scripts.View.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.View.Portraits {
    public class ResourceHolderView : MonoBehaviour, IHolderView<ResourceHolderView.ResourceContent> {
        public struct ResourceContent {
            public int Id;
            public Color NegativeColor;
            public Color FillColor;
            public Sprite Sprite;
            public string BarText;
            public int Numerator;
            public int Denominator;
            public string TypeDescription;
        }

        [SerializeField]
        private ResourceView resourcePrefab;

        private IDictionary<int, ResourceView> previous;

        public void Awake() {
            ObjectPoolManager.Instance.Register(resourcePrefab, 4);
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
                content.TypeDescription);
        }
    }
}