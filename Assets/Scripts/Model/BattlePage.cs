using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BattlePage : Page {
    public const int CHARGE_PER_TICK = 1;

    SelectionType selectionType;
    Stack<SelectionType> lastSelectionStack;
    Stack<Spell> lastSpellStack;

    Spell targetedSpell;
    List<Character> targets;

    public const int BACK_INDEX = 7; //Last hotkey is letter 'F'
    public const int ALLY_CAST_OFFSET = 4;

    public BattlePage(string text = "", string tooltip = "", List<Character> left = null, List<Character> right = null,
        Action onFirstEnter = null, Action onEnter = null, Action onFirstExit = null, Action onExit = null,
        List<Process> actionGrid = null) : base(text, tooltip, left, right, onFirstEnter, onEnter, onFirstExit, onExit, actionGrid, null) {
        selectionType = SelectionType.FAIM;
    }

    public override void tick() {
        base.tick();
    }

    void chargeTick(List<Character> characters) {
        foreach (Character c in characters) {
            int chargeNeededToAct = (int)(120 * ((float)(leftCharacters[0].GetAttribute(AttributeType.DEXTERITY).False)) / c.GetAttribute(AttributeType.DEXTERITY).False);
            c.GetResource(ResourceType.CHARGE).True = chargeNeededToAct;
            c.act(CHARGE_PER_TICK, null);
        }
    }

    void display(Character character) {
        switch (selectionType) {
            case SelectionType.ACT:
                setTooltip(string.Format("What ACTION will {0} do?", character.Name));
                showSpellsAsList(character.Acts, false);
                showBackButton();
                break;
            case SelectionType.FAIM:
                setTooltip(string.Format("What will {0} do?", character.getName().ToUpper()));
                getActionGrid().setButtonAttributes(
                    new Process(getSpellNameAndInfo(character.getFight()[0]), getSpellDescription(fightSpells[0]), () => {
                        determineTargetSelection(fightSpells[0], game);
                    }),
                    new Process(getSpellNameAndInfo(peekLastSpell()), getSpellDescription(peekLastSpell()), () => {
                        determineTargetSelection(peekLastSpell(), game);
                    }),
                    new Process(getCorrectSpellListName(), string.Format("Perform a {0}.", getCorrectSpellListName().ToUpper()), () => {
                        setSelectionType(SelectionType.SPELLS);
                    }),
                    new Process("Act", "Perform an ACTION.", () => setSelectionType(SelectionType.ACT)),
                    new Process("Item", "USE or EQUIP an ITEM.", () => setSelectionType(SelectionType.ITEM)),
                    new Process("Mercy", "Show an enemy MERCY.", () => setSelectionType(SelectionType.MERCY)
                    ));
                game.getActionGrid().setButtonAttribute(new Process("Switch", "Change control to another unit.", () => Debug.Log("This aint a thing yet!")), BACK_INDEX);
                break;
            case SelectionType.ITEM:
                game.setTooltip(string.Format("What ITEM will {0} use?", name.ToUpper()));
                showSpellsAsList(inventory.getList().Cast<Spell>().ToList(), game, false);
                showBackButton(game);
                break;
            case SelectionType.MERCY:
                game.setTooltip(string.Format("What MERCY will {0} use?", name));
                showSpellsAsList(mercySpells, game, false);
                showBackButton(game);
                break;
            case SelectionType.NONE:
                game.getActionGrid().clearAllButtonAttributes();
                break;
            case SelectionType.SPELLS:
                game.setTooltip(string.Format("What {1} will {0} use?", name.ToUpper(), getCorrectSpellListName().ToUpper()));
                showSpellsAsList(fightSpells, game, true);
                showBackButton(game);
                break;
            case SelectionType.CHOOSE_TARGET:
                game.setTooltip(string.Format("{0} > {1} >", name.ToUpper(), targetedSpell.getName().ToUpper()));
                showTargetsAsList(targets, game);
                showBackButton(game);
                break;
        }
    }

    void showSpellsAsList(List<Spell> spells) {
        for (int i = 0; i < spells.Count; i++) {
            Spell spell = spells[i];
            setAction(new Process(getSpellNameAndInfo(spell), getSpellDescription(spell), () => determineTargetSelection(spell)), i);
        }
    }

    void showTargetsAsList(List<Character> targets, Game game) {
        for (int i = 0; i < targets.Count; i++) {
            Character target = targets[i];
            game.getActionGrid().SetButtonAttribute(new Process(
                string.Format("{0}{1}{2}", targetedSpell.canCast() ? "" : "<color=red>", target.getName(), targetedSpell.canCast() ? "" : "</color>"),
                string.Format("{0} > {1} > {2}", name.ToUpper(), targetedSpell.getName().ToUpper(), target.getName().ToUpper()), () => {
                    targetedSpell.setTarget(target);
                    castAndResetStateIfSuccessful(targetedSpell, game);
                }
                ), i);
        }
    }

    void showBackButton(Game game) {
        //this one doesn't use set() to prevent infinite looping of lastSelection
        game.getActionGrid().SetButtonAttribute(new Process("Back", "Go back to the previous selection.", () => returnToLastSelection()), BACK_INDEX);
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
        spell.TryCast();
        talk(spell.getCastText(), game);
        if (spell.getResult() != SpellResult.CANT_CAST) {
            getReactions(spell, game);
            resetSelection();
            if (spell != fightSpells[0]) {
                pushLastSpell(spell);
            }
            if (spell is Item && ((Item)spell).getCount() <= 0) {
                while (lastSpellStack.Count() > 0 && spell.Equals(lastSpellStack.Peek())) {
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

    string getSpellName(Spell spell) {
        if (spell != null) {
            return spell.getName();
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
            game.getActionGrid().SetButtonAttribute(new Process(item.canCast() ? nameAndCount : string.Format("{0}{1}{2}", "<color=red>", nameAndCount, "</color>"), item.getDescription(), () => {
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

    Spell peekLastSpell() {
        return lastSpellStack.Count == 0 ? null : lastSpellStack.Peek();
    }

    List<Character> getTargets() {
        return targets;
    }

    Spell getTargetedSpell() {
        return targetedSpell;
    }
}
