using UnityEngine;
using System.Collections;
using Scripts.View.Sound;
using System;

namespace Scripts.View.Sounds {

    /// <summary>
    /// One shot sounds play once, hence they are not modified at all!
    /// </summary>
    /// <seealso cref="Scripts.View.Sounds.SoundHolderView" />
    public class OneShotHolderView : SoundHolderView {
        /// <summary>
        /// Modifies the clip.
        /// </summary>
        /// <param name="clip">The clip.</param>
        protected override void ModifyClip(ClipView clip) {

        }
    }
}