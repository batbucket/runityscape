using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BattlePage : Page {
    Queue<Character> characterQueue;
    Character activeCharacter;
    Spell targetedSpell;
    IList<Character> targets;
    TreeNode<Selection> selectionTree;
    TreeNode<Selection> currentSelectionNode;

    public const int BACK_INDEX = ActionGridView.ROWS * ActionGridView.COLS - 1;
    public const int ATTACK_INDEX = 0;
    public const int LAST_SPELL_INDEX = 1;
    public const int FAIM_OFFSET = 4;

    public BattlePage(
        string text = "",
        string tooltip = "",
        Character mainCharacter = null,
        Character[] left = null,
        Character[] right = null,
        Action onFirstEnter = null,
        Action onEnter = null,
        Action onFirstExit = null,
        Action onExit = null,
        Action onTick = null
        )
        : base(text, tooltip, mainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick) {

        characterQueue = new Queue<Character>();

        /**
         * Set up the Selection Tree.
         * Tree looks like this:
         *               FAIM   -> TARGET (quickcast)
         * SPELLS    ACTS   ITEMS    MERCIES    SWITCH
         * TARGET   TARGET  TARGET   TARGET     TARGET
         */
        selectionTree = new TreeNode<Selection>(Selection.FAIM); //Topmost node
        selectionTree.AddChildren(Selection.SPELL, Selection.ACT, Selection.ITEM, Selection.MERCY, Selection.SWITCH, Selection.TARGET);
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

        //No one can move so clear the board
        if (activeCharacter == null) {
            Tooltip = "";
            ClearActionGrid();
        } else {
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

    void Display(Character character) {
        ClearActionGrid();
        Tooltip = string.Format(currentSelectionNode.Value.Question, character.Name, targetedSpell == null ? "" : targetedSpell.Name);
        if (currentSelectionNode.Value == Selection.FAIM) {
            ShowSwitchButton(character);
            ActionGrid[ATTACK_INDEX] = new Process(character.Attack.Name, character.Attack.Description,
                () => {
                    Tooltip = "";
                    DetermineTargetSelection(character.Attack, character);
                });
            ActionGrid[LAST_SPELL_INDEX] = character.SpellStack.Count == 0 ? new Process() : new Process(character.SpellStack.Peek().GetNameAndInfo(character), character.SpellStack.Peek().Description,
                () => {
                    Tooltip = "";
                    DetermineTargetSelection(character.SpellStack.Peek(), character);
                });
            int index = FAIM_OFFSET;
            foreach (KeyValuePair<Selection, ICollection<Spell>> myPair in character.Selections) {
                KeyValuePair<Selection, ICollection<Spell>> pair = myPair;
                ActionGrid[index++] = new Process(pair.Key.Name, string.Format(pair.Key.Declare, character.Name),
                    () => {
                        Tooltip = "";
                        currentSelectionNode = currentSelectionNode.FindChild(pair.Key);
                    });
            }
        } else {
            ShowBackButton(character);
            if (currentSelectionNode.Value == Selection.TARGET) {
                ShowTargetsAsList(targets, character);
            } else if (currentSelectionNode.Value == Selection.SWITCH) {
                ShowSwitchMenu(character);
            } else {
                ShowSpellsAsList(character.Selections[currentSelectionNode.Value], character);
            }
        }
    }

    void ShowSpellsAsList(ICollection<Spell> spells, Character caster) {
        int index = 0;
        foreach (Spell mySpell in spells) {
            Spell spell = mySpell;
            ActionGrid[index++] = new Process(spell.GetNameAndInfo(caster), spell.Description,
                () => {
                    Tooltip = "";
                    DetermineTargetSelection(spell, caster);
                });
        }
    }

    void ShowTargetsAsList(IList<Character> targets, Character caster) {
        int index = 0;
        foreach (Character myTarget in targets) {
            Character target = myTarget;
            ActionGrid[index++] =
                new Process(
                    targetedSpell.IsCastable(caster, target) ? target.Name : Util.Color(target.Name, Color.red),
                    string.Format(currentSelectionNode.Value.Declare, caster.Name, target.Name, targetedSpell.Name),
                    () => {
                        Tooltip = "";
                        targetedSpell.TryCast(caster, target);
                        SpellCastEnd(targetedSpell, caster);
                    }
                );
        }
    }

    void ShowBackButton(Character current) {
        this.ActionGrid[BACK_INDEX] = new Process("Back", "Go BACK to the previous selection.",
            () => {
                Tooltip = "";
                ReturnToLastSelection();
            });
    }

    void ShowSwitchButton(Character current) {
        this.ActionGrid[BACK_INDEX] = new Process(Selection.SWITCH.Name, string.Format(Selection.SWITCH.Declare, current),
            () => {
                Tooltip = "";
                currentSelectionNode = currentSelectionNode.FindChild(Selection.SWITCH);
            });
    }

    void ShowSwitchMenu(Character current) {
        int index = 0;
        foreach (Character myTarget in GetAll()) {
            Character target = myTarget;

            //You are not allowed to switch with yourself.
            if (!current.Equals(target) && target.IsDisplayable && target.IsCharged()) {
                ActionGrid[index++] = new Process(target.Name, string.Format("{0} will SWITCH with {1}.", current.Name, target.Name),
                    () => {
                        Tooltip = "";
                        activeCharacter = target;
                        ReturnToLastSelection();
                    });
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
            caster.SpellStack.Push(spell);
        }
        if (spell is Item && ((Item)spell).Count <= 0) {
            while (caster.SpellStack.Count > 0 && spell.Equals(caster.SpellStack.Peek())) {
                caster.SpellStack.Pop();
            }
        }
    }

    void DetermineTargetSelection(Spell spell, Character caster) {
        Util.Assert(spell != null);

        //These TargetTypes might require target selection if there's more than 1 possible target.
        if (spell.TargetType == SpellTarget.SINGLE_ALLY || spell.TargetType == SpellTarget.SINGLE_ENEMY) {
            switch (spell.TargetType) {
                case SpellTarget.SINGLE_ALLY:
                    DetermineSingleTargetQuickCast(spell, caster, this.GetAllies(caster.Side));
                    break;
                case SpellTarget.SINGLE_ENEMY:
                    DetermineSingleTargetQuickCast(spell, caster, this.GetEnemies(caster.Side));
                    break;
            }
        } else { //These TargetTypes don't need target selection and thusly can have their results be instantly evaluated.
            switch (spell.TargetType) {
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
            SpellCastEnd(spell, caster);
        }
    }

    void DetermineSingleTargetQuickCast(Spell spell, Character caster, IList<Character> targets) {
        if (targets.Count == 0) {
            // Do nothing
        } else if (targets.Count == 1) {
            spell.TryCast(caster, targets[0]);
            SpellCastEnd(spell, caster);
        } else {
            targetedSpell = spell;
            this.targets = targets;
            currentSelectionNode = currentSelectionNode.FindChild(Selection.TARGET); //No linkage from FAIM -> Target
        }
    }

    void SpellCastEnd(Spell spell, Character caster) {
        if (spell.Result != SpellResult.CANT_CAST) {
            foreach (Character witness in this.GetAll()) {
                witness.React(spell);
            }
            UpdateLastSpellStack(spell, caster);
            ResetSelection();
        }
    }
}
