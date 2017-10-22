using Scripts.View.ActionGrid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.View.TextInput {

    public class InputView : MonoBehaviour {
        [SerializeField]
        private Text text;

        [SerializeField]
        private InputField field;

        [SerializeField]
        private ActionGridView actionGrid;

        public string Request {
            set {
                this.text.text = value;
            }
        }

        public string Input {
            get {
                return this.field.text;
            }
        }


        private void OnEnable() {
            actionGrid.IsHotKeysEnabled = false;

            //Force focus on InputField
            EventSystem.current.SetSelectedGameObject(field.gameObject, null);
            field.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void OnDisable() {
            actionGrid.IsHotKeysEnabled = true;
        }
    }
}
