using UnityEngine;

namespace Scripts.Model.Interfaces {

    /// <summary>
    /// Represents an object that can be converted
    /// into a button.
    /// </summary>
    public interface IButtonable {

        /// <summary>
        /// Gets the button text.
        /// </summary>
        /// <value>
        /// The button text.
        /// </value>
        string ButtonText { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is invokable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is invokable; otherwise, <c>false</c>.
        /// </value>
        bool IsInvokable { get; }

        /// <summary>
        /// Gets the tooltip text.
        /// </summary>
        /// <value>
        /// The tooltip text.
        /// </value>
        string TooltipText { get; }

        /// <summary>
        /// Gets the sprite.
        /// </summary>
        /// <value>
        /// The sprite.
        /// </value>
        Sprite Sprite { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is visible on disable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible on disable; otherwise, <c>false</c>.
        /// </value>
        bool IsVisibleOnDisable { get; }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        void Invoke();
    }
}