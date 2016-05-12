using System;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderView : MonoBehaviour {

    [SerializeField]
    GameObject textBoxPrefab;
    [SerializeField]
    GameObject leftBoxPrefab;
    [SerializeField]
    GameObject rightBoxPrefab;
    [SerializeField]
    GameObject inputBoxPrefab;

    IList<GameObject> children;

    const int TEXTBOX_LIMIT = 25;

    void Awake() {
        this.children = new List<GameObject>();
    }

    public void AddTextBoxView(TextBox textBox, Action callBack = null) {
        GameObject g = (GameObject)GameObject.Instantiate(textBoxPrefab);
        children.Add(g);
        TextBoxView textBoxView = g.GetComponent<TextBoxView>();
        Util.Parent(g, gameObject);
        textBoxView.WriteText(textBox, callBack);
    }

    public void AddAvatarBoxView(bool isRightSide, string spriteLocation, TextBox textBox, Action callBack = null) {
        GameObject g = Instantiate(isRightSide ? rightBoxPrefab : leftBoxPrefab);
        children.Add(g);
        AvatarBoxView textBoxView = g.GetComponent<AvatarBoxView>();
        Util.Parent(g, gameObject);
        textBoxView.WriteText(Util.GetSprite(spriteLocation), textBox, callBack);
    }

    public InputBoxView AddInputBoxView() {
        GameObject g = Instantiate(inputBoxPrefab);
        children.Add(g);
        Util.Parent(g, gameObject);
        return g.GetComponent<InputBoxView>();
    }

    void Update() {
        if (children.Count > TEXTBOX_LIMIT) {
            Destroy(children[TEXTBOX_LIMIT]);
            children.RemoveAt(TEXTBOX_LIMIT);
        }
    }
}
