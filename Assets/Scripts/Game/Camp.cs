using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Camp : ReadPage {

    public Party party;
    public PlayerCharacter pc;
    public Inventory PartyItems;

    public int days;
    public TimeType time;

    public ExplorePage explore;
    public PlacesPage places;

    public CharactersPage character;
    public ItemManagePage itemMan;

    public Flags Flags;

    public Camp(PlayerCharacter pc)
        : base(
            "",
            "",
            "Camp",
            false
            ) {
        this.party = new Party(pc);
        this.Flags = new Flags();

        this.pc = pc;
        this.PartyItems = pc.Items;

        OnFirstEnterAction += () => {
            CreateCamp();
        };
        OnEnterAction += () => {
            Game.Instance.Other.Camp = this;
            foreach (Character c in party) {
                if (c.State == CharacterState.KILLED) {
                    c.AddToResource(ResourceType.HEALTH, false, 1);
                }
                c.State = CharacterState.NORMAL;
            }
        };

        this.days = 0;
        this.time = TimeType.DAWN;
    }

    private void CreateCamp() {

        this.explore = new ExplorePage(Flags, this, party);
        this.places = new PlacesPage(Flags, this, party);

        this.character = new CharactersPage(this, party);
        this.itemMan = new ItemManagePage(this, party);

        this.ActionGrid = new IButtonable[] {
                explore,
                places,
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
            time = time.Next;
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party rests.")));
        }, () => time != TimeType.NIGHT);
    }

    private const float SLEEP_RESTORE_PERCENT = 1f;
    private Process Sleep() {
        return new Process("Sleep", "End the day.", () => {
            RestoreResources(SLEEP_RESTORE_PERCENT);
            time = TimeType.DAWN;
            days++;

            Game.Instance.CurrentPage = this;
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party sleeps.")));
        });
    }

    private void RestoreResources(float percent) {
        foreach (Character c in party.Members) {
            c.CancelBuffs();
            foreach (ResourceType r in ResourceType.RESTORED_RESOURCES) {
                c.AddToResource(r, false, c.GetResourceCount(r, true) * percent, true);
            }
        }
    }
}
