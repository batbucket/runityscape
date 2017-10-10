using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Whomst a Spell can target.
    /// </summary>
    public sealed class TargetType {
        private const string MULTI_TARGET_ICON = "person";

        public readonly string Name;
        public readonly bool IsTargetEnemies;
        public readonly TargetCount TargetCount;
        private Func<Character, Page, ICollection<Character>> getFunc;

        private TargetType(string name, TargetCount targetCount, Func<Character, Page, ICollection<Character>> getFunc) {
            this.Name = name;
            this.TargetCount = targetCount;
            this.getFunc = getFunc;
        }

        public static TargetType SELF = new TargetType("Self", TargetCount.SINGLE_TARGET, (c, p) => new Character[] { c });
        public static TargetType SINGLE_ALLY = new TargetType("One ally", TargetCount.SINGLE_TARGET, (c, p) => p.GetAllies(c));

        //public static TargetType ALL_ALLIES = new TargetType("All allies", (c, p) => p.GetAllies(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType SINGLE_ENEMY = new TargetType("One enemy", TargetCount.SINGLE_TARGET, (c, p) => p.GetFoes(c));

        //public static TargetType ALL_ENEMIES = new TargetType("All enemies", (c, p) => p.GetFoes(c).Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType ANY = new TargetType("Any", TargetCount.SINGLE_TARGET, (c, p) => p.GetAll());

        //public static TargetType ALL = new TargetType("All", (c, p) => p.GetAll().Where(a => a.Stats.State == State.ALIVE).ToArray());
        public static TargetType NONE = new TargetType("None", TargetCount.NONE, (c, p) => new Character[0]);

        //public static HashSet<TargetType> SINGLE_TARGET_OPTIONS = new HashSet<TargetType>(new IdentityEqualityComparer<TargetType>()) { SELF, SINGLE_ALLY, SINGLE_ENEMY, ANY };

        public ICollection<Character> GetTargets(Character caster, Page current) {
            return getFunc.Invoke(caster, current);
        }

        public Process[] GetTargetProcesses(Page current, ISpellable spellable, Character caster, Action<Spell> spellHandler) {
            List<Process> processes = new List<Process>();
            SpellBook spellbook = spellable.GetSpellBook();
            if (this.TargetCount == TargetCount.SINGLE_TARGET) {
                foreach (Character target in GetTargets(caster, current)) {
                    processes.Add(GetTargetProcess(current, spellbook, caster, target, spellHandler));
                }
            } else if (this.TargetCount == TargetCount.MULTIPLE_TARGETS) {
                processes.Add(GetMultiTargetProcess(current, spellable, caster, spellHandler));
            }
            return processes.ToArray();
        }

        private Process GetMultiTargetProcess(Page current, ISpellable spellable, Character caster, Action<Spell> spellHandler) {
            SpellBook spellbook = spellable.GetSpellBook();
            return new Process(
                spellbook.TargetType.Name,
                Util.GetSprite(MULTI_TARGET_ICON),
                () => spellHandler(caster.Spells.CreateSpell(current, spellbook, caster)),
                () => spellbook.IsCastable(caster, spellbook.TargetType.GetTargets(caster, current))
                );
        }

        private Process GetTargetProcess(Page current, SpellBook spell, Character caster, Character target, Action<Spell> spellHandler) {
            Character[] targets = new Character[] { target };
            return new Process(
                target.Look.DisplayName,
                target.Look.Sprite,
                spell.CreateTargetDescription(target.Look.DisplayName),
                () => spellHandler(caster.Spells.CreateSpell(current, spell, caster, target)),
                () => spell.IsCastable(caster, targets)
                );
        }
    }
}

public enum TargetCount {
    MULTIPLE_TARGETS,
    SINGLE_TARGET,
    NONE
}