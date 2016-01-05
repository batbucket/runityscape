using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
    Text text;
    RectTransform tooltipTransform;
    GameObject toolTipCombo; //Background + tooltip text

    public static readonly Vector2 TOP_LEFT_MIN = new Vector2(0, 1);
    public static readonly Vector2 TOP_LEFT_MAX = new Vector2(0, 1);
    public static readonly Vector2 TOP_LEFT_PIVOT = new Vector2(.5f, .5f);

    public static readonly Vector2 TOP_RIGHT_MIN = new Vector2(1, 1);
    public static readonly Vector2 TOP_RIGHT_MAX = new Vector2(1, 1);
    public static readonly Vector2 TOP_RIGHT_PIVOT = new Vector2(.5f, .5f);

    public static readonly Vector2 BOT_LEFT_MIN = new Vector2(0, 0);
    public static readonly Vector2 BOT_LEFT_MAX = new Vector2(0, 0);
    public static readonly Vector2 BOT_LEFT_PIVOT = new Vector2(0, 0);

    public static readonly Vector2 BOT_RIGHT_MIN = new Vector2(1, 0);
    public static readonly Vector2 BOT_RIGHT_MAX = new Vector2(1, 0);
    public static readonly Vector2 BOT_RIGHT_PIVOT = new Vector2(0, 0);

    public const int OFFSET = 50;

    // Use this for initialization
    void Awake() {
        text = gameObject.GetComponentInChildren<Text>();
        text.text = "hello";
        toolTipCombo = Util.findChild(gameObject, "TooltipBackground");
        tooltipTransform = toolTipCombo.GetComponent<RectTransform>();
    }

	// Update is called once per frame
	void Update() {
        toolTipCombo.SetActive(text.text.Length > 0);
        Vector3 newPos = Input.mousePosition;
        newPos.x = newPos.x - Screen.width / 2;
        newPos.y = newPos.y - Screen.height / 2;
        toolTipCombo.GetComponent<RectTransform>().position = newPos;
        Debug.Log(Input.mousePosition.x);

        //if (Input.mousePosition.x < Screen.width / 2) {
        //    //bottom left
        //    if (Input.mousePosition.y < Screen.height / 2) {
        //        setAnchor(TOP_RIGHT_MIN, TOP_RIGHT_MAX, TOP_RIGHT_PIVOT);

        //    //top left
        //    } else {
        //        setAnchor(BOT_RIGHT_MIN, BOT_RIGHT_MAX, BOT_RIGHT_PIVOT);
        //    }
        //} else {
        //    //bottom right
        //    if (Input.mousePosition.y < Screen.height / 2) {
        //        setAnchor(TOP_LEFT_MIN, TOP_LEFT_MAX, TOP_LEFT_PIVOT);

        //    //top right
        //    } else {
        //        setAnchor(BOT_LEFT_MIN, BOT_LEFT_MAX, BOT_LEFT_PIVOT);
        //    }
        //}
    }

    bool mouseInBounds(RectTransform rect) {
        return rect.rect.Contains(Input.mousePosition);
    }

    void setAnchor(Vector2 min, Vector2 max, Vector2 pivot) {
        tooltipTransform.anchorMin = min;
        tooltipTransform.anchorMax = max;
        tooltipTransform.pivot = pivot;
    }
}
