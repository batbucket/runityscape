using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Pages {
    public static class PageUtil {
        public static readonly Sprite SPELLBOOK = Util.GetSprite("spell-book");
        public static readonly Sprite INVENTORY = Util.GetSprite("swap-bag");
        public static readonly Sprite EQUIPMENT = Util.GetSprite("shoulder-armor");

        public static Grid GenerateSpellBooks(
            Page p,
            Grid previous,
            SpellParams owner,
            SpellBook excluded,
            IEnumerable<ISpellable> spellCollection,
            int count,
            Action<IPlayable> addPlay
            ) {
            return GenerateSpellableGrid(
                p,
                previous,
                owner,
                excluded,
                spellCollection,
                count,
                addPlay,
                SPELLBOOK,
                "Spells",
                "Cast a spell.");
        }

        public static Grid GenerateUseableItems(
            Page p,
            Grid previous,
            SpellParams owner,
            SpellBook excluded,
            IEnumerable<ISpellable> spellCollection,
            int count,
            Action<IPlayable> addPlay
            ) {
            return GenerateSpellableGrid(
                p,
                previous,
                owner,
                excluded,
                spellCollection,
                count,
                addPlay,
                INVENTORY,
                string.Format("Items ({0}/{1})", owner.Inventory.TotalOccupiedSpace, owner.Inventory.Capacity),
                string.Format("Use an item.\n{0} out of {1} inventory space is occupied.", owner.Inventory.TotalOccupiedSpace, owner.Inventory.Capacity)
                );
        }

        public static Grid GenerateBackableGrid(Grid previous, Sprite icon, int size, string name, string tooltip) {
            Grid grid = new Grid(name);
            grid.Icon = icon;
            grid.Tooltip = tooltip;
            IButtonable[] buttons = new IButtonable[size + 1];
            buttons[0] = GenerateBack(previous);
            grid.Array = buttons;
            return grid;
        }

        public static Grid GenerateSpellableGrid(
            Page p,
            Grid previous,
            SpellParams owner,
            SpellBook excluded,
            IEnumerable<ISpellable> spellCollection,
            int count,
            Action<IPlayable> addPlay,
            Sprite sprite,
            string name,
            string description) {

            Grid grid = GenerateBackableGrid(previous, sprite, count, name, description);
            int index = 1;
            foreach (ISpellable myS in spellCollection) {
                ISpellable s = myS;
                if (!s.Equals(excluded)) {
                    grid.Array[index++] = GenerateSpellProcess(p, grid, owner, s, addPlay);
                }
            }
            return grid;
        }

        public static Process GenerateSpellProcess(Page p, Grid previous, SpellParams owner, ISpellable spell, Action<IPlayable> handlePlayable) {
            SpellBook sb = spell.GetSpellBook();
            return new Process(sb.GetDetailedName(owner), sb.Icon, sb.CreateDescription(owner),
                () => {
                    if (sb.IsMeetPreTargetRequirements(owner.Stats)) {
                        GenerateTargets(p, previous, owner, sb, handlePlayable).Invoke();
                    }
                });
        }

        public static Grid GenerateTargets(Page p, Grid previous, SpellParams owner, SpellBook sb, Action<IPlayable> handlePlayable) {
            ICollection<Character> targets = sb.TargetType.GetTargets(owner.Character, p);
            Grid grid = GenerateBackableGrid(previous, sb.Icon, targets.Count, sb.Name, sb.CreateDescription(owner));

            grid.Icon = sb.Icon;

            int index = 1;
            foreach (Character myTarget in targets) {
                SpellParams target = new SpellParams(myTarget);
                grid.Array[index++] = GenerateTargetProcess(previous, owner, target, sb, handlePlayable);

            }
            return grid;
        }

        public static Process GenerateTargetProcess(Grid previous, SpellParams owner, SpellParams target, SpellBook sb, Action<IPlayable> handlePlayable) {
            return new Process(CreateDetailedTargetName(owner, target, sb),
                                target.Look.Sprite,
                                sb.CreateTargetDescription(owner, target),
                                () => {
                                    if (sb.IsCastable(owner, target)) {
                                        handlePlayable(owner.Spells.CreateSpell(sb, owner, target));
                                        previous.Invoke();
                                    }
                                });
        }

        public static Grid GenerateUnequipGrid(Grid previous, SpellParams owner, Action<IPlayable> handlePlayable) {
            Grid grid = GenerateBackableGrid(previous, Util.GetSprite("shoulder-armor"), EquipType.AllTypes.Count, "Equipment", "Manage equipped items.");
            int index = 1;
            foreach (EquipType myET in EquipType.AllTypes) {
                EquipType et = myET;
                Process p = null;
                Equipment eq = owner.Equipment;
                if (owner.Equipment.Contains(et)) {
                    CastUnequipItem unequip = new CastUnequipItem(owner.Inventory, owner.Equipment, eq.PeekItem(et));
                    p = new Process(unequip.Name, unequip.CreateDescription(owner),
                        () => {
                            handlePlayable(owner.Spells.CreateSpell(unequip, owner, owner));
                            previous.Invoke();
                        }
                            );
                } else {
                    p = new Process(Util.ColorString(et.Name, Color.grey), et.Sprite, "No item is equipped in this slot. Items can be equipped via the inventory menu.");
                }
                grid.Array[index++] = p;
            }
            return grid;
        }

        public static string CreateDetailedTargetName(SpellParams owner, SpellParams target, SpellBook sb) {
            return Util.ColorString(target.Look.DisplayName, sb.IsCastable(owner, target));
        }

        public static Process GenerateBack(IButtonable previous) {
            return new Process(
                "Back",
                Util.GetSprite("plain-arrow"),
                string.Format("Go back to {0}.", previous.ButtonText),
                () => previous.Invoke()
                );
        }
    }
}