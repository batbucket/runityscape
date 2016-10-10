using System;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderView : MonoBehaviour {

    [SerializeField]
    private TextBoxView textBoxPrefab;
    [SerializeField]
    private AvatarBoxView leftBoxPrefab;
    [SerializeField]
    private AvatarBoxView rightBoxPrefab;
    [SerializeField]
    private InputBoxView inputBoxPrefab;

    private IDictionary<TextBoxType, PooledBehaviour> textBoxes;

    private void Start() {
        textBoxes = new Dictionary<TextBoxType, PooledBehaviour>() {
            { TextBoxType.TEXT, textBoxPrefab },
            { TextBoxType.LEFT, leftBoxPrefab },
            { TextBoxType.RIGHT, rightBoxPrefab }
        };

        foreach (PooledBehaviour pb in textBoxes.Values) {
            ObjectPoolManager.Instance.Register(pb, 100);
        }
        ObjectPoolManager.Instance.Register(inputBoxPrefab, 1);
    }

    public GameObject AddTextBoxView(TextBox textBox, Action callBack = null) {
        PooledBehaviour pb = ObjectPoolManager.Instance.Get(textBoxes[textBox.Type]);
        Util.Parent(pb.gameObject, gameObject);
        textBox.Write(pb.gameObject, callBack);
        return pb.gameObject;
    }

    public InputBoxView AddInputBoxView() {
        InputBoxView ibv = ObjectPoolManager.Instance.Get(inputBoxPrefab);
        Util.Parent(ibv.gameObject, gameObject);
        return ibv;
    }
}
