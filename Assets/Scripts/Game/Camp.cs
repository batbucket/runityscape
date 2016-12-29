using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Camp : ReadPage {

    public Party party;
    public PlayerCharacter pc;
    public Inventory PartyItems;

    private int days;
    public TimeType time;

    public ExplorePage explore;
    public PlacesPage places;

    public CharactersPage character;
    public ItemManagePage itemMan;

    public Camp(PlayerCharacter pc)
        : base(
            "",
            "",
            "Camp",
            false
            ) {
        this.party = new Party(pc);

        this.pc = pc;
        this.PartyItems = pc.Items;
        Game.Instance.Gold.Inventory = PartyItems;

        OnFirstEnterAction += () => {
            CreateCamp();
        };
        OnEnterAction += () => {
            foreach (Character c in party) {
                c.State = CharacterState.DEFEAT;
            }
        };

        this.days = 0;
        this.time = TimeType.DAWN;
        Game.Instance.Time.Time = Util.Color(time.Name, time.Color);
        Game.Instance.Time.Day = days;
    }

    private void CreateCamp() {

        this.explore = new ExplorePage(this, party);
        this.places = new PlacesPage(this, party);

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
            Game.Instance.Time.Time = Util.Color(time.Name, time.Color);
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party rests.")));
        }, () => time != TimeType.NIGHT);
    }

    private const float SLEEP_RESTORE_PERCENT = 1f;
    private Process Sleep() {
        return new Process("Sleep", "End the day.", () => {
            RestoreResources(SLEEP_RESTORE_PERCENT);
            time = TimeType.DAWN;
            days++;
            Game.Instance.Time.Time = Util.Color(time.Name, time.Color);
            Game.Instance.Time.Day = days;
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
