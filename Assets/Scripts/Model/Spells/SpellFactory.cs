using Scripts.Model.Characters;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Creates Spells.
    /// </summary>
    public abstract class SpellFactory {
        protected readonly IDictionary<ResourceType, int> costs;
        protected string name;
        private readonly bool isSelfTargetable;
        private readonly SpellType spellType;
        private readonly TargetType targetType;
        private string abbreviation;
        private Color color;
        private string description;

        public SpellFactory(string name,
                            string description,
                            SpellType spellType,
                            TargetType targetType,
                            Cost[] costs = null,
                            bool isSelfTargetable = true,
                            string abbreviation = "???",
                            Color? color = null) {
            this.name = name;
            this.description = description;
            this.spellType = spellType;
            this.targetType = targetType;
            this.costs = new Dictionary<ResourceType, int>();
            if (costs != null) {
                foreach (Cost c in costs) {
                    this.costs.Add(new KeyValuePair<ResourceType, int>(c.resource, c.amount));
                }
            }
            this.IsEnabled = true;
            this.isSelfTargetable = isSelfTargetable;
            this.color = color ?? Color.white;
            this.abbreviation = abbreviation;
            Util.Assert(!(!isSelfTargetable && targetType == TargetType.SELF), "Cannot be non self targetable and have target type be self!");
        }

        public SpellFactory(
                        SpellType spellType,
                        TargetType targetType,
                        string name,
                        string description,
                        params Cost[] costs) {
            this.name = name;
            this.description = description;
            this.spellType = spellType;
            this.targetType = targetType;
            this.costs = new Dictionary<ResourceType, int>();
            if (costs != null) {
                foreach (Cost c in costs) {
                    this.costs.Add(new KeyValuePair<ResourceType, int>(c.resource, c.amount));
                }
            }
            this.IsEnabled = true;
            this.isSelfTargetable = true;
            this.color = Color.white;
            this.abbreviation = "???";
            Util.Assert(!(!isSelfTargetable && targetType == TargetType.SELF), "Cannot be non self targetable and have target type be self!");
        }

        public string Abbreviation { get { return abbreviation; } }
        public Color Color { get { return color; } }
        public IDictionary<ResourceType, int> Costs { get { return costs; } }
        public string Description { get { return description; } }
        public bool IsEnabled { get; set; }
        public bool IsSelfTargetable { get { return isSelfTargetable; } }
        public virtual string Name { get { return name; } }
        public SpellType SpellType { get { return spellType; } }
        public TargetType TargetType { get { return targetType; } }

        public void Cast(Character caster, Character target) {
            Spell spell = new Spell(this, caster, target);
            caster.CastSpells.Add(spell);
            target.RecievedSpells.Add(spell);

            if (!spell.IsOneShot) {
                target.AddToBuffs(spell);
            } else {
                spell.Invoke();
            }
        }

        public virtual Critical CreateCritical() {
            return new Critical();
        }

        public abstract Hit CreateHit();

        public virtual Miss CreateMiss() {
            return new Miss(
                sfx: (c, t, calc) => {
                    return new CharacterEffect[] {
                    new HitsplatEffect(t.Presenter.PortraitView, Color.grey, "MISS")
                    };
                }
            );
        }

        public override bool Equals(object obj) {
            // If parameter is null return false.
            if (obj == null) {
                return false;
            }

            // If parameter cannot be cast to SpellFactory return false.
            SpellFactory s = obj as SpellFactory;
            if ((object)s == null) {
                return false;
            }

            // Return true if the fields match:
            return this.Name.Equals(s.Name);
        }

        public string GetCosts() {
            IList<string> s = new List<string>();
            foreach (KeyValuePair<ResourceType, int> pair in costs) {
                s.Add(Util.Color(string.Format("{0} {1}", pair.Value, pair.Key.Name), pair.Key.FillColor));
            }
            return string.Join(", ", s.ToArray());
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }

        public virtual string GetNameAndCosts(Character caster) {
            StringBuilder s = new StringBuilder();
            s.Append((IsCastable(caster) ? Name : Util.Color(Name, Color.red)) + (costs.Count == 0 ? "" : " - "));
            List<string> elements = new List<string>();
            foreach (KeyValuePair<ResourceType, int> entry in costs) {
                if (entry.Key != ResourceType.CHARGE) {
                    Color resourceColor = entry.Key.FillColor;
                    int cost = entry.Value;
                    elements.Add(Util.Color("" + cost, resourceColor));
                }
            }
            s.Append(string.Join("/", elements.ToArray()));
            return s.ToString();
        }

        public bool IsCastable(Character caster, Character target = null) {
            if (targetType == TargetType.SELF) {
                target = caster;
            }
            if (target != null && !target.IsTargetable) {
                return false;
            }
            if (!caster.IsCharged || !IsEnabled) {
                return false;
            }
            if (!IsSelfTargetable && caster == target) {
                return false;
            }
            foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
                if (caster.GetResourceCount(resourceCost.Key, false) < resourceCost.Value) {
                    return false;
                }
            }
            return target == null || Castable(caster, target);
        }

        public bool IsSingleTargetQuickCastable(Character caster, IList<Character> targets) {
            return targets.Count == 1;
        }

        public void TryCast(Character caster, Character target) {
            if (target != null && target.State != CharacterState.KILLED) {
                TryCast(caster, new List<Character>() { target });
            }
        }

        public void TryCast(Character caster, IList<Character> targets) {
            if (targets.Any(target => IsCastable(caster, target))) {
                ConsumeResources(caster);
                OnOnce(caster);
                foreach (Character target in targets) {
                    Cast(caster, target);
                }
            }
        }

        protected virtual bool Castable(Character caster, Character target) {
            if (SpellType == SpellType.ACT) {
                return caster.Actions.Contains(this);
            } else if (SpellType == SpellType.MERCY) {
                return caster.Flees.Contains(this);
            } else {
                return this is Attack || caster.Spells.Contains(this);
            }
        }

        protected virtual void ConsumeResources(Character caster) {
            foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
                caster.AddToResource(resourceCost.Key, false, -resourceCost.Value);
            }
            caster.Discharge();
        }

        protected virtual void OnOnce(Character caster) {
        }
    }
}