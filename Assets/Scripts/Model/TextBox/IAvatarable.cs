using UnityEngine;

namespace Scripts.Model.TextBoxes {

    public interface IAvatarable {

        Sprite Sprite {
            get;
        }

        Color TextColor {
            get;
        }
    }
}