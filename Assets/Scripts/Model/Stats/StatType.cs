using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Stats {

    /// <summary>
    /// Type-safe enum representing the various stat types in the game.
    /// </summary>
    public sealed class StatType : IComparable {
        public static readonly IDictionary<BoundType, Bounds> AttributeBounds = new Dictionary<BoundType, Bounds>() {
            { BoundType.RESOURCE, new Bounds(0, 10000) },
            { BoundType.ASSIGNABLE, new Bounds(0, 999) },
            { BoundType.LEVEL, new Bounds(0, 100) }
        };

        public readonly Bounds Bounds;
        public readonly string Name;
        public readonly Sprite Sprite;
        public readonly string Description;
        public readonly Color Color;
        public readonly Color NegativeColor;

        private readonly int order;

        private StatType(
            BoundType boundType,
            string name,
            string spriteLoc,
            string description,
            Color color,
            int order,
            Color negativeColor) {
            AttributeBounds.TryGetValue(boundType, out Bounds);
            this.Name = Util.ColorString(name, color);
            this.Sprite = Util.LoadIcon(spriteLoc);
            this.Description = description;
            this.Color = color;
            this.order = order;
            this.NegativeColor = negativeColor;
        }

        private StatType(
            BoundType boundType,
            string name,
            string spriteLoc,
            string description,
            Color color,
            int order) : this(boundType, name, spriteLoc, description, color, order, Color.grey) { }

        public static readonly StatType LEVEL = new StatType(BoundType.LEVEL,
                                                                       "Level",
                                                                       "fox-head",
                                                                       "Current level of power.",
                                                                       Color.white,
                                                                       0);

        public static readonly StatType STRENGTH = new StatType(BoundType.ASSIGNABLE,
                                                                          "Strength",
                                                                          "fox-head",
                                                                          "Increases basic attack damage.",
                                                                          Color.red,
                                                                          0);

        public static readonly StatType INTELLECT = new StatType(BoundType.ASSIGNABLE,
                                                                           "Intellect",
                                                                           "fox-head",
                                                                           "Increases spell effects.",
                                                                           Color.blue,
                                                                           1);

        public static readonly StatType AGILITY = new StatType(BoundType.ASSIGNABLE,
                                                                         "Agility",
                                                                         "fox-head",
                                                                         "Increases critical hit rate and accuracy.",
                                                                         Color.green,
                                                                         2);

        public static readonly StatType VITALITY = new StatType(BoundType.ASSIGNABLE,
                                                                          "Vitality",
                                                                          "fox-head",
                                                                          "Increases Health.",
                                                                          Color.yellow,
                                                                          3);

        public static readonly HashSet<StatType> ASSIGNABLES = new HashSet<StatType>() { STRENGTH, INTELLECT, AGILITY, VITALITY };

        public static readonly StatType HEALTH = new StatType(BoundType.RESOURCE,
                                                              "Health",
                                                              "nested-hearts",
                                                              "Life state. Most units enter a deathlike state when their health reaches zero.",
                                                              Color.green,
                                                              0,
                                                              Color.red);

        public static readonly StatType SKILL = new StatType(BoundType.RESOURCE,
                                                                     "Skill",
                                                                     "concentration-orb",
                                                                     "A spell resource that is replenished on basic attacks.",
                                                                     Color.yellow,
                                                                     1);

        public static readonly StatType MANA = new StatType(BoundType.RESOURCE,
                                                                    "Mana",
                                                                    "fox-head",
                                                                    "Magical resources.",
                                                                    Color.blue,
                                                                    2);

        public static readonly StatType CHARGE = new StatType(BoundType.RESOURCE,
                                                                      "Charge",
                                                                      "fox-head",
                                                                      "Actions may be performed when full.",
                                                                      Color.white,
                                                                      999);

        public static readonly StatType CORRUPTION = new StatType(BoundType.RESOURCE,
                                                                          "Corruption",
                                                                          "fox-head",
                                                                          "Corruption level.",
                                                                          Color.magenta,
                                                                          3);

        public static readonly StatType EXPERIENCE = new StatType(BoundType.RESOURCE,
                                                                    "Experience",
                                                                    "upgrade",
                                                                    "Needed to level up.",
                                                                    Color.white,
                                                                    998);

        public static readonly StatType DEATH_EXP = new StatType(BoundType.RESOURCE,
                                                                    "Experience",
                                                                    "fox-head",
                                                                    "Needed to level up.",
                                                                    Color.white,
                                                                    998
                                                                    );

        public static readonly HashSet<StatType> RESOURCES = new HashSet<StatType>() { HEALTH, SKILL, MANA, CHARGE, CORRUPTION, EXPERIENCE };
        public static readonly HashSet<StatType> RESTORED = new HashSet<StatType>() { HEALTH, MANA };

        public int CompareTo(object obj) {
            StatType other = (StatType)obj;
            int res = this.order.CompareTo(other.order);
            if (res == 0) {
                res = this.Name.CompareTo(other.Name);
            }
            return res;
        }

        public override bool Equals(object obj) {
            return this == obj;
        }

        public override int GetHashCode() {
            return this.Name.GetHashCode();
        }

        public Color DetermineColor(float value) {
            Color c = Color.grey;
            if (value < 0) {
                c = this.NegativeColor;
            } else if (value == 0) {
                c = Color.grey;
            } else {
                c = this.Color;
            }
            return c;
        }
    }
}