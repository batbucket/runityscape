using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.TextBoxes {

    /// <summary>
    /// Represents a TextBox but with a Character portrait next to it.
    /// </summary>
    public class AvatarBoxView : PooledBehaviour {

        [SerializeField]
        private Image avatar;

        [SerializeField]
        private TextBoxView textBoxView;

        /// <summary>
        /// Set the icon seen on the avatarbox
        /// </summary>
        public Sprite Sprite {
            set {
                avatar.sprite = value;
            }
        }

        /// <summary>
        /// Reset the view. Remove the sprite.
        /// </summary>
        public override void Reset() {
            textBoxView.Reset();
            avatar.sprite = null;
        }
    }
}