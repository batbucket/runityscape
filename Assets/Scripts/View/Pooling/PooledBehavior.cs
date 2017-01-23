using Scripts.Presenter;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.View.ObjectPool {

    /// <summary>
    /// Represents a MonoBehavior that can be pooled.
    /// Pooled objects are Reset() and returned to the object pool when "destroyed."
    /// So we can retrieve them from there instead of doing costly instantiations.
    /// </summary>
    public abstract class PooledBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private bool isMouseOver;

        protected virtual string HoverOverText {
            get {
                return "";
            }
        }

        private bool IsShowHoverOver {
            get {
                return !string.IsNullOrEmpty(HoverOverText) && isMouseOver;
            }
        }

        public void MouseEnter() {
            Util.Log("enter");
            isMouseOver = true;
        }

        public void MouseExit() {
            isMouseOver = false;
            Game.Instance.Tooltip.MouseText = "";
        }

        public void OnPointerEnter(PointerEventData eventData) {
            MouseEnter();
        }

        public void OnPointerExit(PointerEventData eventData) {
            MouseExit();
        }

        /// <summary>
        /// Reset the state of this MonoBehavior.
        /// </summary>
        abstract public void Reset();

        private void LateUpdate() {
            if (IsShowHoverOver) {
                Game.Instance.Tooltip.MouseText = HoverOverText;
            }
        }
    }
}