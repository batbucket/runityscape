﻿using UnityEngine;

namespace Scripts.Model.Interfaces {

    /// <summary>
    /// Represents an object that can be converted
    /// into a button.
    /// </summary>
    public interface IButtonable {
        string ButtonText { get; }
        bool IsInvokable { get; }
        string TooltipText { get; }
        Sprite Sprite { get; }
        bool IsVisibleOnDisable { get; }

        void Invoke();
    }
}