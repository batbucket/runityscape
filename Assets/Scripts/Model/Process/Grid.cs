using System;
using Scripts.Model.Interfaces;
using UnityEngine;

namespace Scripts.Model.Processes {
    public class Grid : IButtonable {
        public const int DEFAULT_BUTTON_COUNT = 16;
        public static Action<IButtonable[]> ChangeGridFunc;

        public string Text;
        public string Tooltip;
        public Sprite Icon;
        public Action Action;
        public Func<bool> Condition;
        public IButtonable[] Array;

        public Grid(string text) {
            this.Text = text;
            this.Tooltip = "";
            this.Icon = null;
            this.Action = () => { };
            this.Condition = () => true;
            this.Array = new IButtonable[0];
        }

        public Grid(Grid back, string text, params IButtonable[] content) : this(text) {
            this.Array = new IButtonable[content.Length + 1];
            Array[0] = back;

            int index = 1;
            foreach (IButtonable ib in content) {
                Array[index++] = ib;
            }
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
            ChangeGridFunc.Invoke(Array);
        }
    }
}