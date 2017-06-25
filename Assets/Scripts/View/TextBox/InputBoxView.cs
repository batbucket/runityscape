using Scripts.View.ObjectPool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.View.TextBoxes {

    /// <summary>
    /// Represents a box where the user can input their own text.
    /// Used for naming the user's character, and possibly other
    /// functions in the future.
    /// </summary>
    public class InputBoxView : PooledBehaviour {

        [SerializeField]
        private InputField inputField;

        public string Input { get { return inputField.text; } }

        public override void Reset() {
            inputField.text = "";
        }

        private void Update() {
            //Force focus on InputField
            EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
}