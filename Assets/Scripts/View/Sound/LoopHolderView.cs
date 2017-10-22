using UnityEngine;
using System.Collections;
using Scripts.View.Sound;
using System;

namespace Scripts.View.Sounds {

    /// <summary>
    /// Holds sounds that loop
    /// </summary>
    /// <seealso cref="Scripts.View.Sounds.SoundHolderView" />
    public class LoopHolderView : SoundHolderView {
        /// <summary>
        /// Modifies the clip to loop.
        /// </summary>
        /// <param name="clip">The clip.</param>
        protected override void ModifyClip(ClipView clip) {
            clip.IsLoop = true;
        }
    }

}