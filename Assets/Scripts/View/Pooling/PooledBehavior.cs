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
    public abstract class PooledBehaviour : MonoBehaviour {

        private Sprite icon;

        public virtual string ToolTitle {
            get {
                return "Placeholder Title";
            }
        }

        public virtual string ToolBody {
            get {
                return "This object does not have any tooltip text. Hello world!";
            }
        }

        public virtual Sprite ToolIcon {
            get {
                if (icon == null) {
                    icon = Util.LoadIcon(toolIconLoc);
                }
                return icon;
            }
        }

        protected virtual string toolIconLoc {
            get {
                return "info";
            }
        }

        /// <summary>
        /// Reset the state of this MonoBehavior.
        /// </summary>
        abstract public void Reset();
    }
}