using Scripts.View.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Scripts.View.Effects {

    public class ImageView : PooledBehaviour {
        [SerializeField]
        private Image image;

        [SerializeField]
        private RectTransform rectTransform;

        public Sprite Sprite {
            set {
                image.sprite = value;
            }
        }

        public Vector2 Size {
            set {
                rectTransform.localScale = value;
            }
        }

        public Vector2 Position {
            set {
                rectTransform.position = value;
            }
        }

        public float Alpha {
            set {
                Util.SetImageAlpha(image, value);
            }
        }

        public override void Reset() {
            Sprite = null;
        }
    }
}