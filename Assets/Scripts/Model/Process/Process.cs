using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using System;
using UnityEngine;

namespace Scripts.Model.Processes {

    public class Process : IButtonable {
        private Action action;
        private Sprite sprite;
        private string description;
        private string name;
        private Func<bool> condition;
        private bool isVisibleOnDisable;

        /// <summary>
        /// Descriptionless constructor
        /// </summary>
        public Process(string name,
               Action action,
               Func<bool> condition = null) : this(name, null, null, action, condition) { }

        public Process(string name,
                Sprite sprite,
                Action action,
                Func<bool> condition = null) : this(name, sprite, null, action, condition) { }


        /// <summary>
        /// Dummy Process for indicating something
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Process(string name, string description) : this(name, description, () => { }) { }

        /// <summary>
        /// Dummy Process for indicating something
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Process(string name, Sprite sprite, string description) : this(name, sprite, description, () => { }) { }

        /// <summary>
        /// Spriteless constructor
        /// </summary>
        public Process(string name,
                       string description,
                       Action action,
                       Func<bool> condition = null) : this(name, null, description, action, condition) { }

        public Process(string name,
               Sprite sprite,
               string description,
               Action action,
               Func<bool> condition) {
            this.name = name;
            this.sprite = sprite;
            this.description = description;
            this.action = action;
            this.condition = condition ?? (() => true);
            this.isVisibleOnDisable = true;
        }

        public Process(string name, Sprite sprite, string description, Action action) : this(name, sprite, description, action, null) { }

        public Process() {
            this.name = "";
            this.description = "";
            this.action = (() => { });
            this.condition = (() => { return false; });
            this.isVisibleOnDisable = true;
        }

        public string ButtonText {
            get {
                return
                    Util.ColorString(name, IsInvokable ? Color.white : Color.red);
            }
        }

        public bool IsInvokable {
            get {
                return isInvokable();
            }
        }

        public string TooltipText {
            get {
                return description;
            }
        }

        public Sprite Sprite {
            get {
                return sprite;
            }
        }

        public bool IsVisibleOnDisable {
            get {
                return isVisibleOnDisable;
            }
        }

        public virtual void Invoke() {
            if (IsInvokable) {
                action.Invoke();
            }
        }

        public Process SetVisibleOnDisable(bool isVisibleOnDisable) {
            this.isVisibleOnDisable = isVisibleOnDisable;
            return this;
        }

        public static Process GetBackProcess(Page previous) {
            return new Process(
                "Back",
                Util.GetSprite("plain-arrow"),
                "Return to the previous page.",
                () => previous.Invoke()
                );
        }

        protected virtual bool isInvokable() {
            return condition.Invoke();
        }
    }
}