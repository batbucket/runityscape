using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class BattlePage : Page {
    internal Queue<Character> characterQueue;
    internal Character activeCharacter;
    internal SpellFactory targetedSpell;
    internal IList<Character> targets;
    internal TreeNode<Selection> selectionRoot;
    internal TreeNode<Selection> currentSelectionNode;

    public Func<bool> IsVictory;
    public Page Victory;
    public Func<bool> IsDefeat;
    public Page Defeat;

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

    public BattleState State;

    private Character mainCharacter;

    private LootPage loot;

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
        characterQueue = new Queue<Character>();
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
        selectionRoot = new TreeNode<Selection>(Selection.FAIM); //Topmost node
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

    public BattlePage(
    Party party,
    Page victory,
    Page defeat,
    string musicLoc,
    string location,
    string text,
    params Character[] enemies) : this(text, location, party.Main, party.Members, enemies, null, null, null, null, null, musicLoc, null, victory, null, defeat) {

    }

    protected override void OnAddCharacter(Character c) {
        base.OnAddCharacter(c);
        c.OnBattleStart();
        c.Mercies.Add(new Flee(Defeat));
    }

    protected override void OnAnyEnter() {
        base.OnAnyEnter();
        GetAll().ForEach(c => {
            c.OnBattleStart();
            c.Mercies.Add(new Flee(Defeat));
        });
    }

    protected override void OnAnyExit() {
        GetAll().ForEach(c => {
            c.OnBattleStart();
            c.Mercies.Remove(new Flee(Defeat));
        });
    }

    protected override void OnTick() {
        if (State == BattleState.BATTLE && IsVictory.Invoke()) {
            State = BattleState.VICTORY;
        } else if (State == BattleState.BATTLE && IsDefeat.Invoke()) {
            State = BattleState.DEFEAT;
        }

        if (State == BattleState.DEFEAT || State == BattleState.VICTORY) {
            this.Tooltip = "";
            foreach (Character c in GetAll()) {
                c.OnBattleEnd();
            }
        }

        switch (State) {
            case BattleState.PRE_BATTLE:
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Engage", "", () => State = BattleState.BATTLE);
                break;
            case BattleState.BATTLE:
                IList<Character> all = GetAll().Where(c => c.IsTargetable).ToArray();
                foreach (Character c in all) {
                    if (c.IsCommandable) { characterQueue.Enqueue(c); }
                    if (c.HasResource(ResourceType.CHARGE)) { c.Resources[ResourceType.CHARGE].IsVisible = true; }
                    c.BattleTick(mainCharacter);
                }

                /**
                 * Remove activeCharacter if they are not charged,
                 * Indicating that they've casted a spell or are under some status effect
                 */
                if (activeCharacter == null || (activeCharacter != null && (!activeCharacter.IsCharged || activeCharacter.State == CharacterState.DEFEAT || activeCharacter.State == CharacterState.KILLED))) {
                    activeCharacter = PopAbledCharacter(characterQueue);
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
            case BattleState.VICTORY:
                IList<Character> allies = GetAllies(mainCharacter);
                int expSum = GetEnemies(mainCharacter).Sum(c => c.GetResourceCount(ResourceType.DEATH_EXP, false));
                foreach (Character c in allies) {
                    c.OnVictory();
                    if (expSum > 0 && c.HasResource(ResourceType.EXPERIENCE)) {
                        Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} gained {1} experience.", c.DisplayName, expSum)));
                        Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} has enough experience to level up.", c.DisplayName)));
                        c.AddToResource(ResourceType.EXPERIENCE, false, expSum);
                    }
                }
                int goldSum = GetEnemies(mainCharacter).Sum(c => c.Inventory.Gold);
                mainCharacter.Inventory.Gold += goldSum;
                if (goldSum > 0) {
                    Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("<color=yellow>{0}</color> gold was added to the inventory.", goldSum)));
                }

                Game.Instance.TextBoxes.AddTextBox(new TextBox("Victory!"));

                Game.Instance.Sound.StopAll();
                loot = new LootPage(this, new Party(mainCharacter, GetAllies(mainCharacter)), GetEnemies(mainCharacter));
                State = BattleState.POST_VICTORY;
                break;
            case BattleState.DEFEAT:
                IList<Character> enemies = GetEnemies(mainCharacter);
                foreach (Character c in enemies) {
                    c.OnVictory();
                }
                Game.Instance.TextBoxes.AddTextBox(new TextBox("Defeat!"));
                Game.Instance.Sound.StopAll();

                State = BattleState.POST_DEFEAT;
                break;
            case BattleState.POST_VICTORY:
                ActionGrid = new IButtonable[0];
                if (loot.HasLoot) {
                    ActionGrid[0] = loot;
                }
                ActionGrid[1] = new ItemManagePage(this, new Party(mainCharacter, GetAllies(mainCharacter)));
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Continue", "", () => Game.Instance.CurrentPage = Victory);
                break;
            case BattleState.POST_DEFEAT:
                ActionGrid = new IButtonable[0];
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Continue", "", () => Game.Instance.CurrentPage = Defeat);
                break;
        }
    }

    private void Display(Character character) {
        ClearActionGrid();
        Tooltip = string.Format("{0} > {1}\n", character.DisplayName, currentSelectionNode.Value.Text);
        switch (currentSelectionNode.Value.SelectionType) {
            case Selection.Type.FAIM:
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
                ActionGrid = CreateSpellList(this, character.Mercies, character).ToArray();
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


    //Creates a Process that brings up the "Who to target with this Spell" display
    private static Process CreateSpellProcess(BattlePage page, SpellFactory spell, Character caster) {
        return new Process(spell.GetNameAndCosts(caster), string.Format("{0} > {1}  {2}\n{3}", caster.DisplayName, spell.Name, spell.GetCosts(), spell.Description),
            () => {
                DetermineTargetSelection(page, spell, caster);
            });
    }

    //Show worn equipment first that can be unequipped, then Items in inventory that can be equipped
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

    private static Process CreateUnequipProcess(Character caster, EquippableItem item, ICollection<EquippableItem> equipment) {
        return new Process(
            Util.Color(string.Format("{0}{1}", item.Name, "(E)"), Color.yellow),
            string.Format("{0} > {1} > {2}\n{3}", caster.DisplayName, "UNEQUIP", item.Name, item.Description), () => {
                Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.", caster.DisplayName, item.Name)));
                item.UnequipItemInSlot(caster);
            });
    }

    private static IList<Process> CreateSpellList<T>(BattlePage page, ICollection<T> spells, Character caster) where T : SpellFactory {
        IList<Process> processes = new List<Process>();
        foreach (SpellFactory mySpell in spells) {
            SpellFactory spell = mySpell;
            processes.Add(CreateSpellProcess(page, spell, caster));
        }
        return processes;
    }

    //Creates a process that targets a specific Character with a spell
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

    private static IList<Process> CreateTargetList(BattlePage page, Character caster, SpellFactory spell, IList<Character> targets) {
        IList<Process> processes = new List<Process>();
        foreach (Character myTarget in targets) {
            Character target = myTarget;
            processes.Add(CreateTargetProcess(page, spell, caster, target));
        }
        return processes;
    }

    private static Process CreateBackButton(BattlePage page, Character current, string parentSelectionName) {
        return new Process("Back", string.Format("{0} > {1}\n", current.DisplayName, parentSelectionName),
            () => {
                page.currentSelectionNode = page.currentSelectionNode.Parent;
            });
    }

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

    private static IList<Process> CreateSwitchMenu(Character current, BattlePage page) {
        IList<Process> processes = new List<Process>();
        foreach (Character myTarget in page.GetAll()) {
            Character target = myTarget;

            //You are not allowed to switch with yourself.
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

    /**
     * Since you can switch out the activeCharacter whenever,
     * Characters in the Queue may not neccessarily be charged
     */
    private static Character PopAbledCharacter(Queue<Character> characterQueue) {

        //Remove all !IsCharged() characters from the front of the Queue
        while (characterQueue.Count > 0 && !characterQueue.Peek().IsCharged) {
            characterQueue.Dequeue();
        }

        return characterQueue.Count == 0 ? null : characterQueue.Dequeue();
    }

    private static void DetermineTargetSelection(BattlePage page, SpellFactory spell, Character caster) {
        Util.Assert(spell != null);

        bool isResetSelection = false;
        SpellTargetState sts = SpellTargetState.ONE_TARGET;
        IList<Character> targetables = null;

        //These TargetTypes might require target selection if there's more than 1 possible target.
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
                    spell.TryCast(caster, targetables[0]);
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
                case SpellTargetState.NO_TARGETS:
                    Util.Log("There's no targets?");
                    break;
            }
        }
    }

    private enum SpellTargetState {
        ONE_TARGET, MULTIPLE_TARGETS, NO_TARGETS
    }
    private static SpellTargetState GetSpellTargetState(SpellFactory spell, Character caster, IList<Character> targetables) {
        if (spell.IsSingleTargetQuickCastable(caster, targetables) && spell.IsCastable(caster, targetables[0])) {
            return SpellTargetState.ONE_TARGET;
        } else if (spell.IsCastable(caster) && targetables.Count > 1) {
            return SpellTargetState.MULTIPLE_TARGETS;
        } else {
            return SpellTargetState.NO_TARGETS;
        }
    }

    private static void SpellCastEnd(SpellFactory spell, Character caster, BattlePage page) {
        UpdateLastSpellStack(spell, caster);
        page.currentSelectionNode = page.selectionRoot;
    }
}
