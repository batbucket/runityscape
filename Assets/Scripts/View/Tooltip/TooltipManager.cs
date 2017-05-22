using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.View.Tooltip {

    public class TooltipManager : MonoBehaviour {
        [SerializeField]
        private TooltipBox box;

        [SerializeField]
        private GraphicRaycaster grc;

        [SerializeField]
        private float distanceMultiplier;

        private bool isOverTooltipContent;

        private void Update() {
            box.Pivot = CalculatePivot();
            box.Position = CalculatePosition();
            UpdateTooltip();
            if (isOverTooltipContent) {
                StartCoroutine(DisplayTooltip());
            } else {
                SetRenderers(false);
            }
        }

        private void UpdateTooltip() {
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            grc.Raycast(ped, results);

            bool isOverContent = false;
            box.Sprite = null;
            box.Title = string.Empty;
            box.Body = string.Empty;

            foreach (RaycastResult rr in results) {
                Tip tip = rr.gameObject.GetComponent<Tip>();
                if (tip != null && tip.enabled && !string.IsNullOrEmpty(tip.Body)) {
                    Debug.Log("You are on a tooltippable object by the name of " + rr.gameObject.name);
                    box.Sprite = tip.Sprite;
                    box.Title = tip.Title;
                    box.Body = tip.Body;
                    isOverContent = true;
                }
            }
            isOverTooltipContent = isOverContent;
        }

        /// <summary>
        /// Disable renderers instead of disabling the tooltip box object so that the
        /// content size fitter has enough time to work with the new text.
        ///
        /// If we disable the gameobject instead, the tooltip box will appear to jump, since it
        /// needs a moment to resize to accomodate the new text.
        /// </summary>
        /// <param name="isEnabled">True if renderers of the tooltip box object and its children should be enabled</param>
        private void SetRenderers(bool isEnabled) {
            CanvasRenderer[] renderers = box.GetComponentsInChildren<CanvasRenderer>();
            foreach (CanvasRenderer r in renderers) {
                r.SetAlpha(isEnabled ? 1 : 0);
            }
        }

        /// <summary>
        /// Delay showing of the tooltip until the end of the frame so that
        /// there is enough time for the tooltip box's contentsizefitters
        /// to resize itself for the new text.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayTooltip() {
            yield return new WaitForEndOfFrame();
            SetRenderers(true);
        }

        private Vector2 CalculatePosition() {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float width = Screen.width;
            float height = Screen.height;

            float x = 0;
            float y = 0;

            if (Input.mousePosition.x < width / 2) {
                x = box.Outline.effectDistance.x * distanceMultiplier;
            } else {
                x = -box.Outline.effectDistance.x * distanceMultiplier;
            }

            if (Input.mousePosition.y < height / 2) {
                y = -box.Outline.effectDistance.y * distanceMultiplier;
            } else {
                y = box.Outline.effectDistance.y * distanceMultiplier;
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