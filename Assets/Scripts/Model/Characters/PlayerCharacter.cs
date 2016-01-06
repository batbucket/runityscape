using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class PlayerCharacter : Character {
    SelectionType selectionType;
    Stack<SelectionType> lastSelectionStack;
    Spell selectedSpell;

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

    public void display(Game game) {
        switch (selectionType) {
            case SelectionType.ACT:
                showSpellsAsList(actSpells, game, false);
                showBackButton(game);
                break;
            case SelectionType.FAIM:
                game.getActionGrid().setButtonAttributes(ActionGridFactory.createProcesses(
                    "Fight", () => setSelectionType(SelectionType.FIGHT),
                    "Act", () => setSelectionType(SelectionType.ACT),
                    "Item", () => setSelectionType(SelectionType.ITEM),
                    "Mercy", () => setSelectionType(SelectionType.MERCY)
                    ));
                game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess("Switch", () => Debug.Log("This aint a thing yet!")), BACK_INDEX);
                break;
            case SelectionType.FIGHT:
                game.getActionGrid().setButtonAttributes(ActionGridFactory.createProcesses(
                    fightSpells[0].getNameAndCosts(), () => {
                        selectedSpell = fightSpells[0];
                        List<Character> enemyList = game.getPage().getCharacters(false);
                        if (enemyList.Count == 0) {
                            //Do nothing
                        } else if (enemyList.Count == 1) {
                            selectedSpell.setTargets(enemyList[0]);
                            if (selectedSpell.tryCast(game)) {
                                resetSelection();
                            }
                        } else {
                            setSelectionType(SelectionType.CHOOSE_TARGET);
                        }
                    },

                    resources.ContainsKey(ResourceType.SKILL) ? "Skill" : "Spell", () => {
                        setSelectionType(SelectionType.SPELLS);
                    }));
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
                List<Character> enemies = game.getPage().getCharacters(false);
                List<Character> allies = game.getPage().getCharacters(true);
                switch (selectedSpell.getTargetType()) {
                    case TargetType.SINGLE_ALLY:
                        showTargets(allies, game);
                        break;
                    case TargetType.SINGLE_ENEMY:
                        showTargets(enemies, game);
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
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(spell.getNameAndCosts(), () => {
                selectedSpell = spell;
                List<Character> enemies = game.getPage().getCharacters(false);
                List<Character> allies = game.getPage().getCharacters(true);
                List<Character> allChars = new List<Character>();
                allChars.AddRange(enemies);
                allChars.AddRange(allies);
                switch (selectedSpell.getTargetType()) {
                    case TargetType.SINGLE_ALLY:
                        if (allies.Count == 0) {
                            //Do nothing
                        } else if (allies.Count == 1) {
                            selectedSpell.setTargets(allies[0]);
                        } else {
                            setSelectionType(SelectionType.CHOOSE_TARGET);
                        }
                        break;
                    case TargetType.SINGLE_ENEMY:
                        if (enemies.Count == 0) {
                            //Do nothing
                        } else if (enemies.Count == 1) {
                            selectedSpell.setTargets(enemies[0]);
                        } else {
                            setSelectionType(SelectionType.CHOOSE_TARGET);
                        }
                        break;
                    case TargetType.SELF:
                        selectedSpell.setTargets(this);
                        break;
                    case TargetType.ALL_ALLY:
                        selectedSpell.setTargets(game.getPage().getCharacters(true).ToArray());
                        break;
                    case TargetType.ALL_ENEMY:
                        selectedSpell.setTargets(game.getPage().getCharacters(false).ToArray());
                        break;
                    case TargetType.ALL:
                        selectedSpell.setTargets(allChars.ToArray());
                        break;
                }
                    if (selectedSpell.tryCast(game)) {
                        resetSelection();
                    }
                }

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

    public void setSelectionType(SelectionType selectionType) {
        pushLastSelection(this.selectionType);
        this.selectionType = selectionType;
    }

    public void pushLastSelection(SelectionType lastSelection) {
        this.lastSelectionStack.Push(lastSelection);
    }

    public void showTargets(List<Character> targets,Game game) {
        for (int i = 0; i < targets.Count; i++) {
            Character target = targets[i];
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(selectedSpell.canCast() ? target.getName() : string.Format("{0}{1}{2}", "<color=red>", target.getName(), "</color>"), () => {
                if (selectedSpell.canCast()) {
                    selectedSpell.setTargets(target);
                    selectedSpell.tryCast(game);
                    resetSelection();
                }
            }), i);
        }
    }

    public void resetSelection() {
        this.selectionType = SelectionType.FAIM;
        lastSelectionStack.Clear();
    }

    public void returnToLastSelection() {
        selectionType = lastSelectionStack.Pop();
    }
}
