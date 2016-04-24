using UnityEngine;
using System.Collections;

public abstract class Perk {
    readonly string _name;
    public string Name { get { return _name; } }
    public Perk(string name) {
        this._name = name;
    }

    public override bool Equals(object obj) {
        if (obj == null) {
            return false;
        }
        Perk p = obj as Perk;
        if (p == null) {
            return false;
        }
        return this.Name.Equals(p.Name);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }

    public abstract class CharacterPerk : Perk {
        public enum PerkType {
            SLEEP,
            TICK,
            ON_FULL_CHARGE,
            BATTLE_START,
            BATTLE_END,
            BATTLE_VICTORY,
            SELF_DEFEAT,
            SELF_KILLED,
            ENEMY_DEFEAT,
            ENEMY_KILLED,
        };

        readonly PerkType _type;
        public PerkType Type { get { return _type; } }

        public CharacterPerk(string name, PerkType type) : base(name) {
            this._type = type;
        }

        public abstract void Invoke(Character caster, Character target = null);
    }

    public abstract class SpellPerk : Perk {
        public enum PerkType {
            RECIEVE_SPELL,
            CAST_SPELL
        };

        readonly PerkType _type;
        public PerkType Type { get { return _type; } }

        public SpellPerk(string name, PerkType type) : base(name) {
            this._type = type;
        }

        public abstract void Invoke(Spell s, Result r = null, Calculation c = null);
    }
}