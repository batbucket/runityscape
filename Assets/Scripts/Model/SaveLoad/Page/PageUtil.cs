using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Pages {

    public static class PageUtil {
        public static readonly Sprite EQUIPMENT = Util.GetSprite("battle-gear");
        public static readonly Sprite INVENTORY = Util.GetSprite("knapsack");
        public static readonly Sprite SPELLBOOK = Util.GetSprite("spell-book");
        public static readonly Sprite ACTION = Util.GetSprite("talk");

        public static Func<bool> GetVisitProcessCondition(Flags flags, Party party) {
            return () => flags.Time != TimeOfDay.NIGHT && party.Any(c => c.Stats.State == State.ALIVE);
        }

        public static void GetFirstPlaceVisitMessage(PageGroup place) {
            place.Root.AddText(string.Format("{0} was discovered.\nThis place can be visited again in the <color=yellow>Places</color> menu back at camp.", place.Root.Location));
        }

        /// <summary>
        /// Creates a Grid asking if the user wants to perform a task.
        /// </summary>
        /// <param name="current">Page grid is on</param>
        /// <param name="previous">Previous buttonable</param>
        /// <param name="confirm">Process serving as the confirm button</param>
        /// <param name="name">Name of this grid</param>
        /// <param name="tooltip">Tooltip of this grid</param>
        /// <param name="confirmationQuestion">Question to ask when grid is entered.</param>
        /// <returns></returns>
        public static Grid GetConfirmationGrid(Page current, IButtonable previous, Process confirm, string name, string tooltip, string confirmationQuestion) {
            Grid g = new Grid(name);
            g.Tooltip = tooltip;
            g.OnEnter += () => {
                current.AddText(confirmationQuestion);
            };
            g.List.Add(PageUtil.GenerateBack(previous));
            g.List.Add(confirm);
            return g;
        }

        /// <summary>
        /// Gets a confirmation page asking if the user is sure they want to do a particular action.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="previous">The previous.</param>
        /// <param name="confirm">The confirm.</param>
        /// <param name="name">The name.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="confirmationQuestion">The confirmation question.</param>
        /// <returns></returns>
        public static Page GetConfirmationPage(Page current, IButtonable previous, Process confirm, string name, string tooltip, string confirmationQuestion) {
            Page p = new Page(name);
            p.Body = confirmationQuestion;

            p.Actions = new IButtonable[] {
                GenerateBack(previous),
                confirm
            };
            return p;
        }

        /// <summary>
        /// Create a Back Process, which is used
        /// to leave a subgrid.
        /// </summary>
        /// <param name="previous"></param>
        /// <returns></returns>
        public static Process GenerateBack(IButtonable previous) {
            return new Process(
                "Back",
                Util.GetSprite("plain-arrow"),
                string.Format("Go back to {0}.", previous.ButtonText),
                () => previous.Invoke()
                );
        }

        /// <summary>
        /// Gets the out of battle playable handler.
        /// Allows me to use the same Item usage logic in battle
        /// as out of battle, allowing for
        /// CODE REUSE!
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static Action<IPlayable> GetOutOfBattlePlayableHandler(Page page) {
            return (ip) => {
                Main.Instance.StartCoroutine(PerformInOrder(page, ip, page.OnEnter));
            };
        }

        /// <summary>
        /// Create a subgrid that can be left to a main grid.
        /// </summary>
        /// <param name="previous">Content to go back to</param>
        /// <param name="icon">Icon on the Button leading to this submenu</param>
        /// <param name="name">Name on the button leading to this submenu</param>
        /// <param name="tooltip">Description on the button leading to this submenu</param>
        /// <returns></returns>
        private static Grid GenerateBackableGrid(IButtonable previous, Sprite icon, string name, string tooltip) {
            Grid grid = new Grid(name);
            grid.Icon = icon;
            grid.Tooltip = tooltip;
            List<IButtonable> buttons = new List<IButtonable>();
            buttons.Add(GenerateBack(previous));
            grid.List = buttons;
            return grid;
        }

        /// <summary>
        /// Generates a grid with inventory items
        /// to use
        /// </summary>
        /// <param name="current">Page the owner is on</param>
        /// <param name="previous">Main grid</param>
        /// <param name="owner">Owner of the inventory</param>
        /// <param name="addPlay">IPlayable handler</param>
        /// <param name="isInCombat">If in combat, X will use Y on Z, if out of battle, Z would use Y on themself.</param>
        /// <returns></returns>
        public static Grid GenerateItemsGrid(
                    bool isInCombat,
                    Page current,
                    IButtonable previous,
                    Character owner,
                    Action<IPlayable> addPlay
                    ) {
            return GenerateSpellableGrid(
                !isInCombat,
                current,
                previous,
                owner,
                null,
                owner.Inventory,
                addPlay,
                INVENTORY,
                string.Format("Items ({0}/{1})", owner.Inventory.TotalOccupiedSpace, owner.Inventory.Capacity),
                string.Format("Use an item.\n{0} out of {1} inventory space is occupied.", owner.Inventory.TotalOccupiedSpace, owner.Inventory.Capacity)
                );
        }

        /// <summary>
        /// Generate a Grid containing all SpellBooks
        /// in Characters.SpellBooks
        /// </summary>
        /// <param name="current">Page the Owner is on</param>
        /// <param name="previous">Previous IButtonable you want this Grid to go back to</param>
        /// <param name="owner">Owner of the SpellBooks</param>
        /// <param name="excluded">SpellBook to exclude, typically 'Attack'</param>
        /// <param name="spellCollection">SpellBooks owned by Owner</param>
        /// <param name="addPlay">Chosen action handler</param>
        /// <returns></returns>
        public static Grid GenerateSpellBooks(
            Page current,
            IButtonable previous,
            Character owner,
            SpellBook excluded,
            Action<IPlayable> addPlay
            ) {
            return GenerateSpellableGrid(
                false,
                current,
                previous,
                owner,
                excluded,
                ((IEnumerable<ISpellable>)owner.Spells).Where(s => s.GetSpellBook().Costs.Count > 0),
                addPlay,
                SPELLBOOK,
                "Spells",
                "Cast a spell.");
        }

        /// <summary>
        /// Actions are spells without a cost.
        /// </summary>
        /// <param name="current">The current page.</param>
        /// <param name="previous">The previous page.</param>
        /// <param name="owner">The owner of the action.</param>
        /// <param name="excluded">The excluded spell.</param>
        /// <param name="concat">The spells to add alongside the spellbook's.</param>
        /// <param name="playHandler">The play handler.</param>
        /// <returns></returns>
        public static Grid GenerateActions(
            Page current,
            IButtonable previous,
            Character owner,
            SpellBook excluded,
            IEnumerable<ISpellable> concat,
            Action<IPlayable> playHandler
            ) {
            List<SpellBook> spellsThatHaveACost = new List<SpellBook>();

            return GenerateSpellableGrid(
                false,
                current,
                previous,
                owner,
                excluded,
                ((IEnumerable<ISpellable>)owner.Spells).Where(s => s.GetSpellBook().Costs.Count <= 0).ToList().Concat(concat),
                playHandler,
                ACTION,
                "Action",
                "Perform an action.");
        }

        /// <summary>
        /// Creates a subGrid containing all possible targets
        /// of spellable
        /// </summary>
        /// <param name="current">Page that owner is on</param>
        /// <param name="previous">supergrid containing a list of possible ISpellables to use</param>
        /// <param name="owner">User of the Spellable</param>
        /// <param name="spellable">Spellable to use on target</param>
        /// <param name="handlePlayable">Playable handler</param>
        /// <returns></returns>
        public static Grid GenerateTargets(Page current, IButtonable previous, Character owner, ISpellable spellable, Action<IPlayable> handlePlayable, bool isTargetingSelf) {
            return GenerateTargets(current, previous, owner, spellable, spellable.GetSpellBook().Icon, handlePlayable, isTargetingSelf);
        }

        /// <summary>
        /// Creates a subGrid containing all possible targets
        /// of spellable
        /// </summary>
        /// <param name="current">Page that owner is on</param>
        /// <param name="previous">supergrid containing a list of possible ISpellables to use</param>
        /// <param name="owner">User of the Spellable</param>
        /// <param name="spellable">Spellable to use on target</param>
        /// <param name="handlePlayable">Playable handler</param>
        /// <param name="isTargetingSelf">If true, the target will attempt to cast the spell on themselves (used for out of combat inventories)</param>
        /// <returns></returns>
        public static Grid GenerateTargets(Page current, IButtonable previous, Character owner, ISpellable spellable, Sprite sprite, Action<IPlayable> handlePlayable, bool isTargetingSelf) {
            SpellBook sb = spellable.GetSpellBook();
            ICollection<Character> targets = sb.TargetType.GetTargets(owner, current);
            Grid grid = GenerateBackableGrid(previous, sb.Icon, sb.Name, sb.CreateDescription(owner));

            grid.Icon = sprite;

            foreach (Character myTarget in targets) {
                Character target = myTarget;

                Character spellOwner = null;
                if (isTargetingSelf) {
                    spellOwner = target;
                } else {
                    spellOwner = owner;
                }
                grid.List.Add(GenerateTargetProcess(current, previous, spellOwner, target, sb, handlePlayable));
            }
            Item item = spellable as Item;
            if (item != null && item.HasFlag(Items.Flag.OCCUPIES_SPACE)) {
                grid.List.Add(
                    new Process(
                        string.Format("Drop"),
                        string.Format("Throw away {0}.", item.Name),
                        () => {
                            handlePlayable(
                                owner.Spells.CreateSpell(current, new TossItem(item, owner.Inventory), owner, owner)
                                );
                        }
                        ));
            }
            return grid;
        }

        /// <summary>
        /// Creates a subgrid containing user's equipment
        /// </summary>
        /// <param name="previous">supergrid</param>
        /// <param name="owner">Owner of the equipment</param>
        /// <param name="handlePlayable">Playable handler</param>
        /// <param name="isInCombat">If in combat, text is "Equipment", otherwise it's the owner's name</param>
        /// <returns></returns>
        public static Grid GenerateEquipmentGrid(Page current, IButtonable previous, Character owner, Action<IPlayable> handlePlayable, bool isInCombat) {
            Grid grid = GenerateBackableGrid(
                previous,
                isInCombat ? EQUIPMENT : owner.Look.Sprite,
                isInCombat ? "Equipment" : owner.Look.DisplayName,
                string.Format("Manage {0}'s equipment.", owner.Look.DisplayName));

            foreach (EquipType myET in EquipType.AllTypes) {
                EquipType et = myET;
                IButtonable ib = null;
                Equipment eq = owner.Equipment;
                if (owner.Equipment.Contains(et)) {
                    CastUnequipItem unequip = new CastUnequipItem(owner.Inventory, owner.Equipment, eq.PeekItem(et));
                    ib = GetUnequipProcess(current, unequip, owner, grid, handlePlayable);
                } else {
                    ib = GetEquipGrid(current, owner, et, owner.Inventory, grid, handlePlayable);
                }
                grid.List.Add(ib);
            }
            return grid;
        }

        public static Grid GenerateGroupEquipmentGrid(IButtonable previous, Page current, ICollection<Character> party, Action<IPlayable> handlePlayable, bool isInCombat) {
            Grid grid = new Grid("Equipment");
            grid.Icon = EQUIPMENT;
            grid.List.Add(GenerateBack(previous));

            foreach (Character partyMember in party) {
                grid.List.Add(GenerateEquipmentGrid(current, grid, partyMember, handlePlayable, isInCombat));
            }

            return grid;
        }

        private static string CreateDetailedTargetName(Character owner, Character target, SpellBook sb) {
            return Util.ColorString(target.Look.DisplayName, sb.IsCastable(owner, target));
        }

        private static Grid GenerateSpellableGrid(
                    bool isTargetingSelf,
                    Page current,
                    IButtonable previous,
                    Character owner,
                    SpellBook excluded,
                    IEnumerable<ISpellable> spellCollection,
                    Action<IPlayable> addPlay,
                    Sprite sprite,
                    string name,
                    string description) {
            Grid grid = GenerateBackableGrid(previous, sprite, name, description);

            foreach (ISpellable myS in spellCollection) {
                ISpellable s = myS;
                if (!s.Equals(excluded)) {
                    grid.List.Add(GenerateSpellProcess(current, grid, owner, s, addPlay, isTargetingSelf));
                }
            }

            return grid;
        }

        private static Process GenerateSpellProcess(Page current, IButtonable previous, Character owner, ISpellable spellable, Action<IPlayable> handlePlayable, bool isTargetingSelf) {
            SpellBook sb = spellable.GetSpellBook();
            return new Process(sb.GetDetailedName(owner), sb.Icon, sb.CreateDescription(owner),
                () => {
                    if (sb.IsMeetPreTargetRequirements(owner.Stats)) {
                        GenerateTargets(current, previous, owner, spellable, handlePlayable, isTargetingSelf).Invoke();
                    }
                });
        }

        private static Process GenerateTargetProcess(Page current, IButtonable previous, Character owner, Character target, SpellBook sb, Action<IPlayable> handlePlayable) {
            return GenerateTargetProcessHelper(current, previous, owner, target, sb, handlePlayable, CreateDetailedTargetName(owner, target, sb), target.Look.Sprite);
        }

        private static Process GenerateTargetProcessHelper(
            Page current,
            IButtonable previous,
            Character owner,
            Character target,
            SpellBook sb,
            Action<IPlayable> handlePlayable,
            string name,
            Sprite icon) {
            return new Process(name,
                               icon,
                                sb.CreateTargetDescription(owner, target),
                                () => {
                                    if (sb.IsCastable(owner, target)) {
                                        handlePlayable(owner.Spells.CreateSpell(current, sb, owner, target));
                                        previous.Invoke();
                                    }
                                });
        }

        private static Grid GetEquipGrid(Page current, Character owner, EquipType et, Inventory inv, IButtonable previous, Action<IPlayable> handlePlayable) {
            Grid grid = GenerateBackableGrid(
                previous,
                et.Sprite,
                Util.ColorString(et.Name, Color.grey),
                string.Format("Equip an item in the <color=yellow>{0}</color> slot.", et.Name)
                );
            foreach (EquippableItem ei in inv as IEnumerable<EquippableItem>) {
                if (ei.Type.Equals(et)) {
                    grid.List.Add(GenerateTargetProcessHelper(current, previous, owner, owner, new CastEquipItem(ei), handlePlayable, ei.Name, ei.Icon));
                }
            }
            return grid;
        }

        private static Process GetUnequipProcess(Page current, CastUnequipItem unequipSpell, Character owner, IButtonable previous, Action<IPlayable> handlePlayable) {
            return new Process(unequipSpell.Name, unequipSpell.Icon, unequipSpell.CreateDescription(owner),
                        () => {
                            handlePlayable(owner.Spells.CreateSpell(current, unequipSpell, owner, owner));
                            previous.Invoke();
                        },
                        () => unequipSpell.IsCastable(owner, owner)
                        );
        }

        /// <summary>
        /// There's a slight delay to Play()'s effects occurring, so we wait for
        /// Play() to be completely finished, and then perform our action.
        ///
        /// If we don't do this, then item counts won't be updated since it'll go in this order:
        /// Play coroutine started
        /// Item buttons get setup
        /// Used item is decremented in count by 1
        /// </summary>
        /// <param name="playgroup">PlayGroup</param>
        /// <param name="postAction">Action to perform after PlayGroup is finished.</param>
        /// <returns></returns>
        private static IEnumerator PerformInOrder(Page page, IPlayable play, Action postAction) {
            yield return play.Play();
            page.AddText(play.Text);
            postAction.Invoke();
        }
    }
}