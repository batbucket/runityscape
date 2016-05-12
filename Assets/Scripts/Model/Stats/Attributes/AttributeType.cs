using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AttributeType : IComparable {
    public string Name { get; private set; }
    public string ShortName { get; private set; }
    public string PrimaryDescription { get; private set; }
    public string SecondaryDescription { get; private set; }
    public string ShortDescription { get; private set; }
    public Color Color { get; private set; }
    int order;

    private AttributeType(string name, string shortName, string primaryDescription, string secondaryDescription, string shortDescription, Color color, int order) {
        this.Name = name;
        this.ShortName = shortName;
        this.PrimaryDescription = primaryDescription;
        this.SecondaryDescription = secondaryDescription;
        this.ShortDescription = shortDescription;
        this.Color = color;
        this.order = order;
        ALL.Add(this);
    }

    public static readonly IList<AttributeType> ALL = new List<AttributeType>();

    public static readonly AttributeType STRENGTH = new AttributeType("Strength",
                                                                      "<color=red>STR</color>",
                                                                      "Increases basic attack damage and minimum spell effects.",
                                                                      "Increases health.",
                                                                      "Increases basic attack damage.",
                                                                      Color.red,
                                                                      0);

    public static readonly AttributeType INTELLIGENCE = new AttributeType("Intelligence",
                                                                          "<color=blue>INT</color>",
                                                                          "Increases spell effects and minimum basic attack damage.",
                                                                          "Increases critical hit rate.",
                                                                          "Increases spell effects.",
                                                                          Color.blue,
                                                                          1);

    public static readonly AttributeType DEXTERITY = new AttributeType("Dexterity",
                                                                       "<color=lime>DEX</color>",
                                                                       "Increases Charge generation and evasion rate.",
                                                                       "Increases critical hit rate and accuracy.",
                                                                       "Increases Charge generation.",
                                                                       Color.green,
                                                                       2);

    public static readonly AttributeType VITALITY = new AttributeType("Vitality",
                                                                      "<color=yellow>VIT</color>",
                                                                      "Increases health and healing from items.",
                                                                      "Increases armor and magical resistances.",
                                                                      "Increases health.",
                                                                      Color.yellow,
                                                                      3);

    public int CompareTo(object obj) {
        AttributeType other = (AttributeType)obj;
        int res = this.order.CompareTo(other.order);
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
        AttributeType at = (AttributeType)obj;
        return this.Name.Equals(at.Name);
    }

    public override int GetHashCode() {
        return this.Name.GetHashCode();
    }

    public static string SplatDisplay(int i) {
        return i.ToString("+#;-#");
    }

    public static Color DetermineColor(AttributeType attributeType, int amount) {
        Color textColor = Color.clear;
        if (amount < 0) {
            textColor = attributeType.Color;
        } else if (amount == 0) {
            textColor = Color.grey;
        } else {
            textColor = attributeType.Color;
        }
        return textColor;
    }
}