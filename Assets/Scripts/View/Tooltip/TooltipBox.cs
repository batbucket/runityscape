using Scripts.View.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.View.Tooltip {

    public class TooltipBox : MonoBehaviour {

        [SerializeField]
        private Image image;

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text body;

        [SerializeField]
        private RectTransform backdrop;

        [SerializeField]
        private RectTransform rt;

        [SerializeField]
        private Outline outline;

        public Outline Outline {
            get {
                return outline;
            }
        }

        public Sprite Sprite {
            set {
                image.sprite = value;
            }
        }

        public bool IsIconEnabled {
            set {
                image.gameObject.SetActive(value);
            }
        }

        public string Title {
            set {
                title.text = value;
            }
        }

        public string Body {
            set {
                body.text = value;
            }
        }

        public Vector2 Position {
            set {
                transform.position = value;
            }
        }

        public Vector2 Pivot {
            set {
                rt.pivot = value;
            }
        }
    }
}