using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Camp : ReadPage {

    public int Gold {
        get {
            return inventory.Gold;
        }
    }

    public int Time {
        get {
            return flags.Ints[Flag.TIME];
        }
    }

    public int Day {
        get {
            return flags.Ints[Flag.DAYS];
        }
    }

    private Party party;
    private PlayerCharacter pc;
    private Inventory inventory;

    private ExplorePage explore;
    private PlacesPage places;

    private CharactersPage character;
    private ItemManagePage itemMan;

    private Flags flags;

    private bool IsNight {
        get {
            return flags.Ints[Flag.TIME] == TimeType.NIGHT.Index;
        }
    }

    public Camp(PlayerCharacter pc)
        : base(
            "",
            "",
            "Camp",
            false
            ) {
        this.party = new Party(pc);
        this.flags = new Flags();

        this.pc = pc;
        this.inventory = pc.Inventory;

        OnFirstEnterAction += () => {
            CreateCamp();
        };
        OnEnterAction += () => {
            Game.Instance.Other.Camp = this;
            foreach (Character c in party) {
                if (c.State == CharacterState.KILLED) {
                    c.AddToResource(ResourceType.HEALTH, false, 1);
                }
                if (c.State == CharacterState.DEFEAT) {
                    c.CancelBuffs();
                }
                c.State = CharacterState.NORMAL;
            }
        };

        flags.Ints[Flag.DAYS] = 0;
        flags.Ints[Flag.TIME] = 0;
    }

    private void CreateCamp() {

        this.explore = new ExplorePage(flags, this, party);
        this.places = new PlacesPage(flags, this, party);

        this.character = new CharactersPage(this, party);
        this.itemMan = new ItemManagePage(this, party);

        this.ActionGrid = new IButtonable[] {
                new Process("Explore", "Explore a location.", () => Game.Instance.CurrentPage = explore,
                () => !IsNight),
                new Process("Places", "Visit a place.", () => Game.Instance.CurrentPage = places,
                () => !IsNight),
                character,
                itemMan,

                null,
                null,
                Rest(),
                Sleep(),

                null,
                null,
                null,
                null
        };
        this.LeftCharacters = party.Members;
        Game.Instance.Time.IsTimeEnabled = true;
        Game.Instance.Time.IsDayEnabled = true;
    }

    private const float REST_RESTORE_PERCENT = 0.4f;
    private Process Rest() {
        return new Process("Rest", "Rest until the next part of the day.", () => {
            RestoreResources(REST_RESTORE_PERCENT);
            flags.Ints[Flag.TIME]++;
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party rests.")));
        }, () => !IsNight);
    }

    private const int DAYS_DISPLAY_LIMIT = 9999;
    private const float SLEEP_RESTORE_PERCENT = 0.8f;
    private Process Sleep() {
        return new Process("Sleep", "End the day.", () => {
            RestoreResources(SLEEP_RESTORE_PERCENT);
            flags.Ints[Flag.TIME] = 0;

            // Overflow guard against sleep spammers
            if (flags.Ints[Flag.DAYS] < int.MaxValue) {
                flags.Ints[Flag.DAYS]++;
            }

            Game.Instance.CurrentPage = this;
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party sleeps.")));
        }, () => IsNight);
    }

    private void RestoreResources(float percent) {
        foreach (Character c in party.Members) {
            c.CancelBuffs();
            foreach (ResourceType r in ResourceType.RESTORED_RESOURCES) {
                float missing = c.GetResourceCount(r, true) - c.GetResourceCount(r, false);
                c.AddToResource(r, false, missing * percent, true);
            }
        }
    }
}
