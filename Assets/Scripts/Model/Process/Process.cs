using Scripts.Model.Interfaces;
using System;
using UnityEngine;

namespace Scripts.Model.Processes {

    public class Process : IButtonable {
        private Action action;
        private string description;
        private bool isVisibleOnDisable;
        private string name;
        private Func<bool> playCondition;

        public Process(string name,
                       string description = "",
                       Action action = null,
                       Func<bool> playCondition = null,
                       bool isVisibleOnDisable = true) {
            this.name = name;
            this.description = description;
            this.action = action ?? (() => { });
            this.playCondition = playCondition ?? (() => { return true; });
            this.isVisibleOnDisable = isVisibleOnDisable;
        }

        public Process() {
            this.name = "";
            this.description = "";
            this.action = (() => { });
            this.playCondition = (() => { return false; });
            this.isVisibleOnDisable = false;
        }

        public string ButtonText {
            get {
                return
                    Util.Color(name, IsInvokable ? Color.white : Color.red);
            }
        }

        public bool IsInvokable {
            get {
                return isInvokable();
            }
        }

        public bool IsVisibleOnDisable {
            get {
                return isVisibleOnDisable;
            }
        }

        public string TooltipText {
            get {
                if (!string.IsNullOrEmpty(description)) {
                    return Util.Color(description, IsInvokable ? Color.white : Color.red);
                } else {
                    return "";
                }
            }
        }

        public Sprite Sprite {
            get {
                return Util.LoadIcon("fox-head");
            }
        }

        public virtual void Invoke() {
            if (IsInvokable) {
                action.Invoke();
            }
        }

        protected virtual bool isInvokable() {
            return playCondition.Invoke();
        }
    }
}