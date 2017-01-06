using UnityEngine;
using UnityEngine.UI;


namespace Script.View.Tooltip {

    /// <summary>
    /// Represents the tooltip text that is visible
    /// just above the ActionGrid.
    /// </summary>
    public class TooltipView : MonoBehaviour {

        [SerializeField]
        private Text tooltipText;

        public string PageText;
        public string MouseText;

        private void Update() {
            tooltipText.text = MouseText;

            if (string.IsNullOrEmpty(tooltipText.text) && !string.IsNullOrEmpty(PageText)) {
                tooltipText.text = PageText;
            }
        }
    }
}