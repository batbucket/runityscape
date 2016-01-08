using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class PlayerCharacter : Character {
    SelectionType selectionType;
    Stack<SelectionType> lastSelectionStack;
    Spell lastNonAttackSpell;

    Spell targetedSpell;
    List<Character> targets;

    public const int BACK_INDEX = 11; //Last hotkey is letter 'V'
    public const int ALLY_CAST_OFFSET = 4;

    public PlayerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality, textColor) {
        lastSelectionStack = new Stack<SelectionType>();
    }

    public override void act(int chargeAmount, Game game) {
        base.act(chargeAmount, game);
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
                        determineTargetSelection(fightSpells[0], game);
                    },
                    getSpellText(lastNonAttackSpell), () => {
                        determineTargetSelection(lastNonAttackSpell, game);
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
                showInventory(game);
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
                showTargetSelection(game);
                showBackButton(game);
                break;
        }
    }

    void showSpellsAsList(List<Spell> spells, Game game, bool hideFirst) {
        int startIndex = hideFirst ? 1 : 0;
        for (int i = startIndex; i < spells.Count; i++) {
            Spell spell = spells[i];
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(
                getSpellText(spell), () => determineTargetSelection(spell, game)
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

    void resetSelection() {
        this.selectionType = SelectionType.FAIM;
        lastSelectionStack.Clear();
    }

    void returnToLastSelection() {
        selectionType = lastSelectionStack.Pop();
    }

    void castAndResetStateIfSuccessful(Spell spell, Game game) {
        spell.tryCast();
        game.postText(spell.getCastText(), textColor);
        getReactions(spell, game);
        if (spell.getResult() != SpellResult.CANT_CAST) {
            resetSelection();
            if (spell != fightSpells[0] && !(spell is Item)) {
                lastNonAttackSpell = spell;
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

    void determineTargetSelection(Spell spell, Game game) {
        if (spell == null) {
            return;
        }
        List<Character> enemies = game.getEnemies(side);
        List<Character> allies = game.getEnemies(side);
        List<Character> allChars = game.getAll();
        switch (spell.getTargetType()) {
            case TargetType.SINGLE_ALLY:
                determineSingleTargetQuickCast(spell, allies, game);
                return;
            case TargetType.SINGLE_ENEMY:
                determineSingleTargetQuickCast(spell, enemies, game);
                return;
            case TargetType.SELF:
                spell.setTarget(this);
                break;
            case TargetType.ALL_ALLY:
                spell.setTargets(game.getAllies(side));
                break;
            case TargetType.ALL_ENEMY:
                spell.setTargets(game.getEnemies(side));
                break;
            case TargetType.ALL:
                spell.setTargets(game.getAll());
                break;
        }
        castAndResetStateIfSuccessful(spell, game);
    }

    void determineSingleTargetQuickCast(Spell spell, List<Character> targets, Game game) {
        if (targets.Count == 0) {
            // Do nothing
        } else if (targets.Count == 1) {
            spell.setTarget(targets[0]);
            castAndResetStateIfSuccessful(spell, game);
        } else {
            setTargetedSpell(spell);
            setTargets(targets);
            setSelectionType(SelectionType.CHOOSE_TARGET);
        }
    }

    void showTargetSelection(Game game) {
        for (int i = 0; i < targets.Count; i++) {
            Character target = targets[i];
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(getTargetedSpell().canCast() ? target.getName() : string.Format("{0}{1}{2}", "<color=red>", target.getName(), "</color>"), () => {
                getTargetedSpell().setTarget(target);
                castAndResetStateIfSuccessful(getTargetedSpell(), game);
            }), i);
        }
    }

    void showInventory(Game game) {
        int index = 0;
        foreach (KeyValuePair<Item, int> pair in inventory.getDictionary()) {
            string nameAndCount = string.Format("{0} x {1}", pair.Key.getName(), pair.Value);
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(pair.Key.canCast() ? nameAndCount : string.Format("{0}{1}{2}", "<color=red>", nameAndCount, "</color>"), () => {
                inventory.remove(pair.Key);
                castAndResetStateIfSuccessful(pair.Key, game);
            }), index++);
        }
    }

    string getCorrectSpellListName() {
        return resources.ContainsKey(ResourceType.SKILL) ? "Skill" : "Spell";
    }

    void setTargetedSpell(Spell spell) {
        targetedSpell = spell;
    }

    void setTargets(List<Character> targets) {
        this.targets = targets;
    }

    List<Character> getTargets() {
        return targets;
    }

    Spell getTargetedSpell() {
        return targetedSpell;
    }
}
