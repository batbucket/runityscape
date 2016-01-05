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
                    "Attack", () => { selectedSpell = SpellFactory.createSpell(fightSpells[0]); setSelectionType(SelectionType.CHOOSE_TARGET); },
                    resources.ContainsKey(ResourceType.SKILL) ? "Skill" : "Spell", () => { setSelectionType(SelectionType.SPELLS); }
                    ));
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
                Debug.Assert(selectedSpell != null);
                List<Character> enemies = game.getPage().getCharacters(false);
                List<Character> allies = game.getPage().getCharacters(true);
                for (int i = 0; i < enemies.Count; i++) {
                    Character enemy = enemies[i];
                    game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(enemy.getName(), () => {
                        if (selectedSpell.canCast(this)) {
                            selectedSpell.initialize(this, enemy);
                            selectedSpell.tryCast(game);
                            this.selectionType = SelectionType.FAIM;
                        }
                    }), i);
                }
                for (int i = 0; i < allies.Count; i++) {
                    Character ally = allies[i];
                    game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(ally.getName(), () => {
                        if (selectedSpell.canCast(this)) {
                            selectedSpell.initialize(this, ally);
                            selectedSpell.tryCast(game);
                            this.selectionType = SelectionType.FAIM;
                        }
                    }), i + ALLY_CAST_OFFSET);
                }
                showBackButton(game);
                break;
        }
    }

    void showSpellsAsList(List<string> spells, Game game, bool hideFirst) {
        int startIndex = hideFirst ? 1 : 0;
        for (int i = startIndex; i < spells.Count; i++) {
            Spell spell = SpellFactory.createSpell(spells[i]);
            string descAndCost = spell.getNameAndCosts();
            string descAndCostRed = Util.color(spell.getName(), Color.red) + spell.getCosts();
            game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess(spell.canCast(this) ? descAndCost : descAndCostRed, () => {
                selectedSpell = spell;
                List<Character> allChars = new List<Character>();
                allChars.AddRange(game.getPage().getCharacters(true));
                allChars.AddRange(game.getPage().getCharacters(false));
                switch (selectedSpell.getTargetType()) {
                    case TargetType.SELF:
                        selectedSpell.initialize(this, this);
                        selectedSpell.tryCast(game);
                        break;
                    case TargetType.SINGLE:
                        setSelectionType(SelectionType.CHOOSE_TARGET);
                        break;
                    case TargetType.ALL_ALLY:
                        selectedSpell.initialize(this, game.getPage().getCharacters(true).ToArray());
                        selectedSpell.tryCast(game);
                        break;
                    case TargetType.ALL_ENEMY:
                        selectedSpell.initialize(this, game.getPage().getCharacters(false).ToArray());
                        selectedSpell.tryCast(game);
                        break;
                    case TargetType.ALL:
                        selectedSpell.initialize(this, allChars.ToArray());
                        selectedSpell.tryCast(game);
                        break;
                }
            }

            ), i);
        }
    }

    void showBackButton(Game game) {
        //this one doesn't use set() to prevent infinite looping of lastSelection
        game.getActionGrid().setButtonAttribute(ProcessFactory.createProcess("Back", () => selectionType = lastSelectionStack.Pop()), BACK_INDEX);
    }

    public override bool isDefeated(Game game) {
        throw new NotImplementedException();
    }

    public override bool isKilled(Game game) {
        throw new NotImplementedException();
    }

    public override void onDefeat(Game game) {
        throw new NotImplementedException();
    }

    public override void onKill(Game game) {
        throw new NotImplementedException();
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

    public override void react(Spell spell, Game game) {
        //throw new NotImplementedException();
    }
}
