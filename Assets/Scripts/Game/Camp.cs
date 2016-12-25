using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Camp : Area {
    public Page Hub;

    public IList<ReadPage> VisitablePlaces;
    public IList<PageGenerator> ExplorablePlaces;

    public Party party;
    public PlayerCharacter pc;
    public Inventory PartyItems;

    private int days;
    public TimeType time;

    public Camp(PlayerCharacter pc) {
        this.party = new Party(pc);
        this.VisitablePlaces = new List<ReadPage>();
        this.ExplorablePlaces = new List<PageGenerator>();
        ExplorablePlaces.Add(
            PageUtil.Pg("Ruins", "The dilapidated remains of some civilization.",
                new Encounter(
                    () => PageUtil.Bp(Hub, "Ruins", party, new Regenerator()), () => 1),
                new Encounter(
                    () => PageUtil.Bp(Hub, "Ruins", party, new Lasher()), () => 1)
            ));

        this.pc = pc;
        this.PartyItems = pc.Items;

        CreateCamp();

        this.days = 0;
        this.time = TimeType.DAWN;
        Game.Instance.Time.Time = Util.Color(time.Name, time.Color);
        Game.Instance.Time.Day = days;
    }

    private void CreateCamp() {
        Hub = PageUtil.Rp(
            text: "",
            location: "Camp",
            party: party,
            processes: new Process[] {
                        new Process("Explore", "Explore the world.", () => Page = PageUtil.GenerateExplorePage(party, ExplorablePlaces, Hub)),
                        new Process("Places", "Visit a specific place.", () => Page = PageUtil.GeneratePlacesPage(party, VisitablePlaces, Hub)),
                        new Process(),
                        new Process(),

                        new Process("Character", "View information about a party character.", () => Page = PageUtil.GenerateCharactersPage(party, Hub)),
                        new Process(),
                        new Process("Items", "Manage items.", () => Page = PageUtil.GenerateItemManagementPage(party, PartyItems, Hub)),
                        new Process("+Exp", "Boosted", () => pc.AddToResource(ResourceType.EXPERIENCE, false, 3)),

                        new Process(),
                        new Process(),
                        new Process("Rest", "Rest until the next part of the day.", () => {
                            time = time.Next;
                            Game.Instance.Time.Time = Util.Color(time.Name, time.Color);

                            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party rests.", time.Name.ToLower())));
                            Game.Instance.TextBoxes.AddTextBox(new TextBox(time.Description));
                        }),
                        new Process("Sleep", "End the day.", () => {
                            time = TimeType.DAWN;
                            days++;
                            Game.Instance.Time.Time = Util.Color(time.Name, time.Color);
                            Game.Instance.Time.Day = days;

                            Game.Instance.CurrentPage = Hub;

                            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party sleeps.", days)));
                            Game.Instance.TextBoxes.AddTextBox(new TextBox(time.Description));
                        })
            },
            onEnter: () => {
                Game.Instance.Time.IsTimeEnabled = true;
                Game.Instance.Time.IsDayEnabled = true;
            }
            );
    }

    public override void Init() {

    }
}
