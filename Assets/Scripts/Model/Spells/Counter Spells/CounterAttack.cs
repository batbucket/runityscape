using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CounterAttack : CounterSpellFactory {
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_TEXT = "* {0} strikes back at {1} for {2} damage!";
    public const string CRITICAL_TEXT = "* {0} critically strikes back at {1} for {2} damage!";
    public const string MISS_TEXT = "* {0} strikes back at {1}... But it missed!";

    public CounterAttack() : base(SPELL_TYPE, TARGET_TYPE) { }

    protected override void OnOnce(Character caster, SpellDetails other) {
        other.Calculation.Clear();
        other.Result.CreateTextFunc = (c, t, calc, o) => {
            return string.Format("* {0}'s {2} was blocked by {1}!", c.Name, t.Name, other.Spell.SpellFactory.Name);
        };
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    calculation: (c, t, o) => {
                        return new Calculation(
                            targetResources: new Dictionary<ResourceType, PairedInt>() {
                                { ResourceType.HEALTH, new PairedInt(0, -100) }
                            }
                        );
                    },
                    //Implicit perform
                    createText: (c, t, calc, o) => {
                        return string.Format(SUCCESS_TEXT, c.Name, t.Name, -calc.TargetResources[ResourceType.HEALTH].False);
                    },
                    sfx: (c, t, calc, o) => {
                        Game.Instance.Sound.Play("Sounds/Attack_0");
                    }
                ))
            }
        };
    }
}
