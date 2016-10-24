using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Model.BattlePage {
    public static class PageUtil {

        //Creates a Process that brings up the "Who to target with this Spell" display
        public static Process CreateSpellProcess(BattlePage page, SpellFactory spell, Character caster) {
            return new Process(spell.GetNameAndCosts(caster), string.Format("{0} > {1}  {2}\n{3}", caster.DisplayName, spell.Name, spell.GetCosts(), spell.Description),
                () => {
                    DetermineTargetSelection(page, spell, caster);
                });
        }

        //Show worn equipment first that can be unequipped, then Items in inventory that can be equipped
        public static IList<Process> CreateEquipmentProcesses(BattlePage page, Character current) {
            IList<Process> processes = new List<Process>();
            foreach (EquippableItem myEquip in current.Equipment) {
                EquippableItem equip = myEquip;
                processes.Add(CreateUnequipProcess(current, equip, current.Equipment));
            }
            foreach (Item i in current.Items) {
                if (i is EquippableItem) {
                    EquippableItem e = (EquippableItem)i;
                    processes.Add(CreateSpellProcess(page, e, current));
                }
            }
            return processes;
        }

        public static Process CreateUnequipProcess(Character caster, EquippableItem item, ICollection<SpellFactory> equipment) {
            return new Process(
                Util.Color(string.Format("{0}{1}", item.Name, "(E)"), Color.yellow),
                string.Format("{0} > {1} > {2}\n{3}", caster.DisplayName, "UNEQUIP", item.Name, item.Description), () => {
                    Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.", caster.DisplayName, item.Name), Color.white, TextEffect.FADE_IN));
                    equipment.Remove(item);
                    item.CancelBonus(caster);
                });
        }

        public static IList<Process> CreateSpellList(BattlePage page, ICollection<SpellFactory> spells, Character caster) {
            IList<Process> processes = new List<Process>();
            foreach (SpellFactory mySpell in spells) {
                SpellFactory spell = mySpell;
                processes.Add(CreateSpellProcess(page, spell, caster));
            }
            return processes;
        }

        //Creates a process that targets a specific Character with a spell
        public static Process CreateTargetProcess(BattlePage page, SpellFactory spell, Character caster, Character target) {
            return new Process(
                spell.IsCastable(caster, target) ? target.DisplayName : Util.Color(target.DisplayName, Color.red),
                string.Format("{0} > {1} > {2}\n{3}",
                caster.DisplayName,
                spell.Name,
                target.DisplayName,
                spell.Description),
                () => {
                    spell.TryCast(caster, target);
                    SpellCastEnd(spell, caster, page);
                }
            );
        }

        public static IList<Process> CreateTargetList(BattlePage page, Character caster, SpellFactory spell, IList<Character> targets) {
            IList<Process> processes = new List<Process>();
            foreach (Character myTarget in targets) {
                Character target = myTarget;
                processes.Add(CreateTargetProcess(page, spell, caster, target));
            }
            return processes;
        }

        public static Process CreateBackButton(BattlePage page, Character current, string parentSelectionName) {
            return new Process("Back", string.Format("{0} > {1}\n", current.DisplayName, parentSelectionName),
                () => {
                    page.currentSelectionNode = page.currentSelectionNode.Parent;
                });
        }

        public static Process CreateSwitchButton(Character current, BattlePage page) {
            return new Process(Selection.SWITCH.Name, string.Format(Selection.SWITCH.Text, current.DisplayName),
                () => {
                    //Quick switch if there's only one other abledCharacter to switch with
                    IList<Character> abledCharacters = page.GetAll().FindAll(c => !current.Equals(c) && c.IsControllable && c.IsCharged);
                    if (abledCharacters.Count == 1) {
                        page.activeCharacter = abledCharacters[0];
                        page.currentSelectionNode = page.selectionRoot;
                    } else {
                        page.currentSelectionNode = page.currentSelectionNode.FindChild(Selection.SWITCH);
                    }
                });
        }

        public static IList<Process> CreateSwitchMenu(Character current, BattlePage page) {
            IList<Process> processes = new List<Process>();
            foreach (Character myTarget in page.GetAll()) {
                Character target = myTarget;

                //You are not allowed to switch with yourself.
                if (!current.Equals(target) && target.IsControllable && target.IsCharged) {
                    processes.Add(
                        new Process(
                            target.DisplayName,
                            string.Format("{0} > {1} > {2}\n",
                                current.DisplayName,
                                "SWITCH",
                                target.DisplayName),
                        () => {
                            page.activeCharacter = target;
                            page.currentSelectionNode = page.selectionRoot;
                        })
                        );
                }
            }
            return processes;
        }

        public static void UpdateLastSpellStack(SpellFactory spell, Character caster) {
            if (spell != caster.Attack && !(spell is EquippableItem)) {
                caster.SpellStack.Push(spell);
            }
            if (spell is Item && ((Item)spell).Count <= 1) {
                while (caster.SpellStack.Count > 0 && spell.Equals(caster.SpellStack.Peek())) {
                    caster.SpellStack.Pop();
                }
            }
        }

        /**
         * Since you can switch out the activeCharacter whenever,
         * Characters in the Queue may not neccessarily be charged
         */
        public static Character PopAbledCharacter(Queue<Character> characterQueue) {

            //Remove all !IsCharged() characters from the front of the Queue
            while (characterQueue.Count > 0 && !characterQueue.Peek().IsCharged) {
                characterQueue.Dequeue();
            }

            return characterQueue.Count == 0 ? null : characterQueue.Dequeue();
        }

        public static void DetermineTargetSelection(BattlePage page, SpellFactory spell, Character caster) {
            Util.Assert(spell != null);

            bool isResetSelection = false;
            SpellTargetState sts = SpellTargetState.ONE_TARGET;

            //These TargetTypes might require target selection if there's more than 1 possible target.
            if (spell.TargetType == TargetType.SINGLE_ALLY || spell.TargetType == TargetType.SINGLE_ENEMY || spell.TargetType == TargetType.ANY) {
                switch (spell.TargetType) {
                    case TargetType.SINGLE_ALLY:
                        sts = GetSpellTargetState(spell, caster, page.GetAllies(caster));
                        break;
                    case TargetType.SINGLE_ENEMY:
                        sts = GetSpellTargetState(spell, caster, page.GetEnemies(caster));
                        break;
                    case TargetType.ANY:
                        sts = GetSpellTargetState(spell, caster, page.GetAll(spell.IsSelfTargetable ? null : caster));
                        break;
                }
            } else { //These TargetTypes don't need target selection and thusly can have their results be instantly evaluated.
                switch (spell.TargetType) {
                    case TargetType.SELF:
                        isResetSelection = spell.IsCastable(caster, caster);
                        spell.TryCast(caster, caster);
                        break;
                    case TargetType.ALL_ALLY:
                        isResetSelection = page.GetAllies(caster).Any(c => spell.IsCastable(caster, c));
                        spell.TryCast(caster, page.GetAllies(caster));
                        break;
                    case TargetType.ALL_ENEMY:
                        isResetSelection = page.GetEnemies(caster).Any(c => spell.IsCastable(caster, c));
                        spell.TryCast(caster, page.GetEnemies(caster));
                        break;
                    case TargetType.ALL:
                        isResetSelection = page.GetAll(spell.IsSelfTargetable ? null : caster).Any(c => spell.IsCastable(caster, c));
                        spell.TryCast(caster, page.GetAll());
                        break;
                }
            }

            if (isResetSelection) {
                SpellCastEnd(spell, caster, page);
            } else {
                switch (sts) {
                    case SpellTargetState.ONE_TARGET:
                        switch (spell.TargetType) {
                            case TargetType.SINGLE_ALLY:
                                spell.TryCast(caster, page.GetAllies(caster)[0]);
                                break;
                            case TargetType.SINGLE_ENEMY:
                                spell.TryCast(caster, page.GetEnemies(caster)[0]);
                                break;
                            case TargetType.ANY:
                                spell.TryCast(caster, page.GetAll(caster)[0]);
                                break;
                        }
                        break;
                    case SpellTargetState.MULTIPLE_TARGETS:
                        page.targetedSpell = spell;
                        page.currentSelectionNode = page.currentSelectionNode.FindChild(Selection.TARGET);
                        switch (spell.TargetType) {
                            case TargetType.SINGLE_ALLY:
                                page.targets = page.GetAllies(caster);
                                break;
                            case TargetType.SINGLE_ENEMY:
                                page.targets = page.GetEnemies(caster);
                                break;
                            case TargetType.ANY:
                                page.targets = page.GetAll(spell.IsSelfTargetable ? null : caster);
                                break;
                        }
                        break;
                }
            }
        }

        public enum SpellTargetState {
            ONE_TARGET, MULTIPLE_TARGETS, NO_TARGETS
        }
        public static SpellTargetState GetSpellTargetState(SpellFactory spell, Character caster, IList<Character> targets) {
            IList<Character> targetables = targets.Where(t => t.IsTargetable).ToArray();
            if (spell.IsSingleTargetQuickCastable(caster, targetables) && spell.IsCastable(caster, targetables[0])) {
                return SpellTargetState.ONE_TARGET;
            } else if (spell.IsCastable(caster) && targetables.Count > 1) {
                return SpellTargetState.MULTIPLE_TARGETS;
            } else {
                return SpellTargetState.NO_TARGETS;
            }
        }

        public static void SpellCastEnd(SpellFactory spell, Character caster, BattlePage page) {
            UpdateLastSpellStack(spell, caster);
            page.currentSelectionNode = page.selectionRoot;
        }
    }
}
