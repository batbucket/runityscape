using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputBoxView : PooledBehaviour {
    [SerializeField]
    InputField inputField;

    public string Input { get { return inputField.text; } set { inputField.text = value; } }

    public override void Reset() {
        inputField.text = "";
    }

    void Update() {
        //Force focus on InputField
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
