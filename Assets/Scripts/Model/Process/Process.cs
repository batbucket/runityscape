using Scripts.Model.Interfaces;
using System;
using UnityEngine;

namespace Scripts.Model.Processes {

    public class Process : IButtonable {
        private Action action;
        private Sprite sprite;
        private string description;
        private string name;
        private Func<bool> condition;

        /// <summary>
        /// Descriptionless constructor
        /// </summary>
        public Process(string name,
               Action action,
               Func<bool> condition = null) : this(name, null, null, action, condition) { }


        /// <summary>
        /// Dummy Process for indicating something
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Process(string name, string description) : this(name, description, () => { }) { }

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
        }

        public Process(string name, Sprite sprite, string description, Action action) : this(name, sprite, description, action, null) { }

        public Process() {
            this.name = "";
            this.description = "";
            this.action = (() => { });
            this.condition = (() => { return false; });
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

        public virtual void Invoke() {
            if (IsInvokable) {
                action.Invoke();
            }
        }

        protected virtual bool isInvokable() {
            return condition.Invoke();
        }
    }
}