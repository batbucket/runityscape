using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceType : IComparable {
    public enum DisplayMode {
        NUMERIC, PERCENTAGE, NONE
    }

    public static readonly IDictionary<DisplayMode, Func<int, int, string>> SPLAT_FUNCTIONS
        = new Dictionary<DisplayMode, Func<int, int, string>>() {
            { DisplayMode.NONE,       (a, b) =>          string.Format("", a.ToString("+#;-#")) },
            { DisplayMode.NUMERIC,    (a, b) =>          string.Format("{0}", a.ToString("+#;-#")) },
            { DisplayMode.PERCENTAGE, (a, b) => b == 0 ? string.Format("", a) : string.Format("{0}%", ((a * 100) / b).ToString("+#;-#")) }
        };

    public static readonly IDictionary<DisplayMode, Func<int, int, string>> BAR_DISPLAY_FUNCTIONS
        = new Dictionary<DisplayMode, Func<int, int, string>>() {
            { DisplayMode.NONE, (a, b) =>                string.Format("", a, b) },
            { DisplayMode.NUMERIC, (a, b) =>             string.Format("{0}/{1}", a, b) },
            { DisplayMode.PERCENTAGE, (a, b) => b == 0 ? string.Format("", a, b) : string.Format("{0}%", (a * 100) / b) }
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

    Func<int, int, string> _displayFunction;
    public Func<int, int, string> DisplayFunction { get { return _displayFunction; } } //Display actual values or percentage
    Func<int, int, string> _splatFunction;
    public Func<int, int, string> SplatFunction { get { return _splatFunction; } }

    public ResourceType(string name, string shortName, string description, Color fillColor, Color emptyColor, int order, DisplayMode displayMode = DisplayMode.NUMERIC) {
        this._name = name;
        this._shortName = shortName;
        this._description = description;
        this._fillColor = fillColor;
        this._emptyColor = emptyColor;
        this._order = order;
        this._displayFunction = BAR_DISPLAY_FUNCTIONS[displayMode];
        this._splatFunction = SPLAT_FUNCTIONS[displayMode];
    }

    public static readonly ResourceType HEALTH = new ResourceType("Health",
                                                                  "LIFE",
                                                                  "Vital state.",
                                                                  Color.green,
                                                                  Color.red,
                                                                  0);

    public static readonly ResourceType SKILL = new ResourceType("Skill",
                                                                 "SKIL",
                                                                 "Replenished on Basic Attacks,",
                                                                 Color.yellow,
                                                                 Color.black,
                                                                 1);

    public static readonly ResourceType MANA = new ResourceType("Mana",
                                                                "MANA",
                                                                "Magical resources.",
                                                                Color.blue,
                                                                Color.magenta,
                                                                2);

    public static readonly ResourceType CHARGE = new ResourceType("Charge",
                                                                  "CHRG",
                                                                  "Actions may be performed when full.",
                                                                  Color.white,
                                                                  Color.black,
                                                                  999,
                                                                  DisplayMode.PERCENTAGE);

    public static readonly ResourceType[] ALL = new ResourceType[] { HEALTH, SKILL, MANA, CHARGE };

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
        return this.Name.Equals(res.Name);
    }

    public override int GetHashCode() {
        return this.Name.GetHashCode();
    }

    public static Color DetermineColor(ResourceType resourceType, int amount) {
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