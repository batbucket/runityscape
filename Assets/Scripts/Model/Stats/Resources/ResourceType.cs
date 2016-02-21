using System;
using UnityEngine;

public sealed class ResourceType : IComparable {
    public string Name { get; private set; }
    public string ShortName { get; private set; }
    public string Description { get; private set; }
    public Color OverColor { get; private set; }
    public Color UnderColor { get; private set; }
    int order;

    public ResourceType(string name, string shortName, string description, Color overColor, Color underColor, int order) {
        this.Name = name;
        this.ShortName = shortName;
        this.Description = description;
        this.OverColor = overColor;
        this.UnderColor = underColor;
        this.order = order;
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
                                                                  999);

    public int CompareTo(object obj) {
        ResourceType other = (ResourceType)obj;
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
        ResourceType res = (ResourceType)obj;
        return this.Name.Equals(res.Name);
    }

    public override int GetHashCode() {
        return this.Name.GetHashCode();
    }
}