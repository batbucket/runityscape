using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class PlayerCharacter : Character {
    SelectionType selectionType;
    Stack<SelectionType> lastSelectionStack;
    Stack<Spell> lastSpellStack;

    Spell targetedSpell;
    List<Character> targets;

    public const int BACK_INDEX = 7; //Last hotkey is letter 'F'
    public const int ALLY_CAST_OFFSET = 4;

    public PlayerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality, textColor) {
        lastSelectionStack = new Stack<SelectionType>();
        lastSpellStack = new Stack<Spell>();
        lastSpellStack.Push(null); //placeholder
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
                    getSpellNameAndInfo(fightSpells[0]), getSpellDescription(fightSpells[0]), () => {
                        determineTargetSelection(fightSpells[0], game);
                    },
                    getSpellNameAndInfo(lastSpellStack.Peek()), getSpellDescription(lastSpellStack.Peek()), () => {
                        determineTargetSelection(lastSpellStack.Peek(), game);
                    },
                    getCorrectSpellListName(), "Abilities cost resources.", () => {
                        setSelectionType(SelectionType.SPELLS);
                    },
                    "Act", "Perform a non-combat action.", () => setSelectionType(SelectionType.ACT),
                    "Item", "Consume or equip items.", () => setSelectionType(SelectionType.ITEM),
                    "Mercy", "Attempt to end the fight peacefully.", () => setSelectionType(SelectionType.MERCY)
                    ));
                game.getActionGrid().setButtonAttribute(new Process("Switch", "Change control to another unit.", () => Debug.Log("This aint a thing yet!")), BACK_INDEX);
                break;
            case SelectionType.ITEM:
                showSpellsAsList(inventory.getList().Cast<Spell>().ToList(), game, false);
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
                showTargetsAsList(targets, game);
                showBackButton(game);
                break;
        }
    }

    void showSpellsAsList(List<Spell> spells, Game game, bool hideFirst) {
        int startIndex = hideFirst ? 1 : 0;
        for (int i = startIndex; i < spells.Count; i++) {
            Spell spell = spells[i];
            game.getActionGrid().setButtonAttribute(new Process(
                getSpellNameAndInfo(spell), getSpellDescription(spell), () => determineTargetSelection(spell, game)
            ), hideFirst ? i - 1 : i);
        }
    }

    void showTargetsAsList(List<Character> targets, Game game) {
        for (int i = 0; i < targets.Count; i++) {
            Character target = targets[i];
            game.getActionGrid().setButtonAttribute(new Process(
                string.Format("{0}{1}{2}", targetedSpell.canCast() ? "" : "<color=red>", target.getName(), targetedSpell.canCast() ? "" : "</color>"), targetedSpell.getDescription(), () => {
                    targetedSpell.setTarget(target);
                    castAndResetStateIfSuccessful(targetedSpell, game);
                }
                ), i);
        }
    }

    void showBackButton(Game game) {
        //this one doesn't use set() to prevent infinite looping of lastSelection
        game.getActionGrid().setButtonAttribute(new Process("Back", "Go back to the previous selection.", () => returnToLastSelection() ), BACK_INDEX);
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
        talk(spell.getCastText(), game);
        if (spell.getResult() != SpellResult.CANT_CAST) {
            getReactions(spell, game);
            resetSelection();
            if (spell != fightSpells[0]) {
                pushLastSpell(spell);
            }
            if (spell is Item && ((Item)spell).getCount() <= 0) {
                while (lastSpellStack.Peek() != null && spell.Equals(lastSpellStack.Peek())) {
                    lastSpellStack.Pop();
                }
            }
        }
    }

    string getSpellNameAndInfo(Spell spell) {
        if (spell != null) {
            return spell.getNameAndInfo();
        } else {
            return "";
        }
    }

    string getSpellDescription(Spell spell) {
        if (spell != null) {
            return spell.getDescription();
        } else {
            return "";
        }
    }

    void determineTargetSelection(Spell spell, Game game) {
        if (spell == null) {
            return;
        }
        switch (spell.getTargetType()) {
            case TargetType.SINGLE_ALLY:
                determineSingleTargetQuickCast(spell, game.getAllies(side), game);
                return;
            case TargetType.SINGLE_ENEMY:
                determineSingleTargetQuickCast(spell, game.getEnemies(side), game);
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

    void showInventory(Game game) {
        int index = 0;
        foreach (Item item in inventory.getList()) {
            string nameAndCount = string.Format("{0} x {1}", item.getName(), item.getCount());
            game.getActionGrid().setButtonAttribute(new Process(item.canCast() ? nameAndCount : string.Format("{0}{1}{2}", "<color=red>", nameAndCount, "</color>"), item.getDescription(), () => {
                castAndResetStateIfSuccessful(item, game);
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

    void pushLastSpell(Spell spell) {
        lastSpellStack.Push(spell);
    }

    Spell popLastSpell() {
        return lastSpellStack.Pop();
    }

    List<Character> getTargets() {
        return targets;
    }

    Spell getTargetedSpell() {
        return targetedSpell;
    }
}
