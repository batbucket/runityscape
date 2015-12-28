using UnityEngine;
using System.Collections;

public class Testy : MonoBehaviour {

    public const string testy = "a b c d e f g h i j k l m n o p 1 2 3 4 5 6 7 8 ";

    // Use this for initialization
    void Start() {
        TextBoxManager t = gameObject.GetComponentInChildren<TextBoxManager>();
        t.post(testy, .01f);
    }

    // Update is called once per frame
    void Update() {

    }
}
