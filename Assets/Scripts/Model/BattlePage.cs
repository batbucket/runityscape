using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BattlePage : Page {
    Stack<Spell> lastSpellStack;
    Queue<Character> characterQueue;
    Character activeCharacter;
    Spell targetedSpell;
    IList<Character> targets;
    TreeNode<Selection> selectionTree;
    TreeNode<Selection> currentSelectionNode;

    public const int BACK_INDEX = 7; //Last hotkey is letter 'F'
    public const int ATTACK_INDEX = 0;
    public const int LAST_SPELL_INDEX = 1;

    public BattlePage(string text = "", string tooltip = "", Character mainCharacter = null, List<Character> left = null, List<Character> right = null,
        Action onFirstEnter = null, Action onEnter = null, Action onFirstExit = null, Action onExit = null, Action onTick = null)
            : base(text, tooltip, mainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, null) {
        lastSpellStack = new Stack<Spell>();
        characterQueue = new Queue<Character>();

        /**
         * Set up the Selection Tree.
         * Tree looks like this:
         *               FAIM
         * SPELLS    ACTS   ITEMS    MERCIES    SWITCH
         * TARGET   TARGET  TARGET   TARGET     TARGET
         */
        selectionTree = new TreeNode<Selection>(Selection.FAIM); //Topmost node
        selectionTree.AddChildren(Selection.SPELL, Selection.ACT, Selection.ITEM, Selection.MERCY, Selection.SWITCH);
        foreach (TreeNode<Selection> st in selectionTree.Children) {
            st.AddChild(Selection.TARGET);
        }
        currentSelectionNode = selectionTree;
    }

    public override void Tick() {
        base.Tick();
        foreach (Character c in GetAll()) {
            c.Tick();
            c.CalculateSpeed(MainCharacter);
            if (c.IsControllable()) { characterQueue.Enqueue(c); }
        }

        /**
         * Remove activeCharacter if they are not charged,
         * Indicating that they've casted a spell or are under some status effect
         */
        if (activeCharacter == null || (activeCharacter != null && !activeCharacter.IsCharged())) {
            activeCharacter = PopAbledCharacter();
        }

        if (activeCharacter != null) {
            Display(activeCharacter);
        }
    }

    /**
     * Since you can switch out the activeCharacter whenever,
     * Characters in the Queue may not neccessarily be charged
     */
    Character PopAbledCharacter() {

        //Remove all !IsCharged() characters from the front of the Queue
        while (characterQueue.Count > 0 && !characterQueue.Peek().IsCharged()) {
            characterQueue.Dequeue();
        }

        return characterQueue.Count == 0 ? null : characterQueue.Dequeue();
    }

    void Display(Character character) { //Zzzzz null reference here
        Tooltip = string.Format(currentSelectionNode.Value.Question, character.Name, targetedSpell == null ? "" : targetedSpell.Name);
        if (currentSelectionNode.Value == Selection.FAIM) {
            AddSwitchButton(character);
            ActionGrid[ATTACK_INDEX] = new Process(character.Attack.Name, character.Attack.Description, () => DetermineTargetSelection(character.Attack, character));
            ActionGrid[LAST_SPELL_INDEX] = lastSpellStack.Count == 0 ? new Process() : new Process(lastSpellStack.Peek().Name, lastSpellStack.Peek().Description, () => DetermineTargetSelection(lastSpellStack.Peek(), character));
            int index = LAST_SPELL_INDEX + 1;
            foreach (KeyValuePair<Selection, ICollection<Spell>> pair in character.Selections) {
                ActionGrid[index++] = new Process(pair.Key.Name, string.Format(pair.Key.Declare, character.Name), () => currentSelectionNode = currentSelectionNode.FindChild(pair.Key));
            }
        }
        else {
            AddBackButton(character);
            if (currentSelectionNode.Value == Selection.TARGET) {
                ShowTargetsAsList(targets, character);
            }
            else if (currentSelectionNode.Value == Selection.SWITCH) {

            }
            else {
                ShowSpellsAsList(character.Selections[currentSelectionNode.Value], character);
            }
        }
    }

    void ShowSpellsAsList(ICollection<Spell> spells, Character caster) {
        int index = 0;
        foreach (Spell spell in spells) {
            ActionGrid[index++] = new Process(spell.GetNameAndInfo(caster), spell.Description, () => DetermineTargetSelection(spell, caster));
        }
    }

    void ShowTargetsAsList(IList<Character> targets, Character caster) {
        int index = 0;
        foreach (Character target in targets) {
            ActionGrid[index++] =
                new Process(
                    targetedSpell.IsCastable(caster, target) ? target.Name : Util.Color(target.Name, Color.red),
                    string.Format(currentSelectionNode.Value.Declare, caster.Name, target.Name, targetedSpell.Name), () => {
                        targetedSpell.TryCast(caster, target);
                    }
                );
        }
    }

    void AddBackButton(Character current) {
        this.ActionGrid[BACK_INDEX] = new Process("Back", "Go BACK to the previous selection.", () => ReturnToLastSelection());
    }

    void AddSwitchButton(Character current) {
        this.ActionGrid[BACK_INDEX] = new Process(Selection.SWITCH.Name, "SWITCH to another character.", () => currentSelectionNode = currentSelectionNode.FindChild(Selection.SWITCH));
    }

    void SwitchCharacterDisplay(Character current) {
        int index = 0;
        foreach (Character c in GetAll()) {
            if (c.IsDisplayable && c.IsCharged()) {
                ActionGrid[index++] = new Process(c.Name, string.Format("{0} will SWITCH with {1}.", current, c.Name), () => { }); //Make an instance field called currentdisplaying char and set it here
            }
        }
    }

    void ResetSelection() {
        this.currentSelectionNode = selectionTree;
    }

    void ReturnToLastSelection() {
        Util.Assert(currentSelectionNode.Parent != null);
        currentSelectionNode = currentSelectionNode.Parent;
    }

    void UpdateLastSpellStack(Spell spell, Character caster) {
        if (spell != caster.Attack) {
            lastSpellStack.Push(spell);
        }
        if (spell is Item && ((Item)spell).Count <= 0) {
            while (lastSpellStack.Count > 0 && spell.Equals(lastSpellStack.Peek())) {
                lastSpellStack.Pop();
            }
        }
    }

    void DetermineTargetSelection(Spell spell, Character caster) {
        Util.Assert(spell != null);
        switch (spell.TargetType) {
            case SpellTarget.SINGLE_ALLY:
                DetermineSingleTargetQuickCast(spell, caster, this.GetAllies(caster.Side));
                return;
            case SpellTarget.SINGLE_ENEMY:
                DetermineSingleTargetQuickCast(spell, caster, this.GetEnemies(caster.Side));
                return;
            case SpellTarget.SELF:
                spell.TryCast(caster, caster);
                break;
            case SpellTarget.ALL_ALLY:
                spell.TryCast(caster, this.GetAllies(caster.Side));
                break;
            case SpellTarget.ALL_ENEMY:
                spell.TryCast(caster, this.GetEnemies(caster.Side));
                break;
            case SpellTarget.ALL:
                spell.TryCast(caster, this.GetAll());
                break;
        }
        if (spell.Result != SpellResult.CANT_CAST) {
            foreach (Character witness in this.GetAll()) {
                witness.React(spell);
            }
            UpdateLastSpellStack(spell, caster);
            ResetSelection();
        }
    }

    void DetermineSingleTargetQuickCast(Spell spell, Character caster, IList<Character> targets) {
        if (targets.Count == 0) {
            // Do nothing
        }
        else if (targets.Count == 1) {
            spell.TryCast(caster, targets[0]);
        }
        else {
            targetedSpell = spell;
            this.targets = targets;
            currentSelectionNode = currentSelectionNode.FindChild(Selection.TARGET);
        }
    }
}
