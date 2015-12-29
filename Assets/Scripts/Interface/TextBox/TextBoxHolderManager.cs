using UnityEngine;

public class TextBoxHolderManager : MonoBehaviour {
    public TextBoxManager addTextBox() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        Util.parent(g, gameObject);
        return g.GetComponent<TextBoxManager>();
    }
}
