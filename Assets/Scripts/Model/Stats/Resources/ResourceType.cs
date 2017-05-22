using Scripts.Model.Stats.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Stats.Resources {

    /// <summary>
    /// Type-safe enum representation of the various resource types in the game.
    /// </summary>
    public sealed class ResourceType : IComparable {

        public enum DisplayMode {
            NUMERIC, PERCENTAGE, NONE
        }

        public static readonly IDictionary<DisplayMode, Func<int, int, string>> SPLAT_FUNCTIONS
            = new Dictionary<DisplayMode, Func<int, int, string>>() {
            { DisplayMode.NONE,       (a, b) =>          string.Format("", a.ToString("+#;-#")) },
            { DisplayMode.NUMERIC,    (a, b) =>          string.Format("{0}", a.ToString("+#;-#")) },
            { DisplayMode.PERCENTAGE, (a, b) => b == 0 ? string.Format("", a) : string.Format("{0}%", ((a * 100) / b).ToString("+#;-#")) },
            };

        public static readonly IDictionary<DisplayMode, Func<float, int, string>> BAR_DISPLAY_FUNCTIONS
            = new Dictionary<DisplayMode, Func<float, int, string>>() {
            { DisplayMode.NONE, (a, b) =>                string.Format("", (int)a, b) },
            { DisplayMode.NUMERIC, (a, b) =>             string.Format("{0}", (int)a, b) },
            { DisplayMode.PERCENTAGE, (a, b) => b == 0 ? string.Format("", a, b) : string.Format("{0}%", (int)((a * 100) / b)) },
            };

        private string _name;
        public string Name { get { return _name; } }

        private Sprite _sprite;
        public Sprite Sprite { get { return _sprite; } }

        private string _description;
        public string Description { get { return _description; } }

        private Color _fillColor;
        public Color FillColor { get { return _fillColor; } }

        private Color _emptyColor;
        public Color EmptyColor { get { return _emptyColor; } }

        private int _order;
        public int Order { get { return _order; } }

        private AttributeType _dependent;
        public AttributeType Dependent { get { return _dependent; } }

        private Func<int, int> _calculation;
        public Func<int, int> Calculation { get { return _calculation; } }

        private Func<float, int, string> _displayFunction;
        public Func<float, int, string> DisplayFunction { get { return _displayFunction; } } //Display actual values or percentage
        private Func<int, int, string> _splatFunction;
        public Func<int, int, string> SplatFunction { get { return _splatFunction; } }

        private ResourceType(string name, string iconLoc, string description, Color fillColor, Color emptyColor, int order, AttributeType dependent = null, Func<int, int> calculation = null, DisplayMode displayMode = DisplayMode.NUMERIC) {
            this._name = name;
            this._sprite = Util.LoadIcon(iconLoc);
            this._description = description;
            this._fillColor = fillColor;
            this._emptyColor = emptyColor;
            this._order = order;
            this._dependent = dependent;
            this._calculation = calculation ?? ((a) => { return 0; });
            this._displayFunction = BAR_DISPLAY_FUNCTIONS[displayMode];
            this._splatFunction = SPLAT_FUNCTIONS[displayMode];
            ALL.Add(this);
        }

        public static readonly IList<ResourceType> ALL = new List<ResourceType>();

        public static readonly ResourceType HEALTH = new ResourceType("Health",
                                                                      "nested-hearts",
                                                                      "Life state. Most units enter a deathlike state when their health reaches zero.",
                                                                      Color.green,
                                                                      Color.red,
                                                                      0,
                                                                      AttributeType.VITALITY,
                                                                      a => a * 5);

        public static readonly ResourceType SKILL = new ResourceType("Skill",
                                                                     "concentration-orb",
                                                                     "A spell resource that is replenished on basic attacks.",
                                                                     Color.yellow,
                                                                     new Color(51.0f / 255, 51.0f / 255, 0),
                                                                     1);

        public static readonly ResourceType MANA = new ResourceType("Mana",
                                                                    "fox-head",
                                                                    "Magical resources.",
                                                                    Color.blue,
                                                                    Color.magenta,
                                                                    2,
                                                                    AttributeType.INTELLIGENCE,
                                                                    a => a * 5);

        public static readonly ResourceType CHARGE = new ResourceType("Charge",
                                                                      "fox-head",
                                                                      "Actions may be performed when full.",
                                                                      Color.white,
                                                                      Color.black,
                                                                      999,
                                                                      displayMode: DisplayMode.PERCENTAGE);

        public static readonly ResourceType CORRUPTION = new ResourceType("Corruption",
                                                                          "fox-head",
                                                                          "Corruption level.",
                                                                          Color.magenta,
                                                                          new Color(50.0f / 255, 0f, 50.0f / 255),
                                                                          3,
                                                                          displayMode: DisplayMode.PERCENTAGE);

        public static readonly ResourceType EXPERIENCE = new ResourceType("Experience",
                                                                    "upgrade",
                                                                    "Needed to level up.",
                                                                    Color.white,
                                                                    Color.grey,
                                                                    998,
                                                                    AttributeType.LEVEL,
                                                                    a => (int)(1 + Mathf.Pow(2, a)));

        public static readonly ResourceType DEATH_EXP = new ResourceType("Experience",
                                                                    "fox-head",
                                                                    "Needed to level up.",
                                                                    Color.white,
                                                                    Color.grey,
                                                                    998,
                                                                    displayMode: DisplayMode.NONE
                                                                    );

        public static readonly ResourceType[] RESTORED_RESOURCES = new ResourceType[] { HEALTH, MANA };

        public int CompareTo(object obj) {
            ResourceType other = (ResourceType)obj;
            int res = this.Order.CompareTo(other.Order);
            if (res == 0) {
                res = this.Name.CompareTo(other.Name);
            }
            return res;
        }

        public override bool Equals(object obj) {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            ResourceType res = (ResourceType)obj;
            return this == res;
        }

        public override int GetHashCode() {
            return this.Name.GetHashCode();
        }

        public static Color DetermineColor(ResourceType resourceType, float amount) {
            Color textColor = Color.clear;
            if (amount < 0) {
                textColor = resourceType.EmptyColor;
            } else if (amount == 0) {
                textColor = Color.grey;
            } else {
                textColor = resourceType.FillColor;
            }
            return textColor;
        }
    }
}