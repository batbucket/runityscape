using Scripts.View.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.View.Portraits {
    public class BuffHolderView : MonoBehaviour, IHolderView<BuffHolderView.BuffContent> {
        public struct BuffContent {
            public readonly int Id;
            public readonly Color Color;
            public readonly Sprite Sprite;
            public readonly string Name;
            public readonly string Duration;
            public readonly string Description;

            public BuffContent(int id, Color color, Sprite sprite, string name, string duration, string description) {
                Id = id;
                Color = color;
                Sprite = sprite;
                Name = name;
                Duration = duration;
                Description = description;
            }
        }

        [SerializeField]
        private BuffView buffPrefab;

        private IDictionary<int, BuffView> previous;

        public void Awake() {
            ObjectPoolManager.Instance.Register(buffPrefab, 10);
            this.previous = new Dictionary<int, BuffView>();
        }

        public void Reset() {
            BuffView[] bvs = gameObject.GetComponentsInChildren<BuffView>();
            for (int i = 0; i < bvs.Length; i++) {
                ObjectPoolManager.Instance.Return(bvs[i]);
            }
        }

        public void AddContents(IEnumerable<BuffContent> contents) {
            PortraitUtil.GetDifferences(ref previous, contents, buffPrefab, gameObject, bc => bc.Id, (v, c) => SetupViewFromContent(v, c));
        }

        private void SetupViewFromContent(BuffView view, BuffContent content) {
            view.Setup(content.Name,
                content.Duration,
                content.Sprite,
                content.Color,
                content.Description);
        }
    }
}