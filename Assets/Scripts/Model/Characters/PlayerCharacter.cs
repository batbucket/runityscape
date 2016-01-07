using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class PlayerCharacter : Character {
    SelectionType selectionType;
    Stack<SelectionType> lastSelectionStack;
    Spell selectedSpell;
    Spell lastNonAttackSpell;

    public const int BACK_INDEX = 11; //Last hotkey is letter 'V'
    public const int ALLY_CAST_OFFSET = 4;

    public PlayerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality) {
        lastSelectionStack = new Stack<SelectionType>();
    }

    public override void act(int chargeAmount, Game game) {
        charge(chargeAmount);
        display(game);
    }

    void display(Game game) {
        switch (selectionType) {
            case SelectionType.ACT:
                showSpellsAsList(actSpells, game, false);
                showBackButton(game);
                break;
            case SelectionType.FAIM:
                game.getActionGrid().setButtonAttributes(ActionGridFactory.createProcesses(
                    getSpellText(fightSpells[0]), () => {
                        targetSelection(fightSpells[0], game);
                    },
                    getSpellText(lastNonAttackSpell), () => {
                        targetSelection(lastNonAttackSpell, game);
                    },
                    getCorrectSpellListName(), () => {
                        setSelectionType(SelectionType.SPELLS);
                    },
                    "Act", () => setSelectionType(SelectionType.ACT),
                    "Item", () => setSelectionType(SelectionType.ITEM),
                    "Mercy", () => setSelectionType(SelectionType.MERCY)
                    ));
                game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess("Switch", () => Debug.Log("This aint a thing yet!")), BACK_INDEX);
                showBackButton(game);
                break;
            case SelectionType.ITEM:
                showBackButton(game);
                break;
            case SelectionType.MERCY:
                showSpellsAsList(mercySpells, game, false);
                showBackButton(game);
                break;
            case SelectionType.NONE:
                game.getActionGrid().clearAllButtonAttributes();
                break;
            case SelectionType.SPELLS:
                showSpellsAsList(fightSpells, game, true);
                showBackButton(game);
                break;
            case SelectionType.CHOOSE_TARGET:
                switch (selectedSpell.getTargetType()) {
                    case TargetType.SINGLE_ALLY:
                        showTargets(game.getAllies(side), game);
                        break;
                    case TargetType.SINGLE_ENEMY:
                        showTargets(game.getEnemies(side), game);
                        break;
                }
                showBackButton(game);
                break;
        }
    }

    void showSpellsAsList(List<Spell> spells, Game game, bool hideFirst) {
        int startIndex = hideFirst ? 1 : 0;
        for (int i = startIndex; i < spells.Count; i++) {
            Spell spell = spells[i];
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(
                getSpellText(spell), () => targetSelection(spell, game)
            ), hideFirst ? i - 1 : i);
        }
    }

    void showBackButton(Game game) {
        //this one doesn't use set() to prevent infinite looping of lastSelection
        game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess("Back", () => returnToLastSelection() ), BACK_INDEX);
    }

    public override void onStart(Game game) {
        //this one doesn't use set() to prevent NONE from being the original selection on the bottom of the stack
        selectionType = SelectionType.FAIM;
    }

    void setSelectionType(SelectionType selectionType) {
        pushLastSelection(this.selectionType);
        this.selectionType = selectionType;
    }

    void pushLastSelection(SelectionType lastSelection) {
        this.lastSelectionStack.Push(lastSelection);
    }

    void showTargets(List<Character> targets,Game game) {
        for (int i = 0; i < targets.Count; i++) {
            Character target = targets[i];
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(selectedSpell.canCast() ? target.getName() : string.Format("{0}{1}{2}", "<color=red>", target.getName(), "</color>"), () => {
                selectedSpell.setTarget(target);
                castAndResetStateIfSuccessful();
            }), i);
        }
    }

    void resetSelection() {
        this.selectionType = SelectionType.FAIM;
        lastSelectionStack.Clear();
    }

    void returnToLastSelection() {
        selectionType = lastSelectionStack.Pop();
    }

    void castAndResetStateIfSuccessful() {
        selectedSpell.tryCast();
        if (selectedSpell.getResult() != SpellResult.CANT_CAST) {

            resetSelection();
            if (selectedSpell != fightSpells[0]) {
                lastNonAttackSpell = selectedSpell;
            }
        }
    }

    string getSpellText(Spell spell) {
        if (spell != null) {
            return spell.getNameAndCosts();
        } else {
            return "";
        }
    }

    void targetSelection(Spell spell, Game game) {
        if (spell == null) {
            return;
        }
        selectedSpell = spell;
        List<Character> enemies = game.getEnemies(side);
        List<Character> allies = game.getEnemies(side);
        List<Character> allChars = game.getAll();
        switch (selectedSpell.getTargetType()) {
            case TargetType.SINGLE_ALLY:
                showTargetSelectionIfNeeded(allies);
                break;
            case TargetType.SINGLE_ENEMY:
                showTargetSelectionIfNeeded(enemies);
                break;
            case TargetType.SELF:
                selectedSpell.setTarget(this);
                break;
            case TargetType.ALL_ALLY:
                selectedSpell.setTargets(game.getAllies(side));
                break;
            case TargetType.ALL_ENEMY:
                selectedSpell.setTargets(game.getEnemies(side));
                break;
            case TargetType.ALL:
                selectedSpell.setTargets(game.getAll());
                break;
        }
        castAndResetStateIfSuccessful();
    }

    string getCorrectSpellListName() {
        return resources.ContainsKey(ResourceType.SKILL) ? "Skill" : "Spell";
    }

    void showTargetSelectionIfNeeded(List<Character> targets) {
        if (targets.Count == 0) {
            //Do nothing
        } else if (targets.Count == 1) {
            selectedSpell.setTarget(targets[0]);
        } else {
            setSelectionType(SelectionType.CHOOSE_TARGET);
        }
    }
}
