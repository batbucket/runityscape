using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputBoxView : MonoBehaviour {
    [SerializeField]
    InputField inputField;

    public string Input { get { return inputField.text; } set { inputField.text = value; } }

    void Update() {
        //Force focus on InputField
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
