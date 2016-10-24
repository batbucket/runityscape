using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.BattlePage {
    public class BattlePage : Page {
        internal Queue<Character> characterQueue;
        internal Character activeCharacter;
        internal SpellFactory targetedSpell;
        internal IList<Character> targets;
        internal TreeNode<Selection> selectionRoot;
        internal TreeNode<Selection> currentSelectionNode;

        public Func<bool> IsVictory;
        public Action OnVictory;
        public Func<bool> IsMercy;
        public Action OnMercy;
        public Func<bool> IsDefeat;
        public Action OnDefeat;

        public Selection CurrentSelection { get { return currentSelectionNode.Value; } }

        public const int BACK_INDEX = ActionGridView.ROWS * ActionGridView.COLS - 1;
        public const int ATTACK_INDEX = 0;
        public const int LAST_SPELL_INDEX = 1;
        public const int SPELL_INDEX = 4;
        public const int ACT_INDEX = 5;
        public const int ITEM_INDEX = 6;
        public const int MERCY_INDEX = 7;
        public const int EQUIP_INDEX = 10;
        public const int SWITCH_INDEX = 11;

        public const int FAIM_OFFSET = 4;

        BattleState? _battleState;
        public BattleState? State { get { return _battleState; } set { _battleState = value; } }

        private bool isTicking;

        public BattlePage(
            string text = "",
            string tooltip = "",
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
            Action onVictory = null,
            Func<bool> isMercy = null,
            Action onMercy = null,
            Func<bool> isDefeat = null,
            Action onDefeat = null
            )
            : base(text, tooltip, location, false, mainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, musicLoc: musicLoc) {

            characterQueue = new Queue<Character>();

            /**
             * Set up the Selection Tree.
             * Tree looks like this:
             *               FAIM   -> TARGET (quickcast)
             * SPELLS    ACTS   ITEMS    MERCIES  EQUIPS  SWITCH
             * TARGET   TARGET  TARGET   TARGET           TARGET
             */
            selectionRoot = new TreeNode<Selection>(Selection.FAIM); //Topmost node
            selectionRoot.AddChildren(Selection.SPELL, Selection.ACT, Selection.ITEM, Selection.MERCY, Selection.EQUIP, Selection.SWITCH, Selection.TARGET);
            foreach (TreeNode<Selection> st in selectionRoot.Children) {
                st.AddChild(Selection.TARGET);
            }
            currentSelectionNode = selectionRoot;

            this.IsVictory = isVictory ?? (() => GetEnemies(mainCharacter).All(c => c.State == CharacterState.KILLED)); // No non-killed enemies
            this.OnVictory = onVictory ?? (() => { });

            this.IsMercy = isMercy ?? (() => {
                return GetAll().All(c => (c.CastSpells.Count > 0 && string.Equals(c.CastSpells.Last().SpellFactory.Name, "Spare")) || c.State == CharacterState.DEFEAT); //Everyone is defeated or sparing
            });
            this.OnMercy = onMercy ?? (() => { });

            this.IsDefeat = isDefeat ?? (() => mainCharacter.State == CharacterState.KILLED);
            this.OnDefeat = onDefeat ?? (() => { Game.Instance.Defeat(); });
            this.State = BattleState.BATTLE;
            this.isTicking = true;
        }

        protected override void OnAddCharacter(Character c) {
            base.OnAddCharacter(c);
            c.OnBattleStart();
        }

        public override void OnAnyEnter() {
            base.OnAnyEnter();
            GetAll().ForEach(c => {
                c.OnBattleStart();
            });
        }

        public override void Tick() {
            if (!isTicking) {
                return;
            }

            base.Tick();

            if (IsMercy.Invoke()) {
                State = BattleState.MERCY;
            } else if (IsVictory.Invoke()) {
                State = BattleState.VICTORY;
            } else if (IsDefeat.Invoke()) {
                State = BattleState.DEFEAT;
            }

            if (_battleState == BattleState.MERCY || _battleState == BattleState.DEFEAT || _battleState == BattleState.VICTORY) {
                foreach (Character c in GetAll()) {
                    c.OnBattleEnd();
                }
                isTicking = false;
            }

            switch (State) {
                case BattleState.BATTLE:
                    IList<Character> all = GetAll().Where(c => c.IsTargetable).ToArray();
                    foreach (Character c in GetAll()) {
                        c.Tick(MainCharacter, true);
                        if (c.IsActive) { characterQueue.Enqueue(c); }
                        if (c.HasResource(ResourceType.CHARGE)) { c.Resources[ResourceType.CHARGE].IsVisible = true; }
                    }

                    /**
                     * Remove activeCharacter if they are not charged,
                     * Indicating that they've casted a spell or are under some status effect
                     */
                    if (activeCharacter == null || (activeCharacter != null && (!activeCharacter.IsCharged || activeCharacter.State == CharacterState.DEFEAT || activeCharacter.State == CharacterState.KILLED))) {
                        activeCharacter = PageUtil.PopAbledCharacter(characterQueue);
                        currentSelectionNode = selectionRoot;
                    }

                    //No one can move so clear the board
                    if (activeCharacter == null) {
                        Tooltip = "";
                        ClearActionGrid();
                    }

                    if (activeCharacter != null) {
                        Display(activeCharacter);
                    }
                    break;
                case BattleState.MERCY:
                    OnMercy.Invoke();
                    Game.Instance.Sound.StopAll();
                    break;
                case BattleState.VICTORY:
                    foreach (Character c in GetAllies(MainCharacter)) {
                        c.OnVictory();
                    }
                    OnVictory.Invoke();
                    Game.Instance.Sound.StopAll();
                    break;
                case BattleState.DEFEAT:
                    foreach (Character c in GetEnemies(MainCharacter)) {
                        c.OnVictory();
                    }
                    Game.Instance.Sound.StopAll();
                    OnDefeat.Invoke();
                    break;
            }
        }

        private void Display(Character character) {
            ClearActionGrid();
            Tooltip = string.Format("{0} > {1}\n", character.DisplayName, currentSelectionNode.Value.Text);
            switch (currentSelectionNode.Value.SelectionType) {
                case Selection.Type.FAIM:
                    ActionGrid[SWITCH_INDEX] = PageUtil.CreateSwitchButton(character, this);
                    ActionGrid[ATTACK_INDEX] = character.Attack == null ? new Process() : PageUtil.CreateSpellProcess(this, character.Attack, character);
                    ActionGrid[LAST_SPELL_INDEX] = character.SpellStack.Count == 0 ? null : PageUtil.CreateSpellProcess(this, character.SpellStack.Peek(), character);

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
                    ActionGrid[MERCY_INDEX] = new Process(Selection.MERCY.Name, string.Format("{0} > {1}\n", character.DisplayName, Selection.MERCY.Text),
                        () => {
                            currentSelectionNode = currentSelectionNode.FindChild(Selection.MERCY);
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
                    ActionGrid = PageUtil.CreateSpellList(this, character.Spells, character).ToArray();
                    ActionGrid[BACK_INDEX] = PageUtil.CreateBackButton(this, character, "BACK");
                    break;
                case Selection.Type.ACTION:
                    ActionGrid = PageUtil.CreateSpellList(this, character.Actions, character).ToArray();
                    ActionGrid[BACK_INDEX] = PageUtil.CreateBackButton(this, character, "BACK");
                    break;
                case Selection.Type.ITEM:
                    ActionGrid = PageUtil.CreateSpellList(this, character.Items, character).ToArray();
                    ActionGrid[BACK_INDEX] = PageUtil.CreateBackButton(this, character, "BACK");
                    break;
                case Selection.Type.MERCY:
                    ActionGrid = PageUtil.CreateSpellList(this, character.Mercies, character).ToArray();
                    ActionGrid[BACK_INDEX] = PageUtil.CreateBackButton(this, character, "BACK");
                    break;
                case Selection.Type.EQUIP:
                    ActionGrid = PageUtil.CreateEquipmentProcesses(this, character).ToArray();
                    ActionGrid[BACK_INDEX] = PageUtil.CreateBackButton(this, character, "BACK");
                    break;
                case Selection.Type.SWITCH:
                    ActionGrid = PageUtil.CreateSwitchMenu(character, this).ToArray();
                    ActionGrid[BACK_INDEX] = PageUtil.CreateBackButton(this, character, "BACK");
                    break;
                case Selection.Type.TARGET:
                    Tooltip = string.Format("{0} > {1} > {2}\n{3}", character.DisplayName, targetedSpell.Name, Selection.TARGET.Text, targetedSpell.Description);
                    ActionGrid = PageUtil.CreateTargetList(this, character, targetedSpell, targets.Where(t => t.IsTargetable && t.State != CharacterState.KILLED).ToArray()).ToArray();
                    ActionGrid[BACK_INDEX] = PageUtil.CreateBackButton(this, character, "BACK");
                    break;
            }
        }
    }
}
