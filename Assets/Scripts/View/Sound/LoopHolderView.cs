using UnityEngine;
using System.Collections;
using Scripts.View.Sound;
using System;

namespace Scripts.View.Sounds {

    public class LoopHolderView : SoundHolderView {
        protected override void ClipModifier(ClipView clip) {
            clip.IsLoop = true;
        }
    }

}