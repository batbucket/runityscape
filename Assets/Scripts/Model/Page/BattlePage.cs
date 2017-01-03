using Scripts.Model.Characters;
using Scripts.Model.World.Pages;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Resources;
using Scripts.Model.TextBoxes;
using Scripts.View.ActionGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts.Presenter;

namespace Scripts.Model.Pages {

    /// <summary>
    /// This class represents a battle scene in the game.
    /// Where two teams of Characters can battle each other.
    /// </summary>
    public class BattlePage : Page {
        public const int ACT_INDEX = 5;

        public const int ATTACK_INDEX = 0;

        /// <summary>
        /// These are the various indicies where each button from the "Root" selection
        /// will be placed.
        /// </summary>
        public const int BACK_INDEX = ActionGridView.ROWS * ActionGridView.COLS - 1;

        public const int EQUIP_INDEX = 10;

        public const int ITEM_INDEX = 6;

        public const int LAST_SPELL_INDEX = 1;

        public const int MERCY_INDEX = 7;

        public const int SPELL_INDEX = 4;

        public const int SWITCH_INDEX = 11;

        /// <summary>
        /// The page the user will be able to go to if they lose the battle.
        /// </summary>
        public Page Defeat;

        /// <summary>
        /// A function that returns true when the user's party has been defeated.
        /// </summary>
        public Func<bool> IsDefeat;

        /// <summary>
        /// A function that returns true when the user's party is victorious.
        /// </summary>
        public Func<bool> IsVictory;

        /// <summary>
        /// The current State of battle.
        /// Example: After victory
        /// </summary>
        public BattleState State;

        /// <summary>
        /// The page the user will be able to go to if they are victorious in the battle.
        /// </summary>
        public Page Victory;

        /// <summary>
        /// The active Character is the Character who is currently under control
        /// by the user. This is the Character whose abilities are visible
        /// on the ActionGrid.
        /// </summary>
        private Character activeCharacter;

        /// <summary>
        /// Queue for the Character who will be the active character.
        /// </summary>
        private Queue<Character> activeCharacterQueue;

        /// <summary>
        /// The current Selection node. These are the leaves of the tree.
        /// If your selection node was "Spell", you would see buttons with your
        /// castable spells.
        /// If your selection node was "Root", you would see buttons such as
        /// Attack, Spell, Action, Item, Flee, and Switch.
        /// </summary>
        private TreeNode<Selection> currentSelectionNode;

        /// <summary>
        /// Reference to the Page where the user will be able to
        /// loot items.
        /// </summary>
        private LootPage loot;

        /// <summary>
        /// The main character of the battle.
        /// We need this to calculate everyone's Charge cap.
        /// </summary>
        private Character mainCharacter;

        /// <summary>
        /// The root of the Tree is the group of generic commands that open up
        /// more specific commands.
        /// In this case it's the ActionGrid with
        /// Attack, Spell, Action, Item, Flee, and Switch.
        /// </summary>
        private TreeNode<Selection> selectionRoot;

        /// <summary>
        /// Reference to a SpellFactory that requires its target to be
        /// selected.
        /// </summary>
        private SpellFactory targetedSpell;

        /// <summary>
        /// Reference to either the left, right, or both Character list(s) in this Page
        /// to be used for target selection.
        /// </summary>
        private IList<Character> targets;

        /// <summary>
        /// Constructs a battle page.
        /// </summary>
        /// <param name="text">Text in the textbox that will appear on enter.</param>
        /// <param name="location">In game location.</param>
        /// <param name="mainCharacter">Main character.</param>
        /// <param name="left">Characters on the left side.</param>
        /// <param name="right">Characters on the right side.</param>
        /// <param name="onFirstEnter">Action invoked when the user first enters this page.</param>
        /// <param name="onEnter">Action invoked when the user enters this page.</param>
        /// <param name="onFirstExit">Action invoked when the user first exits this page.</param>
        /// <param name="onExit">Action invoked when the user first exits this page.</param>
        /// <param name="onTick">Action invoked every tick.</param>
        /// <param name="musicLoc">Name of the music file to be played during battle.</param>
        /// <param name="isVictory">Function returning true when the user's side is victorious.</param>
        /// <param name="victory">Page user can go to on victory.</param>
        /// <param name="isDefeat">Function returning true when the user's side is defeated.</param>
        /// <param name="defeat">Page user can go to on defeat.</param>
        public BattlePage(
            string text = "",
            string location = "",
            Character mainCharacter = null,
            IList<Character> left = null,
            IList<Character> right = null,
            Action onFirstEnter = null,
            Action onEnter = null,
            Action onFirstExit = null,
            Action onExit = null,
            Action onTick = null,
            string musicLoc = null,
            Func<bool> isVictory = null,
            Page victory = null,
            Func<bool> isDefeat = null,
            Page defeat = null
            )
            : base("", "", location, false, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, musicLoc: musicLoc) {
            this.mainCharacter = mainCharacter;
            activeCharacterQueue = new Queue<Character>();
            this.OnFirstEnterAction += () => {
                Game.Instance.TextBoxes.AddTextBox(new TextBox(text));
            };

            /**
             * Set up the Selection Tree.
             * Tree looks like this:
             *               FAIM   -> TARGET (quickcast)
             * SPELLS    ACTS   ITEMS    MERCIES  EQUIPS  SWITCH
             * TARGET   TARGET  TARGET   TARGET           TARGET
             */
            selectionRoot = new TreeNode<Selection>(Selection.ROOT); //Topmost node
            selectionRoot.AddChildren(Selection.SPELL, Selection.ACT, Selection.ITEM, Selection.FLEE, Selection.EQUIP, Selection.SWITCH, Selection.TARGET);
            foreach (TreeNode<Selection> st in selectionRoot.Children) {
                st.AddChild(Selection.TARGET);
            }
            currentSelectionNode = selectionRoot;

            this.IsVictory = isVictory ?? (() => GetEnemies(mainCharacter).All(c => c.State == CharacterState.KILLED));
            this.Victory = victory;

            this.IsDefeat = isDefeat ?? (() => GetAllies(mainCharacter).All(c => c.State == CharacterState.DEFEAT || c.State == CharacterState.KILLED));
            this.Defeat = defeat;
            this.State = BattleState.PRE_BATTLE;
        }

        /// <summary>
        /// Special battle page constructor meant to be called in an actual game.
        /// </summary>
        /// <param name="party">User's party</param>
        /// <param name="victory">Page to go to on victory.</param>
        /// <param name="defeat">Page to go to on defeat.</param>
        /// <param name="musicLoc">Location of music to be played.</param>
        /// <param name="location">In game location.</param>
        /// <param name="text">"Text in the first textbox that appears when entering."</param>
        /// <param name="enemies">Enemies to be battled in this page.</param>
        public BattlePage(
        Party party,
        Page victory,
        Page defeat,
        string musicLoc,
        string location,
        string text,
        params Character[] enemies) : this(text, location, party.Leader, party.Members, enemies, null, null, null, null, null, musicLoc, null, victory, null, defeat) {
        }

        /// <summary>
        /// ONE_TARGET: There is one possible target for the spell
        /// MULTIPLE_TARGETS: There are more than one possible targets for the spell
        /// NO_TARGETS: There are no targets for the spell
        /// </summary>
        private enum SpellTargetState {
            ONE_TARGET, MULTIPLE_TARGETS, NO_TARGETS
        }

        protected override void OnAddCharacter(Character c) {
            base.OnAddCharacter(c);
            c.OnBattleStart();
            c.Flees.Add(new Flee(Defeat));
        }

        protected override void OnAnyEnter() {
            base.OnAnyEnter();
            GetAll().ForEach(c => {
                c.OnBattleStart();
                c.Flees.Add(new Flee(Defeat));
            });
        }

        protected override void OnAnyExit() {
            GetAll().ForEach(c => {
                c.OnBattleStart();
                c.Flees.Remove(new Flee(Defeat));
            });
        }

        protected override void OnTick() {
            // Check the battle state
            if (State == BattleState.BATTLE && IsVictory.Invoke()) {
                State = BattleState.VICTORY;
            } else if (State == BattleState.BATTLE && IsDefeat.Invoke()) {
                State = BattleState.DEFEAT;
            }

            // If the battle has ended either way
            if (State == BattleState.DEFEAT || State == BattleState.VICTORY) {
                this.Tooltip = "";
                foreach (Character c in GetAll()) {
                    c.OnBattleEnd();
                }
            }

            switch (State) {
                // Before the battle begins, users will be able to denote when to begin it
                case BattleState.PRE_BATTLE:
                    ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Engage", "", () => State = BattleState.BATTLE);
                    break;

                case BattleState.BATTLE:
                    /**
                     * Get all targetable characters,
                     * Add them to the active character queue if they can be controlled by the user,
                     * Make their charge bar visible,
                     * Call their battletick method.
                     */
                    IList<Character> all = GetAll().Where(c => c.IsTargetable).ToArray();
                    foreach (Character c in all) {
                        if (c.IsCommandable) { activeCharacterQueue.Enqueue(c); }
                        if (c.HasResource(ResourceType.CHARGE)) { c.Resources[ResourceType.CHARGE].IsVisible = true; }
                        c.BattleTick(mainCharacter);
                    }

                    /**
                     * Remove activeCharacter if they are not charged,
                     * Indicating that they've casted a spell or had their charge removed some other way
                     */
                    if (activeCharacter == null || (activeCharacter != null && (!activeCharacter.IsCharged || activeCharacter.State == CharacterState.DEFEAT || activeCharacter.State == CharacterState.KILLED))) {
                        activeCharacter = PopAbledCharacter(activeCharacterQueue);
                        currentSelectionNode = selectionRoot;
                    }

                    //No one can move so clear the board
                    if (activeCharacter == null) {
                        Tooltip = "";
                        ClearActionGrid();
                    }

                    // If we have an active character available, display their commands
                    if (activeCharacter != null) {
                        Display(activeCharacter);
                    }
                    break;

                case BattleState.VICTORY:
                    IList<Character> allies = GetAllies(mainCharacter);

                    // Sum up all the experience given by the killed enemies to add to each player in the party
                    int expSum = GetEnemies(mainCharacter).Sum(c => c.GetResourceCount(ResourceType.DEATH_EXP, false));
                    foreach (Character c in allies) {
                        c.OnVictory();

                        // Only show the XP textboxes if experience was actually gained
                        if (expSum > 0 && c.HasResource(ResourceType.EXPERIENCE)) {
                            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} gained {1} experience.", c.DisplayName, expSum)));
                            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} has enough experience to level up.", c.DisplayName)));
                            c.AddToResource(ResourceType.EXPERIENCE, false, expSum);
                        }
                    }

                    // Sum up all the gold by the killed enemies to put in the shared inventory
                    int goldSum = GetEnemies(mainCharacter).Sum(c => c.Inventory.Gold);
                    mainCharacter.Inventory.Gold += goldSum;

                    // Only show the gold textbox if gold was actually gained
                    if (goldSum > 0) {
                        Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("<color=yellow>{0}</color> gold was added to the inventory.", goldSum)));
                    }

                    Game.Instance.TextBoxes.AddTextBox(new TextBox("Victory!"));

                    Game.Instance.Sound.StopAllSounds();
                    loot = new LootPage(this, new Party(mainCharacter, GetAllies(mainCharacter)), GetEnemies(mainCharacter));
                    State = BattleState.POST_VICTORY;
                    break;

                case BattleState.DEFEAT:
                    IList<Character> enemies = GetEnemies(mainCharacter);
                    foreach (Character c in enemies) {
                        c.OnVictory();
                    }
                    Game.Instance.TextBoxes.AddTextBox(new TextBox("Defeat!"));
                    Game.Instance.Sound.StopAllSounds();

                    State = BattleState.POST_DEFEAT;
                    break;

                case BattleState.POST_VICTORY:
                    ActionGrid = new IButtonable[0];
                    if (loot.HasLoot) {
                        ActionGrid[0] = loot;
                    }
                    ActionGrid[1] = new ItemManagePage(this, new Party(mainCharacter, GetAllies(mainCharacter)));
                    ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Continue", "", () => Game.Instance.CurrentPage = Victory);
                    Game.Instance.Sound.StopAllSounds();
                    break;

                case BattleState.POST_DEFEAT:
                    ActionGrid = new IButtonable[0];
                    ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Continue", "", () => Game.Instance.CurrentPage = Defeat);
                    Game.Instance.Sound.StopAllSounds();
                    break;
            }
        }

        /// <summary>
        /// Create a process that allows the user to return to the above selection node
        /// </summary>
        /// <param name="page">This page</param>
        /// <param name="current">Currently active character</param>
        /// <param name="parentSelectionName">Name of the parent node to go to.</param>
        /// <returns>A process that allows the user to return to the above selection node</returns>
        private static Process CreateBackButton(BattlePage page, Character current, string parentSelectionName) {
            return new Process("Back", string.Format("{0} > {1}\n", current.DisplayName, parentSelectionName),
                () => {
                    page.currentSelectionNode = page.currentSelectionNode.Parent;
                });
        }

        /// <summary>
        /// Show worn equipment first that can be unequipped, then Items in inventory that can be equipped
        /// </summary>
        /// <param name="page">This page</param>
        /// <param name="current">Character whose equipment we're viewing</param>
        /// <returns></returns>
        private static IList<Process> CreateEquipmentProcesses(BattlePage page, Character current) {
            IList<Process> processes = new List<Process>();
            foreach (EquippableItem myEquip in current.Equipment) {
                EquippableItem equip = myEquip;
                if (equip != null) {
                    processes.Add(CreateUnequipProcess(current, equip, current.Equipment));
                }
            }
            foreach (Item i in current.Inventory) {
                if (i is EquippableItem) {
                    EquippableItem e = (EquippableItem)i;
                    processes.Add(CreateSpellProcess(page, e, current));
                }
            }
            return processes;
        }

        /// <summary>
        /// Create an IList of Processes that have a specific
        /// Character casting a spell as the action
        /// </summary>
        /// <typeparam name="T">Derived classes of Spell.</typeparam>
        /// <param name="page">This page</param>
        /// <param name="spells">Spells for the caster to cast</param>
        /// <param name="caster">Caster of these spells</param>
        /// <returns></returns>
        private static IList<Process> CreateSpellList<T>(BattlePage page, ICollection<T> spells, Character caster) where T : SpellFactory {
            IList<Process> processes = new List<Process>();
            foreach (SpellFactory mySpell in spells) {
                SpellFactory spell = mySpell;
                processes.Add(CreateSpellProcess(page, spell, caster));
            }
            return processes;
        }

        /// <summary>
        /// Creates a Process that brings up the "Who to target with this Spell" display if needed
        /// </summary>
        /// <param name="page">This page</param>
        /// <param name="spell">SpellFactory to check for targetability</param>
        /// <param name="caster">The caster of the spell</param>
        /// <returns></returns>
        private static Process CreateSpellProcess(BattlePage page, SpellFactory spell, Character caster) {
            return new Process(spell.GetNameAndCosts(caster), string.Format("{0} > {1}  {2}\n{3}", caster.DisplayName, spell.Name, spell.GetCosts(), spell.Description),
                () => {
                    DetermineTargetSelection(page, spell, caster);
                });
        }

        /// <summary>
        /// Create a process that allows the user to change control to another character who can be active
        /// </summary>
        /// <param name="current">Current active character</param>
        /// <param name="page">This page</param>
        /// <returns>A process that allows the user to change control to another character who can be active</returns>
        private static Process CreateSwitchButton(Character current, BattlePage page) {
            return new Process(Selection.SWITCH.Name, string.Format(Selection.SWITCH.Text, current.DisplayName),
                () => {
                    //Quick switch if there's only one other abledCharacter to switch with
                    IList<Character> abledCharacters = page.GetAll().FindAll(c => !current.Equals(c) && c.IsCommandable);
                    if (abledCharacters.Count == 1) {
                        page.activeCharacter = abledCharacters[0];
                        page.currentSelectionNode = page.selectionRoot;
                    } else {
                        page.currentSelectionNode = page.currentSelectionNode.FindChild(Selection.SWITCH);
                    }
                });
        }

        /// <summary>
        /// Create a list of buttons that allow the current active
        /// character to be switched with another activeable character.
        /// </summary>
        /// <param name="current">Current active character</param>
        /// <param name="page">This page</param>
        /// <returns>A list of buttons that allow the current active
        /// character to be switched with another activeable character.</returns>
        private static IList<Process> CreateSwitchMenu(Character current, BattlePage page) {
            IList<Process> processes = new List<Process>();
            foreach (Character myTarget in page.GetAll()) {
                Character target = myTarget;

                // You are not allowed to switch with yourself.
                if (!current.Equals(target) && target.IsCommandable) {
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

        /// <summary>
        /// Create a list of processes that involve a caster hitting a target
        /// with a spell.
        /// </summary>
        /// <param name="page">This page</param>
        /// <param name="caster">Caster of the spell</param>
        /// <param name="spell">Spell to be cast</param>
        /// <param name="targets">Recipients of the spell</param>
        /// <returns>A list of processes that involve a caster hitting a target</returns>
        private static IList<Process> CreateTargetList(BattlePage page, Character caster, SpellFactory spell, IList<Character> targets) {
            IList<Process> processes = new List<Process>();
            foreach (Character myTarget in targets) {
                Character target = myTarget;
                processes.Add(CreateTargetProcess(page, spell, caster, target));
            }
            return processes;
        }

        /// <summary>
        /// Creates a process that targets a specific Character with a spell
        /// </summary>
        /// <param name="page">This page</param>
        /// <param name="spell">Spell to be targeted on someone</param>
        /// <param name="caster">Caster of spell</param>
        /// <param name="target">Recipient of th espell</param>
        /// <returns></returns>
        private static Process CreateTargetProcess(BattlePage page, SpellFactory spell, Character caster, Character target) {
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

        /// <summary>
        /// Create a process that allows a Character to remove an equippableitem from their inventory
        /// </summary>
        /// <param name="caster">Character who we're removing the item from</param>
        /// <param name="item">EquippableItem to be removed from their equipment</param>
        /// <param name="equipment">Equipment from which the item will be removed from.</param>
        /// <returns></returns>
        private static Process CreateUnequipProcess(Character caster, EquippableItem item, ICollection<EquippableItem> equipment) {
            return new Process(
                Util.Color(string.Format("{0}{1}", item.Name, "(E)"), Color.yellow),
                string.Format("{0} > {1} > {2}\n{3}", caster.DisplayName, "UNEQUIP", item.Name, item.Description), () => {
                    Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.", caster.DisplayName, item.Name)));
                    item.UnequipItemInSlot(caster);
                });
        }

        /// <summary>
        /// Used to determine if a spell can be instantly cast,
        /// or if it requires the user to specify a target.
        /// </summary>
        /// <param name="page">This page</param>
        /// <param name="spell">Spell to check</param>
        /// <param name="caster">Caster of the spell</param>
        private static void DetermineTargetSelection(BattlePage page, SpellFactory spell, Character caster) {
            Util.Assert(spell != null);

            bool isResetSelection = false;
            SpellTargetState sts = SpellTargetState.ONE_TARGET;
            IList<Character> targetables = null;

            // These TargetTypes might require target selection if there's more than 1 possible target.
            if (spell.TargetType == TargetType.SINGLE_ALLY || spell.TargetType == TargetType.SINGLE_ENEMY || spell.TargetType == TargetType.ANY) {
                switch (spell.TargetType) {
                    case TargetType.SINGLE_ALLY:
                        targetables = page.GetAllies(caster).Where(t => t.IsTargetable).ToArray();
                        break;

                    case TargetType.SINGLE_ENEMY:
                        targetables = page.GetEnemies(caster).Where(t => t.IsTargetable).ToArray();
                        break;

                    case TargetType.ANY:
                        targetables = page.GetAll(spell.IsSelfTargetable ? null : caster).Where(t => t.IsTargetable).ToArray();
                        break;
                }
                sts = GetSpellTargetState(spell, caster, targetables);
            } else {
                //These TargetTypes don't need target selection and thusly can have their results be instantly evaluated.
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

            // Reset the grid if the spell was cast.
            if (isResetSelection) {
                SpellCastEnd(spell, caster, page);
            } else if (targetables != null && targetables.Count > 0) { // We have targetable enemies
                switch (sts) {
                    // One enemy case, just target them automatically
                    case SpellTargetState.ONE_TARGET:
                        spell.TryCast(caster, targetables[0]);
                        break;

                    // Multiple targets case, go to the target selection grid
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

                    // No targets available case, this shouldn't happen in most cases, but if it does, nothing happens
                    case SpellTargetState.NO_TARGETS:
                        Util.Log("There's no targets?");
                        break;
                }
            }
        }

        /// <summary>
        /// Determine if there is either one target or multiple targets for the spell.
        /// </summary>
        /// <param name="spell">Spell to check</param>
        /// <param name="caster">Caster of the spell</param>
        /// <param name="targetables">Targets to check with the spell and caster</param>
        /// <returns>Enum denoting the number of targets for the spell.</returns>
        private static SpellTargetState GetSpellTargetState(SpellFactory spell, Character caster, IList<Character> targetables) {
            if (spell.IsSingleTargetQuickCastable(caster, targetables) && spell.IsCastable(caster, targetables[0])) {
                return SpellTargetState.ONE_TARGET;
            } else if (spell.IsCastable(caster) && targetables.Count > 1) {
                return SpellTargetState.MULTIPLE_TARGETS;
            } else {
                return SpellTargetState.NO_TARGETS;
            }
        }

        /// <summary>
        /// Since you can switch the active character,
        /// Characters popped from the characterqueue for control may not always
        /// be charged.
        /// </summary>
        /// <param name="characterQueue">Characterqueue containing characters to be controlled</param>
        /// <returns>A character from the queue who is charged.</returns>
        private static Character PopAbledCharacter(Queue<Character> characterQueue) {
            // Remove all !IsCharged() characters from the front of the Queue
            while (characterQueue.Count > 0 && !characterQueue.Peek().IsCharged) {
                characterQueue.Dequeue();
            }

            return characterQueue.Count == 0 ? null : characterQueue.Dequeue();
        }

        /// <summary>
        /// Resets the node and updates the stack
        /// after a spell is successfully cast.
        /// </summary>
        /// <param name="spell">Spell that was cast</param>
        /// <param name="caster">Caster of the spell</param>
        /// <param name="page">This page</param>
        private static void SpellCastEnd(SpellFactory spell, Character caster, BattlePage page) {
            UpdateLastSpellStack(spell, caster);
            page.currentSelectionNode = page.selectionRoot;
        }

        /// <summary>
        /// Updates the stack used to keep track of the last spell
        /// cast by the active character, which we allow recasting from
        /// the root selection.
        /// </summary>
        /// <param name="spell">Spell that was cast</param>
        /// <param name="caster">Caster of the spell</param>
        private static void UpdateLastSpellStack(SpellFactory spell, Character caster) {
            if (spell != caster.Attack && !(spell is EquippableItem)) {
                caster.SpellStack.Push(spell);
            }

            if (spell is Item) {
                while (caster.SpellStack.Count > 0 && spell.Equals(caster.SpellStack.Peek())) {
                    caster.SpellStack.Pop();
                }
            }
        }

        /// <summary>
        /// Display function used to show the commands
        /// of a character
        /// </summary>
        /// <param name="character">Character to have their commands displayed</param>
        private void Display(Character character) {
            ClearActionGrid();
            Tooltip = string.Format("{0} > {1}\n", character.DisplayName, currentSelectionNode.Value.Text);
            switch (currentSelectionNode.Value.SelectionType) {
                case Selection.Type.ROOT:
                    ActionGrid[SWITCH_INDEX] = CreateSwitchButton(character, this);
                    ActionGrid[ATTACK_INDEX] = character.Attack == null ? new Process() : CreateSpellProcess(this, character.Attack, character);
                    ActionGrid[LAST_SPELL_INDEX] = character.SpellStack.Count == 0 ? null : CreateSpellProcess(this, character.SpellStack.Peek(), character);

                    ActionGrid[SPELL_INDEX] = new Process(Selection.SPELL.Name, string.Format("{0} > {1}\n", character.DisplayName, Selection.SPELL.Text),
                        () => {
                            currentSelectionNode = currentSelectionNode.FindChild(Selection.SPELL);
                        }
                        );
                    ActionGrid[ACT_INDEX] = new Process(Selection.ACT.Name, string.Format("{0} > {1}\n", character.DisplayName, Selection.ACT.Text),
                        () => {
                            currentSelectionNode = currentSelectionNode.FindChild(Selection.ACT);
                        }
                        );
                    ActionGrid[ITEM_INDEX] = new Process(Selection.ITEM.Name, string.Format("{0} > {1}\n", character.DisplayName, Selection.ITEM.Text),
                        () => {
                            currentSelectionNode = currentSelectionNode.FindChild(Selection.ITEM);
                        }
                        );
                    ActionGrid[MERCY_INDEX] = new Process(Selection.FLEE.Name, string.Format("{0} > {1}\n", character.DisplayName, Selection.FLEE.Text),
                        () => {
                            currentSelectionNode = currentSelectionNode.FindChild(Selection.FLEE);
                        }
                        );

                    ActionGrid[EQUIP_INDEX] = new Process(Selection.EQUIP.Name, string.Format("{0} > {1}\n", character.DisplayName, Selection.EQUIP.Text),
                        () => {
                            currentSelectionNode = currentSelectionNode.FindChild(Selection.EQUIP);
                        }
                        );
                    ActionGrid[SWITCH_INDEX] = new Process(Selection.SWITCH.Name, string.Format("{0} > {1}\n", character.DisplayName, Selection.SWITCH.Text),
                        () => {
                            currentSelectionNode = currentSelectionNode.FindChild(Selection.SWITCH);
                        }
                        );
                    break;

                case Selection.Type.SPELL:
                    ActionGrid = CreateSpellList(this, character.Spells, character).ToArray();
                    ActionGrid[BACK_INDEX] = CreateBackButton(this, character, "BACK");
                    break;

                case Selection.Type.ACTION:
                    ActionGrid = CreateSpellList(this, character.Actions, character).ToArray();
                    ActionGrid[BACK_INDEX] = CreateBackButton(this, character, "BACK");
                    break;

                case Selection.Type.ITEM:
                    ActionGrid = CreateSpellList(this, character.Inventory, character).ToArray();
                    ActionGrid[BACK_INDEX] = CreateBackButton(this, character, "BACK");
                    break;

                case Selection.Type.FLEE:
                    ActionGrid = CreateSpellList(this, character.Flees, character).ToArray();
                    ActionGrid[BACK_INDEX] = CreateBackButton(this, character, "BACK");
                    break;

                case Selection.Type.EQUIP:
                    ActionGrid = CreateEquipmentProcesses(this, character).ToArray();
                    ActionGrid[BACK_INDEX] = CreateBackButton(this, character, "BACK");
                    break;

                case Selection.Type.SWITCH:
                    ActionGrid = CreateSwitchMenu(character, this).ToArray();
                    ActionGrid[BACK_INDEX] = CreateBackButton(this, character, "BACK");
                    break;

                case Selection.Type.TARGET:
                    Tooltip = string.Format("{0} > {1} > {2}\n{3}", character.DisplayName, targetedSpell.Name, Selection.TARGET.Text, targetedSpell.Description);
                    ActionGrid = CreateTargetList(this, character, targetedSpell, targets.Where(t => t.IsTargetable && t.State != CharacterState.KILLED).ToArray()).ToArray();
                    ActionGrid[BACK_INDEX] = CreateBackButton(this, character, "BACK");
                    break;
            }
        }
    }
}