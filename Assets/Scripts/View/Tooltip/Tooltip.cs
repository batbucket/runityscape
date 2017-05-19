using Scripts.View.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.View.Tooltip {

    public class Tooltip : MonoBehaviour {
        [SerializeField]
        private float distanceMultiplier;

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

        [SerializeField]
        private GraphicRaycaster grc;

        public Sprite Sprite {
            set {
                image.sprite = value;
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

        private void Update() {
            UpdateTooltip();
            rt.pivot = CalculatePivot();
            Position = CalculatePosition();
        }

        private static bool foo;

        private void UpdateTooltip() {
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            grc.Raycast(ped, results);
            Debug.Log(results.Count);
            bool foundTooltip = false;
            foreach (RaycastResult rr in results) {
                PooledBehaviour pb = rr.gameObject.GetComponent<PooledBehaviour>();
                Debug.Log(rr.gameObject.name);
                if (pb != null) {
                    foundTooltip = true;
                    Sprite = pb.ToolIcon;
                    Title = pb.ToolTitle;
                    Body = pb.ToolText;
                }
            }

            if (!foundTooltip) {
                Sprite = null;
                Title = null;
                Body = null;
            }
        }

        private Vector2 CalculatePosition() {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float width = Screen.width;
            float height = Screen.height;

            float x = 0;
            float y = 0;

            if (Input.mousePosition.x < width / 2) {
                x = outline.effectDistance.x * distanceMultiplier;
            } else {
                x = -outline.effectDistance.x * distanceMultiplier;
            }

            if (Input.mousePosition.y < height / 2) {
                y = -outline.effectDistance.y * distanceMultiplier;
            } else {
                y = outline.effectDistance.y * distanceMultiplier;
            }

            return new Vector2(position.x + x, position.y + y);
        }

        private Vector2 CalculatePivot() {
            float width = Screen.width;
            float height = Screen.height;

            float x = 0;
            float y = 0;

            if (Input.mousePosition.x < width / 2) {
                x = 0;
            } else {
                x = 1;
            }

            if (Input.mousePosition.y < height / 2) {
                y = 0;
            } else {
                y = 1;
            }

            return new Vector2(x, y);
        }
    }
}