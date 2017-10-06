using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Whomst a Spell can target.
    /// </summary>
    public sealed class TargetType {
        public readonly string Name;
        public readonly bool IsTargetAllies;
        private Func<Character, Page, ICollection<Character>> getFunc;

        private TargetType(string name, bool isTargetAllies, Func<Character, Page, ICollection<Character>> getFunc) {
            this.Name = name;
            this.IsTargetAllies = isTargetAllies;
            this.getFunc = getFunc;
        }

        public static TargetType SELF = new TargetType("Self", true, (c, p) => new Character[] { c });
        public static TargetType SINGLE_ALLY = new TargetType("Single ally", true, (c, p) => p.GetAllies(c).Where(a => a.Stats.State == State.ALIVE).ToArray());

        //public static TargetType ALL_ALLIES = new TargetType("All allies", (c, p) => p.GetAllies(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType SINGLE_ENEMY = new TargetType("Single enemy", false, (c, p) => p.GetFoes(c).Where(a => a.Stats.State == State.ALIVE).ToArray());

        //public static TargetType ALL_ENEMIES = new TargetType("All enemies", (c, p) => p.GetFoes(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType NONE = new TargetType("None", true, (c, p) => new Character[0]);

        //public static HashSet<TargetType> SINGLE_TARGET_OPTIONS = new HashSet<TargetType>(new IdentityEqualityComparer<TargetType>()) { SELF, SINGLE_ALLY, SINGLE_ENEMY, ANY };

        public ICollection<Character> GetTargets(Character caster, Page p) {
            return getFunc.Invoke(caster, p);
        }
    }
}