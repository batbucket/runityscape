using System;
using System.Collections;
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

    public GameObject AddTextBox(TextBox textBox, Action callBack = null) {
        PooledBehaviour pb = ObjectPoolManager.Instance.Get(textBoxes[textBox.Type]);
        Util.Parent(pb.gameObject, gameObject);
        textBox.Write(pb.gameObject, callBack);
        return pb.gameObject;
    }

    public void AddTextBoxes(IList<TextBox> textBoxes) {
        StartCoroutine(MultiWrite(textBoxes));
    }

    private IEnumerator MultiWrite(IList<TextBox> textBoxes) {
        for (int i = 0; i < textBoxes.Count; i++) {
            TextBox t = textBoxes[i];
            AddTextBox(t);
            while (!t.IsDone) {
                yield return null;
            }
        }
    }

    public InputBoxView AddInputBox() {
        InputBoxView ibv = ObjectPoolManager.Instance.Get(inputBoxPrefab);
        Util.Parent(ibv.gameObject, gameObject);
        return ibv;
    }

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
}
