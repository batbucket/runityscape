using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceType : IComparable {
    public enum DisplayMode {
        NUMERIC, PERCENTAGE, EXP, NONE
    }

    public static readonly IDictionary<DisplayMode, Func<int, int, string>> SPLAT_FUNCTIONS
        = new Dictionary<DisplayMode, Func<int, int, string>>() {
            { DisplayMode.NONE,       (a, b) =>          string.Format("", a.ToString("+#;-#")) },
            { DisplayMode.NUMERIC,    (a, b) =>          string.Format("{0}", a.ToString("+#;-#")) },
            { DisplayMode.PERCENTAGE, (a, b) => b == 0 ? string.Format("", a) : string.Format("{0}%", ((a * 100) / b).ToString("+#;-#")) },
            { DisplayMode.EXP,        (a, b) =>          string.Format("{0}", a.ToString("+#;-#")) }
        };

    public static readonly IDictionary<DisplayMode, Func<float, int, string>> BAR_DISPLAY_FUNCTIONS
        = new Dictionary<DisplayMode, Func<float, int, string>>() {
            { DisplayMode.NONE, (a, b) =>                string.Format("", (int)a, b) },
            { DisplayMode.NUMERIC, (a, b) =>             string.Format("{0}/{1}", (int)a, b) },
            { DisplayMode.PERCENTAGE, (a, b) => b == 0 ? string.Format("", a, b) : string.Format("{0}%", (int)((a * 100) / b)) }
        };

    string _name;
    public string Name { get { return _name; } }

    string _shortName;
    public string ShortName { get { return _shortName; } }

    string _description;
    public string Description { get { return _description; } }

    Color _fillColor;
    public Color FillColor { get { return _fillColor; } }

    Color _emptyColor;
    public Color EmptyColor { get { return _emptyColor; } }

    int _order;
    public int Order { get { return _order; } }

    AttributeType _dependent;
    public AttributeType Dependent { get { return _dependent; } }

    Action<Attribute, Resource> _calculation;
    public Action<Attribute, Resource> Calculation { get { return _calculation; } }

    Func<float, int, string> _displayFunction;
    public Func<float, int, string> DisplayFunction { get { return _displayFunction; } } //Display actual values or percentage
    Func<int, int, string> _splatFunction;
    public Func<int, int, string> SplatFunction { get { return _splatFunction; } }

    public ResourceType(string name, string shortName, string description, Color fillColor, Color emptyColor, int order, AttributeType dependent = null, Action<Attribute, Resource> calculation = null, DisplayMode displayMode = DisplayMode.NUMERIC) {
        this._name = name;
        this._shortName = shortName;
        this._description = description;
        this._fillColor = fillColor;
        this._emptyColor = emptyColor;
        this._order = order;
        this._dependent = dependent;
        this._calculation = calculation ?? ((a, r) => { });
        this._displayFunction = BAR_DISPLAY_FUNCTIONS[displayMode];
        this._splatFunction = SPLAT_FUNCTIONS[displayMode];
        ALL.Add(this);
    }

    public static readonly IList<ResourceType> ALL = new List<ResourceType>();

    public static readonly ResourceType HEALTH = new ResourceType("Life",
                                                                  "LIFE",
                                                                  "Vital state.",
                                                                  Color.green,
                                                                  Color.red,
                                                                  0,
                                                                  AttributeType.VITALITY,
                                                                  (a, r) => r.True = (int)a.False * 5);

    public static readonly ResourceType SKILL = new ResourceType("Skill",
                                                                 "SKIL",
                                                                 "Replenished on Attacks.",
                                                                 Color.yellow,
                                                                 new Color(51.0f / 255, 51.0f / 255, 0),
                                                                 1);

    public static readonly ResourceType MANA = new ResourceType("Mana",
                                                                "MANA",
                                                                "Magical resources.",
                                                                Color.blue,
                                                                Color.magenta,
                                                                2,
                                                                AttributeType.INTELLIGENCE,
                                                                (a, r) => r.True = (int)a.False * 5);

    public static readonly ResourceType CHARGE = new ResourceType("Charge",
                                                                  "CHRG",
                                                                  "Actions may be performed when full.",
                                                                  Color.white,
                                                                  Color.black,
                                                                  999,
                                                                  displayMode: DisplayMode.PERCENTAGE);

    public static readonly ResourceType CORRUPTION = new ResourceType("Corruption",
                                                                      "CRPT",
                                                                      "Corruption level.",
                                                                      Color.magenta,
                                                                      new Color(50.0f / 255, 0f, 50.0f / 255),
                                                                      3,
                                                                      displayMode: DisplayMode.PERCENTAGE);

    public static readonly ResourceType EXPERIENCE = new ResourceType("Experience",
                                                                "EXP",
                                                                "Needed to level up.",
                                                                Color.white,
                                                                Color.grey,
                                                                998,
                                                                AttributeType.LEVEL,
                                                                (a, r) => r.True = (int)(1 + Mathf.Pow(2, a.False)));

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