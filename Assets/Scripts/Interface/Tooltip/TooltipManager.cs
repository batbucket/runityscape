using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
    Text tooltip;
    RectTransform tooltipTransform;
    Image tooltipImage;
    RectTransform q1;
    RectTransform q2;
    RectTransform q3;
    RectTransform q4;

	// Use this for initialization
	void Awake() {
        tooltip = gameObject.GetComponentInChildren<Text>();
        tooltipTransform = gameObject.GetComponentInChildren<Text>().gameObject.GetComponent<RectTransform>();
        tooltipImage = gameObject.GetComponentInChildren<Image>();
        q1 = Util.findChild(gameObject, "Q1").GetComponent<RectTransform>();
        q2 = Util.findChild(gameObject, "Q2").GetComponent<RectTransform>();
        q3 = Util.findChild(gameObject, "Q3").GetComponent<RectTransform>();
        q4 = Util.findChild(gameObject, "Q4").GetComponent<RectTransform>();
    }

	// Update is called once per frame
	void Update() {
        Util.setImageAlpha(tooltipImage, tooltip.text.Length == 0 ? 0 : 1); //No background on no tooltip

        if (q1.rect.Contains(Input.mousePosition)) {
            tooltipTransform.pivot = new Vector2(0, 0);
        } else if (q2.rect.Contains(Input.mousePosition)) {
            tooltipTransform.pivot = new Vector2(0, 0);
        } else if (q3.rect.Contains(Input.mousePosition)) {
            tooltipTransform.pivot = new Vector2(0, 0);
        } else if (q4.rect.Contains(Input.mousePosition)) {
            tooltipTransform.pivot = new Vector2(0, 0);
        }
    }
}
