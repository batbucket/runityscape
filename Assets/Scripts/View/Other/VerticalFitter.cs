using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.Other {

    /// <summary>
    /// This script is used to resize Portraits so their holder can dynamically space them out
    /// when buffs are added underneath them.
    ///
    /// Adding a vertical layout group to each portrait does not work because we need custom anchors
    /// on all the child gameobjects of the portrait so the game can rescale to various aspect ratios,
    /// and a vertical layout group prevents this by forcing a particular alignment on its children.
    /// </summary>
    public class VerticalFitter : MonoBehaviour {
        [SerializeField]
        private RectTransform upperBound;

        [SerializeField]
        private RectTransform lowerBound;

        [SerializeField]
        private RectTransform parent;

        private void Update() {
            parent.sizeDelta = new Vector2(parent.sizeDelta.x, upperBound.localPosition.y - (lowerBound.localPosition.y - lowerBound.sizeDelta.y));
        }
    }
}
