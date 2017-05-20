using Script.View.Tooltip;
using Scripts.View.ObjectPool;
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

        private void Update() {
            UpdateTooltip();
            box.Pivot = CalculatePivot();
            box.Position = CalculatePosition();
        }

        private void UpdateTooltip() {
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            grc.Raycast(ped, results);

            bool isOnTooltipContent = false;

            foreach (RaycastResult rr in results) {
                PooledBehaviour pb = rr.gameObject.GetComponent<PooledBehaviour>();
                if (pb != null) {
                    Debug.Log("You are on a tooltippable object by the name of " + rr.gameObject.name);
                    box.Sprite = pb.ToolIcon;
                    box.Title = pb.ToolTitle;
                    box.Body = pb.ToolBody;
                    isOnTooltipContent = true;
                }
            }

            box.gameObject.SetActive(isOnTooltipContent);
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