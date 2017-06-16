using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Who a Spell can target.
    /// </summary>
    public sealed class TargetType {

        public readonly string Name;
        private Func<Character, Page, ICollection<Character>> getFunc;

        private TargetType(string name, Func<Character, Page, ICollection<Character>> getFunc) {
            this.Name = name;
            this.getFunc = getFunc;
        }

        public static TargetType SELF = new TargetType("Self", (c, p) => new Character[] { c });
        public static TargetType SINGLE_ALLY = new TargetType("Single ally", (c, p) => p.GetAllies(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType ALL_ALLIES = new TargetType("All allies", (c, p) => p.GetAllies(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType SINGLE_ENEMY = new TargetType("Single enemy", (c, p) => p.GetFoes(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType ALL_ENEMIES = new TargetType("All enemies", (c, p) => p.GetFoes(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType ANY = new TargetType("Single", (c, p) => p.GetAll().Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType ALL = new TargetType("All", (c, p) => p.GetAll().Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType NONE = new TargetType("None", (c, p) => null);

        public static HashSet<TargetType> SINGLE_TARGET_OPTIONS = new HashSet<TargetType> { SELF, SINGLE_ALLY, SINGLE_ENEMY, ANY };

        public ICollection<Character> GetTargets(Character caster, Battle b) {
            return getFunc.Invoke(caster, b);
        }

    }
}