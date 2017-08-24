using Scripts.Model.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Model.Tooltips {
    public class TooltipBundle {
        public Sprite Sprite;
        public string Title;
        public string Text;

        public TooltipBundle(Sprite sprite, string title, string text) {
            this.Sprite = sprite;
            this.Title = title;
            this.Text = text;
        }

        public TooltipBundle() {
            this.Sprite = null;
            this.Title = string.Empty;
            this.Text = string.Empty;
        }

        public TooltipBundle Tooltip {
            get {
                return this;
            }
        }
    }
}
