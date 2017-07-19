using System;
using Scripts.View.ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Scripts.View.Effects {
    public class ShroudView : PooledBehaviour {
        [SerializeField]
        private Image image;
        [SerializeField]
        private RectTransform rectT;

        public Color Color {
            set {
                image.color = value;
            }
        }

        public Vector2 Dimensions {
            set {
                rectT.rect.Set(0, 0, value.x, value.y);
            }
        }

        public override void Reset() {
            image.color = Color.white;
        }
    }
}