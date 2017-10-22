using Scripts.View.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.View.Portraits {

    /// <summary>
    /// Holds the buffs for a particular portrait.
    /// </summary>
    public class BuffHolderView : MonoBehaviour, IHolderView<BuffHolderView.BuffContent> {

        public struct BuffContent {

            /// <summary>
            /// Unique identifier to tell buffviews apart
            /// </summary>
            public readonly int Id;

            /// <summary>
            /// Color of the buff's text
            /// </summary>
            public readonly Color Color;

            /// <summary>
            /// Sprite of the buff
            /// </summary>
            public readonly Sprite Sprite;

            /// <summary>
            /// Name of the buff
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// Buff duration
            /// </summary>
            public readonly string Duration;

            /// <summary>
            /// Buff description/tooltip
            /// </summary>
            public readonly string Description;

            /// <summary>
            /// Main
            /// </summary>
            /// <param name="id">Unique identifier to tell buffviews apart</param>
            /// <param name="color">Color of the buff's text</param>
            /// <param name="sprite">Sprite of the buff</param>
            /// <param name="name">Name of the buff</param>
            /// <param name="duration">Buff duration</param>
            /// <param name="description">Buff description/tooltip</param>
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
            ObjectPoolManager.Instance.Register(buffPrefab);
            this.previous = new Dictionary<int, BuffView>();
        }

        public void Reset() {
            BuffView[] bvs = gameObject.GetComponentsInChildren<BuffView>();
            for (int i = 0; i < bvs.Length; i++) {
                ObjectPoolManager.Instance.Return(bvs[i]);
            }
        }

        public void AddContents(IList<BuffContent> contents) {
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