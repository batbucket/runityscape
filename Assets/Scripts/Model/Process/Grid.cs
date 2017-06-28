using System;
using Scripts.Model.Interfaces;
using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Model.Processes {
    public class Grid : IButtonable {
        public const int DEFAULT_BUTTON_COUNT = 16;

        public static Action<IList<IButtonable>> ChangeGridFunc {
            set {
                changeGridFunc = value;
            }
        }
        private static Action<IList<IButtonable>> changeGridFunc;

        public string Text;
        public string Tooltip;
        public Sprite Icon;
        public Func<bool> Condition;
        public Action OnEnter;
        public IList<IButtonable> List;

        public Grid(string text) {
            this.Text = text;
            this.Tooltip = "";
            this.Icon = null;
            this.Condition = () => true;
            OnEnter = () => { };
            this.List = new List<IButtonable>();
        }

        public string ButtonText {
            get {
                return Text;
            }
        }

        public bool IsInvokable {
            get {
                return Condition.Invoke();
            }
        }

        public Sprite Sprite {
            get {
                return Icon;
            }
        }

        public string TooltipText {
            get {
                return Tooltip;
            }
        }

        public void Invoke() {
            changeGridFunc.Invoke(List);
            this.OnEnter();
        }
    }
}