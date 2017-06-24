using System;
using System.Collections.Generic;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using UnityEngine;
using Scripts.Model.Characters;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Items;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Game.Defined.Serialized.Characters {

    public class Player : Brain {
        public static readonly Attack Attack = new Attack();
        private Battle battle;

        public override void DetermineAction(Action<IPlayable> addPlay) {
            Grid main = new Grid("Back");
            main.Array = new IButtonable[] {
                GenerateTargets(main, Attack, addPlay),
                GenerateSpellBooks(main, Owner.Spells, Owner.Spells.Count, addPlay, Util.GetSprite("spell-book"), "Spell", "Cast a spell."),
                GenerateSpellBooks(main, Owner.Inventory, Owner.Inventory.UniqueItemCount, addPlay, Util.GetSprite("swap-bag"),
                    string.Format("Item ({0}/{1})", Owner.Inventory.TotalOccupiedSpace, Owner.Inventory.Capacity),
                    string.Format("Use an item.\n{0} out of {1} inventory space is occupied.", Owner.Inventory.TotalOccupiedSpace, Owner.Inventory.Capacity)),
                GenerateUnequipGrid(main, addPlay)
                };
            battle.Actions = main.Array;
        }

        protected override void PageSetupHelper(Battle battle) {
            this.battle = battle;
        }

        private Grid GenerateBackableGrid(Grid previous, Sprite icon, int size, string name, string tooltip) {
            Grid grid = new Grid(name);
            grid.Icon = icon;
            grid.Tooltip = tooltip;
            IButtonable[] buttons = new IButtonable[size + 1];
            buttons[0] = GenerateBack(previous);
            grid.Array = buttons;
            return grid;
        }

        private Grid GenerateSpellBooks(Grid previous, IEnumerable<ISpellable> spellCollection, int count, Action<IPlayable> addFunc, Sprite sprite, string name, string description) {
            Grid grid = GenerateBackableGrid(previous, sprite, count, name, description);
            int index = 1;
            foreach (ISpellable myS in spellCollection) {
                ISpellable s = myS;
                if (!s.Equals(Attack)) {
                    grid.Array[index++] = GenerateSpellProcess(grid, s, addFunc);
                }
            }
            return grid;
        }

        private Process GenerateSpellProcess(Grid previous, ISpellable spell, Action<IPlayable> addFunc) {
            SpellBook sb = spell.GetSpellBook();
            return new Process(sb.GetDetailedName(new SpellParams(Owner)), sb.Icon, sb.CreateDescription(new SpellParams(Owner)),
                () => {
                    if (sb.CasterHasResources(Owner.Stats)) {
                        GenerateTargets(previous, sb, addFunc).Invoke();
                    }
                });
        }

        private Grid GenerateTargets(Grid previous, SpellBook sb, Action<IPlayable> addFunc) {
            ICollection<Character> targets = sb.TargetType.GetTargets(Owner, battle);
            Grid grid = GenerateBackableGrid(previous, sb.Icon, targets.Count, sb.Name, sb.CreateDescription(new SpellParams(Owner)));

            grid.Icon = sb.Icon;

            int index = 1;
            foreach (Character myTarget in targets) {
                Character target = myTarget;
                grid.Array[index++] = GenerateTargetProcess(target, sb, addFunc);

            }
            return grid;
        }

        private Process GenerateTargetProcess(Character target, SpellBook sb, Action<IPlayable> addFunc) {
            return new Process(CreateDetailedTargetName(target, sb),
                                target.Look.Sprite,
                                sb.CreateTargetDescription(Owner, target),
                                () => {
                                    if (sb.IsCastable(new SpellParams(Owner), new SpellParams(target))) {
                                        addFunc(Spells.CreateSpell(sb, this.Owner, target));
                                    }
                                });
        }

        private Grid GenerateUnequipGrid(Grid previous, Action<IPlayable> addPlay) {
            Grid grid = GenerateBackableGrid(previous, Util.GetSprite("shoulder-armor"), EquipType.AllTypes.Count, "Equipment", "Manage equipped items.");
            int index = 1;

            foreach (EquipType myET in EquipType.AllTypes) {
                EquipType et = myET;
                Process p = null;
                Equipment eq = Owner.Equipment;
                if (Owner.Equipment.Contains(et)) {
                    CastUnequipItem unequip = new CastUnequipItem(Owner.Inventory, Owner.Equipment, eq.PeekItem(et));
                    p = new Process(unequip.Name, unequip.CreateDescription(new SpellParams(Owner)),
                        () => addPlay(Spells.CreateSpell(unequip, Owner, Owner)));
                } else {
                    p = new Process(Util.ColorString(et.Name, Color.grey), "No item is equipped in this slot. Items can be equipped via the inventory menu.");
                }
                grid.Array[index] = p;
            }
            return grid;
        }

        private string CreateDetailedTargetName(Character target, SpellBook sb) {
            return Util.ColorString(target.Look.DisplayName, sb.IsCastable(new SpellParams(Owner), new SpellParams(target)));
        }

        private Process GenerateBack(Grid previous) {
            return new Process(
                "Back",
                "Go back to the previous menu.",
                () => previous.Invoke()
                );
        }
    }

    public class DebugAI : Brain {
        public static readonly Attack Attack = new Attack();

        public override void DetermineAction(Action<IPlayable> addPlay) {
            addPlay(Spells.CreateSpell(Attack, Owner, foes.PickRandom()));
        }
    }
}