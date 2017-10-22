using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Game.Defined.Characters;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Represents the appearance of a Character.
    /// </summary>
    /// <seealso cref="Scripts.Model.SaveLoad.ISaveable{Scripts.Model.SaveLoad.SaveObjects.LookSave}" />
    public class Look : ISaveable<LookSave> {
        /// <summary>
        /// Their name
        /// </summary>
        public string Name;

        /// <summary>
        /// Their text color
        /// </summary>
        protected Color textColor;
        /// <summary>
        /// Their tooltip
        /// </summary>
        protected string tooltip;
        /// <summary>
        /// Their breed
        /// </summary>
        protected Breed breed;

        /// <summary>
        /// Their suffix
        /// </summary>
        private string suffix;
        /// <summary>
        /// Their sprite
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="Look"/> class.
        /// </summary>
        public Look() : this("Default", "fox-head", string.Empty, Breed.UNKNOWN, Color.white) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Look"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="spriteLoc">The sprite loc.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="breed">The breed.</param>
        /// <param name="textColor">Color of the text.</param>
        public Look(string name, string spriteLoc, string tooltip, Breed breed, Color textColor) : this(name, Util.GetSprite(spriteLoc), tooltip, breed, textColor) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Look"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="breed">The breed.</param>
        /// <param name="textColor">Color of the text.</param>
        public Look(string name, Sprite sprite, string tooltip, Breed breed, Color textColor) {
            this.Name = name;
            this.sprite = sprite;
            this.tooltip = tooltip;
            this.breed = breed;
            this.textColor = textColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Look"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="spriteLoc">The sprite loc.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="breed">The breed.</param>
        public Look(string name, string spriteLoc, string tooltip, Breed breed) : this(name, spriteLoc, tooltip, breed, Color.white) { }

        /// <summary>
        /// Gets the breed.
        /// </summary>
        /// <value>
        /// The breed.
        /// </value>
        public Breed Breed {
            get {
                return breed;
            }
        }

        /// <summary>
        /// Gets or sets the sprite.
        /// </summary>
        /// <value>
        /// The sprite.
        /// </value>
        public Sprite Sprite {
            get {
                return sprite;
            }
            protected set {
                this.sprite = value;
            }
        }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public string Tooltip {
            get {
                return tooltip;
            }
        }

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        /// <value>
        /// The color of the text.
        /// </value>
        public Color TextColor {
            get {
                return textColor;
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName {
            get {
                return string.Format("{0}{1}", Name, string.IsNullOrEmpty(suffix) ? "" : string.Format(" {0}", suffix));
            }
        }

        /// <summary>
        /// Sets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix {
            set {
                suffix = value;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            var item = obj as Look;

            if (item == null) {
                return false;
            }

            return
                this.Name.Equals(item.Name)
                && this.textColor.Equals(item.textColor)
                && this.tooltip.Equals(item.tooltip)
                && this.breed.Equals(item.breed)
                && this.sprite.Equals(item.sprite);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the save object. A save object contains the neccessary
        /// information to initialize a clean class to its saved state.
        /// A save object is also serializable.
        /// </summary>
        /// <returns></returns>
        public LookSave GetSaveObject() {
            return new LookSave(Name, sprite, textColor, tooltip, breed);
        }

        /// <summary>
        /// Initializes from save object.
        /// </summary>
        /// <param name="saveObject">The save object.</param>
        public void InitFromSaveObject(LookSave saveObject) {
            this.Name = saveObject.Name;
            this.sprite = saveObject.Sprite;
            this.textColor = saveObject.TextColor;
            this.tooltip = saveObject.Tooltip;
            this.breed = saveObject.Breed;
            this.suffix = string.Empty;
        }
    }
}
