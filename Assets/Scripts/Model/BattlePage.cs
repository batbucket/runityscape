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

    public override void Tick() {
        base.Tick();
    }

    void ChargeCharacters(List<Character> characters) {
        foreach (Character c in characters) {
            int chargeNeededToAct = (int)(120 * ((float)(leftCharacters[0].GetAttribute(AttributeType.DEXTERITY).False)) / c.GetAttribute(AttributeType.DEXTERITY).False);
            c.GetResource(ResourceType.CHARGE).True = chargeNeededToAct;
            c.Act(CHARGE_PER_TICK, null);
        }
    }

    void Display(Character character) {
        switch (selectionType) {
            case SelectionType.ACT:
                SetTooltip(string.Format("What ACTION will {0} do?", character.Name));
                showSpellsAsList(character.Acts, false);
                showBackButton();
                break;
            case SelectionType.FAIM:
                SetTooltip(string.Format("What will {0} do?", character.getName().ToUpper()));
                getActionGrid().setButtonAttributes(
                    new Process(getSpellNameAndInfo(character.getFight()[0]), getSpellDescription(fightSpells[0]), () => {
                        determineTargetSelection(fightSpells[0], game);
                    }),
                    new Process(GetSpellNameAndInfo(PeekLastSpell()), GetSpellDescription(PeekLastSpell()), () => {
                        determineTargetSelection(PeekLastSpell(), game);
                    }),
                    new Process(GetCorrectSpellListName(), string.Format("Perform a {0}.", GetCorrectSpellListName().ToUpper()), () => {
                        SetSelectionType(SelectionType.SPELLS);
                    }),
                    new Process("Act", "Perform an ACTION.", () => SetSelectionType(SelectionType.ACT)),
                    new Process("Item", "USE or EQUIP an ITEM.", () => SetSelectionType(SelectionType.ITEM)),
                    new Process("Mercy", "Show an enemy MERCY.", () => SetSelectionType(SelectionType.MERCY)
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
                game.setTooltip(string.Format("What {1} will {0} use?", name.ToUpper(), GetCorrectSpellListName().ToUpper()));
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

    void ShowSpellsAsList(List<Spell> spells) {
        for (int i = 0; i < spells.Count; i++) {
            Spell spell = spells[i];
            SetAction(new Process(GetSpellNameAndInfo(spell), GetSpellDescription(spell), () => determineTargetSelection(spell)), i);
        }
    }

    void ShowTargetsAsList(List<Character> targets, Game game) {
        for (int i = 0; i < targets.Count; i++) {
            Character target = targets[i];
            game.GetActionGrid().SetButtonAttribute(new Process(
                string.Format("{0}{1}{2}", targetedSpell.canCast() ? "" : "<color=red>", target.getName(), targetedSpell.canCast() ? "" : "</color>"),
                string.Format("{0} > {1} > {2}", name.ToUpper(), targetedSpell.getName().ToUpper(), target.getName().ToUpper()), () => {
                    targetedSpell.setTarget(target);
                    CastAndResetStateIfSuccessful(targetedSpell, game);
                }
                ), i);
        }
    }

    void ShowBackButton(Game game) {
        //this one doesn't use set() to prevent infinite looping of lastSelection
        game.GetActionGrid().SetButtonAttribute(new Process("Back", "Go back to the previous selection.", () => ReturnToLastSelection()), BACK_INDEX);
    }

    void SetSelectionType(SelectionType selectionType) {
        PushLastSelection(this.selectionType);
        this.selectionType = selectionType;
    }

    void PushLastSelection(SelectionType lastSelection) {
        this.lastSelectionStack.Push(lastSelection);
    }

    void ResetSelection() {
        this.selectionType = SelectionType.FAIM;
        lastSelectionStack.Clear();
    }

    void ReturnToLastSelection() {
        selectionType = lastSelectionStack.Pop();
    }

    void CastAndResetStateIfSuccessful(Spell spell, Game game) {
        spell.TryCast();
        talk(spell.getCastText(), game);
        if (spell.getResult() != SpellResult.CANT_CAST) {
            getReactions(spell, game);
            ResetSelection();
            if (spell != fightSpells[0]) {
                PushLastSpell(spell);
            }
            if (spell is Item && ((Item)spell).getCount() <= 0) {
                while (lastSpellStack.Count() > 0 && spell.Equals(lastSpellStack.Peek())) {
                    lastSpellStack.Pop();
                }
            }
        }
    }

    string GetSpellNameAndInfo(Spell spell) {
        if (spell != null) {
            return spell.getNameAndInfo();
        } else {
            return "";
        }
    }

    string GetSpellDescription(Spell spell) {
        if (spell != null) {
            return spell.getDescription();
        } else {
            return "";
        }
    }

    string GetSpellName(Spell spell) {
        if (spell != null) {
            return spell.getName();
        } else {
            return "";
        }
    }

    void DetermineTargetSelection(Spell spell, Game game) {
        if (spell == null) {
            return;
        }
        switch (spell.getTargetType()) {
            case TargetType.SINGLE_ALLY:
                DetermineSingleTargetQuickCast(spell, game.getAllies(side), game);
                return;
            case TargetType.SINGLE_ENEMY:
                DetermineSingleTargetQuickCast(spell, game.getEnemies(side), game);
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
                spell.setTargets(game.GetAll());
                break;
        }
        CastAndResetStateIfSuccessful(spell, game);
    }

    void DetermineSingleTargetQuickCast(Spell spell, List<Character> targets, Game game) {
        if (targets.Count == 0) {
            // Do nothing
        } else if (targets.Count == 1) {
            spell.setTarget(targets[0]);
            CastAndResetStateIfSuccessful(spell, game);
        } else {
            SetTargetedSpell(spell);
            SetTargets(targets);
            SetSelectionType(SelectionType.CHOOSE_TARGET);
        }
    }

    void DisplayInventory(Game game) {
        int index = 0;
        foreach (Item item in inventory.getList()) {
            string nameAndCount = string.Format("{0} x {1}", item.getName(), item.getCount());
            game.GetActionGrid().SetButtonAttribute(new Process(item.canCast() ? nameAndCount : string.Format("{0}{1}{2}", "<color=red>", nameAndCount, "</color>"), item.getDescription(), () => {
                CastAndResetStateIfSuccessful(item, game);
            }), index++);
        }
    }

    string GetCorrectSpellListName() {
        return resources.ContainsKey(ResourceType.SKILL) ? "Skill" : "Spell";
    }

    void SetTargetedSpell(Spell spell) {
        targetedSpell = spell;
    }

    void SetTargets(List<Character> targets) {
        this.targets = targets;
    }

    void PushLastSpell(Spell spell) {
        lastSpellStack.Push(spell);
    }

    Spell PopLastSpell() {
        return lastSpellStack.Pop();
    }

    Spell PeekLastSpell() {
        return lastSpellStack.Count == 0 ? null : lastSpellStack.Peek();
    }

    List<Character> GetTargets() {
        return targets;
    }

    Spell GetTargetedSpell() {
        return targetedSpell;
    }
}
