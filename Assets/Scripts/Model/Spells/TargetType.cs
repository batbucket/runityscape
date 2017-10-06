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
        public readonly bool IsTargetEnemies;
        private Func<Character, Page, ICollection<Character>> getFunc;

        private TargetType(string name, bool isTargetEnemies, Func<Character, Page, ICollection<Character>> getFunc) {
            this.Name = name;
            this.IsTargetEnemies = isTargetEnemies;
            this.getFunc = getFunc;
        }

        public static TargetType SELF = new TargetType("Self", false, (c, p) => new Character[] { c });
        public static TargetType SINGLE_ALLY = new TargetType("One ally", false, (c, p) => p.GetAllies(c));

        //public static TargetType ALL_ALLIES = new TargetType("All allies", (c, p) => p.GetAllies(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType SINGLE_ENEMY = new TargetType("One enemy", true, (c, p) => p.GetFoes(c));

        //public static TargetType ALL_ENEMIES = new TargetType("All enemies", (c, p) => p.GetFoes(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType ANY = new TargetType("Any", true, (c, p) => p.GetAll());

        //public static TargetType ALL = new TargetType("All", (c, p) => p.GetAll().Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType NONE = new TargetType("None", false, (c, p) => new Character[0]);

        //public static HashSet<TargetType> SINGLE_TARGET_OPTIONS = new HashSet<TargetType>(new IdentityEqualityComparer<TargetType>()) { SELF, SINGLE_ALLY, SINGLE_ENEMY, ANY };

        public ICollection<Character> GetTargets(Character caster, Page p) {
            return getFunc.Invoke(caster, p);
        }
    }
}