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
    private int hours;
    private const int NEW_DAY_HOURS = 5;

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
        this.hours = NEW_DAY_HOURS;
    }

    private void CreateCamp() {
        Hub = PageUtil.Rp(
            text: "You awaken in a grassy green field.",
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
                        new Process("Rest", "Take a short rest.", () => {
                            Game.Instance.Time.Time = "" + --hours;
                        }, () => hours > 0),
                        new Process("Sleep", "End the day.", () => {
                            Game.Instance.Time.Day = ++days;
                            hours = NEW_DAY_HOURS;
                            Game.Instance.Time.Time = "" + hours;
                        })
            },
            onEnter: () => {
                Game.Instance.Time.IsTimeEnabled = true;
            }
            );
    }

    public override void Init() {

    }
}
