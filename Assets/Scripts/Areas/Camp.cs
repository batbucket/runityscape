using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Model.BattlePage;

public class Camp : Area {
    public Page Hub;

    public IList<ReadPage> VisitablePlaces;
    public IList<PageGenerator> ExplorablePlaces;

    public List<Character> party;
    public PlayerCharacter pc;
    public Inventory PartyItems;

    private int days;
    private int hours;
    private const int NEW_DAY_HOURS = 5;

    public Camp(PlayerCharacter pc) {
        this.party = new List<Character>();
        this.VisitablePlaces = new List<ReadPage>();
        this.ExplorablePlaces = new List<PageGenerator>();
        ExplorablePlaces.Add(
            Pg("Ruins", "The dilapidated remains of some civilization.",
                new Encounter(
                    () => Bp("Ruins", new Regenerator()), () => 1),
                new Encounter(
                    () => Bp("Ruins", new Lasher()), () => 1)
            ));

        this.pc = pc;
        this.PartyItems = pc.Items;
        party.Add(pc);

        CreateCamp();

        this.days = 0;
        this.hours = NEW_DAY_HOURS;
    }

    private void CreateCamp() {
        Hub = Rp(
            text: "You awaken in a grassy green field.",
            location: "Camp",
            processes: new Process[] {
                        new Process("Explore", "Explore the world.", () => Page = GenerateExplorePage()),
                        new Process("Places", "Visit a specific place.", () => Page = GeneratePlacesPage()),
                        new Process(),
                        new Process(),

                        new Process("Character", "View information about a party character.", () => Page = GenerateCharactersPage()),
                        new Process(),
                        new Process("Items", "Manage items.", () => Page = GenerateItemManagementPage()),
                        new Process("+Exp", "Boosted", () => pc.AddToResource(ResourceType.EXPERIENCE, false, 3)),

                        new Process(),
                        new Process(),
                        new Process("Rest", "Take a short rest.", () => {
                            Game.Time.Time = --hours;
                        }, () => hours > 0),
                        new Process("Sleep", "End the day.", () => {
                            Game.Time.Day = ++days;
                            hours = NEW_DAY_HOURS;
                            Game.Time.Time = hours;
                        })
            },
            onEnter: () => {
                Game.Time.IsEnabled = true;
            }
            );
    }

    private ReadPage GenerateExplorePage() {
        return Rp(
           text: "Where will you explore?",
           location: "Camp",
           tooltip: "",
           processes: GenerateExploreButtons(ExplorablePlaces)
           );
    }

    private IList<IButtonable> GenerateExploreButtons(IList<PageGenerator> exploreablePlaces) {
        IButtonable[] buttonable = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
        for (int i = 0; i < exploreablePlaces.Count; i++) {
            buttonable[i] = exploreablePlaces[i];
        }
        buttonable[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to Camp.", () => Page = Hub);
        return buttonable;
    }

    private ReadPage GeneratePlacesPage() {
        return Rp(
           text: "Where will you go?",
           location: "Camp",
           tooltip: "",
           processes: GeneratePlacesProcesses(VisitablePlaces),
           onEnter: () => {
           }
           );
    }

    private IList<IButtonable> GeneratePlacesProcesses(IList<ReadPage> places) {
        IButtonable[] buttonable = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        for (int i = 0; i < places.Count; i++) {
            buttonable[i] = new Process(places[i].Location, "Visit this area.", () => Page = places[i]);
        }
        buttonable[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to Camp.", () => Page = Hub);
        return buttonable;
    }

    private ReadPage GenerateCharactersPage() {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Character myc in party) {
            Character c = myc;
            processes[index++] = new Process(c.DisplayName, action: () => Page = GenerateCharacterPage(c));
        }
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to camp.", () => Page = Hub);

        return Rp(
            "Which character would you like to view?",
            "Camp",
            processes
            );
    }

    private ReadPage GenerateCharacterPage(Character c) {
        return new ReadPage(
            StatText(c),
            location: c.DisplayName,
            mainCharacter: c,
            left: new Character[] { c },
            buttonables: new Process[] {
                new Process(
                    c.GetResourceCount(ResourceType.EXPERIENCE, false) < c.GetResourceCount(ResourceType.EXPERIENCE, true) ?
                    string.Format("{0}/{1} XP", c.GetResourceCount(ResourceType.EXPERIENCE, false), c.GetResourceCount(ResourceType.EXPERIENCE, true))
                    : "Level up!",
                    "You will be able to level up when you reach the experience cap.",
                    () => {
                        c.AddToResource(ResourceType.EXPERIENCE, false, -c.GetResourceCount(ResourceType.EXPERIENCE, true));
                        c.AddToAttribute(AttributeType.LEVEL, false, 1);
                        Page = LevelUpPage(c);
                    },
                    () => c.GetResourceCount(ResourceType.EXPERIENCE, false) >= c.GetResourceCount(ResourceType.EXPERIENCE, true)
                    ),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process("Back", "", () => Page = Hub)
            }
            );
    }

    private static string StatText(Character target) {
        List<string> a = new List<string>();
        foreach (KeyValuePair<AttributeType, Attribute> pair in target.Attributes) {
            if (pair.Key.IsAssignable) {
                a.Add((string.Format("{1}/{2} <b>{0}</b>: {3}", Util.Color(pair.Key.Name, pair.Key.Color), pair.Value.False, pair.Value.True, pair.Key.ShortDescription)));
            }
        }

        List<string> r = new List<string>();
        foreach (KeyValuePair<ResourceType, Resource> pair in target.Resources) {
            if (pair.Value.IsVisible) {
                r.Add(string.Format("{1}/{2} <b>{0}</b>: {3}", Util.Color(pair.Key.Name, pair.Key.FillColor), pair.Value.False, pair.Value.True, pair.Key.Description));
            }
        }
        return string.Format(
            "Level {1} {0}\n{2}{3}{4}",
            target.DisplayName,
            target.Level.False,
            string.IsNullOrEmpty(target.CheckText) ? "" : string.Format("\n{0}", target.CheckText),
            "\nAttributes:\n" + string.Join("\n", a.ToArray()),
            "\nResources:\n" + string.Join("\n", r.ToArray()),
            target.Equipment.Count <= 0 ? "" : string.Format("\n{0}", target.Equipment.ToString())
            );
    }

    private const int ATT_INCREASE_AMOUNT = 1;
    private ReadPage LevelUpPage(Character c) {
        ReadPage perkAssign =
        new ReadPage(
            "Select a perk.",
            "",
            string.Format("{0}'s Level {1} → Level {2}", c.DisplayName, c.Level.False - 1, c.Level.False),
            mainCharacter: pc,
            left: new Character[] { c },
            right: new Character[] { },
            buttonables:
            new Process[] {
                new Process("None", action: () => Page = GenerateCharacterPage(c))
            }
            );

        IList<IButtonable> attributes = new List<IButtonable>();
        foreach (KeyValuePair<AttributeType, Attribute> myPair in c.Attributes) {
            KeyValuePair<AttributeType, Attribute> pair = myPair;
            if (pair.Key.IsAssignable) {
                attributes.Add(new Process(
                    string.Format(Util.Color(pair.Key.Name, pair.Key.Color)),
                    string.Format("Increase {0} by {1}.\n{0}: {2}", pair.Key.Name, ATT_INCREASE_AMOUNT, pair.Key.PrimaryDescription),
                    () => {
                        c.AddToAttribute(pair.Key, true, ATT_INCREASE_AMOUNT, false);
                        c.AddToAttribute(pair.Key, false, ATT_INCREASE_AMOUNT, false);
                        Game.Instance.Sound.Play("Blip_0");
                        Page = perkAssign;
                    }
                    ));
            }
        }

        ReadPage attributeAssign =
            new ReadPage(
                "Select an attribute to increase.",
                "",
                string.Format("{0}'s Level {1} → Level {2}", c.DisplayName, c.Level.False - 1, c.Level.False),
                false,
                mainCharacter: pc,
                left: new Character[] { c },
                right: new Character[] { },
                buttonables: attributes
                );

        return attributeAssign;
    }

    private ReadPage GenerateItemManagementPage() {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Character myC in party) {
            Character c = myC;
            processes[index++] = new Process(c.DisplayName, string.Format("View and unequip {0}'s equipment.", c.DisplayName), () => Page = GenerateCharacterEquipPage(c));
        }

        processes[ActionGridView.TOTAL_BUTTON_COUNT - 2] = new Process("Inventory", "Use items in inventory.", () => Page = GenerateInventoryPage(PartyItems));
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to camp.", () => Page = Hub);

        return Rp(
            "",
            "Item and Equipment Manager",
            processes,
            "Unequip a specific character's items, or use items in inventory."
            );
    }

    private ReadPage GenerateCharacterEquipPage(Character c) {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (EquippableItem myE in c.Equipment) {
            EquippableItem e = myE;
            processes[index++] = e != null ? CreateUnequipProcess(c, e) : new Process();
        }
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to camp.", () => Page = GenerateItemManagementPage());

        return new ReadPage(
            "",
            string.Format("View and unequip equipment from {0}.", c.DisplayName),
            string.Format("{0}'s Equipment", c.DisplayName),
            mainCharacter: pc,
            left: new Character[] { c },
            buttonables: processes
            );
    }


    private Process CreateUnequipProcess(Character caster, EquippableItem item) {
        Process p = new Process(Util.Color(item.Name, Color.yellow), item.Description, () => {
            Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.", caster.DisplayName, item.Name), Color.white, TextEffect.FADE_IN));
            item.UnequipItemInSlot(caster);
            Page = GenerateCharacterEquipPage(caster);
        });
        return p;
    }

    private ReadPage GenerateInventoryPage(Inventory items) {
        return Rp(
            "",
            "Inventory",
            GenerateInventoryProcesses(items),
            "Use and equip items on particular characters."
            );
    }

    private IList<IButtonable> GenerateInventoryProcesses(Inventory items) {
        IButtonable[] itemButtons = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Item myI in items) {
            Item i = myI;
            itemButtons[index++] = (new Process(string.Format("{0}", i.Name), i.Description, () => Page = GenerateUseItemOnCharacterPage(i)));
        }
        itemButtons[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to Item and Equipment Manager", () => Page = GenerateItemManagementPage());
        return itemButtons;
    }

    private ReadPage GenerateUseItemOnCharacterPage(Item item) {
        return Rp(
            "",
            "Items",
            GenerateUseItemProcesses(item, party),
            string.Format("Who will use {0}?\n{1}", item.Name, item.Description)
            );
    }

    private IList<IButtonable> GenerateUseItemProcesses(Item item, IList<Character> characters) {
        IButtonable[] useButtons = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Character myC in party) {
            Character c = myC;
            useButtons[index++] = (new Process(c.DisplayName, string.Format("{0} will use {1}.\n{2}", c.DisplayName, item.Name, item.Description),
                () => {
                    item.Cast(c, c);
                    Page = GenerateInventoryPage(PartyItems);
                }
                ));
        }
        useButtons[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Use a different item.", () => Page = GenerateInventoryPage(PartyItems));
        return useButtons;
    }

    private PageGenerator Pg(
        string name,
        string description,
        params Encounter[] encounters
        ) {
        return new PageGenerator(p => { Page = p; }, name, description, encounters);
    }

    private ReadPage Rp(
    string text,
    string location,
    IList<IButtonable> processes,
    string tooltip = "",
    IList<Character> right = null,
    Action onEnter = null,
    Action onExit = null,
    Action onTick = null) {
        right = right ?? new Character[0];
        return new ReadPage(
            text,
            tooltip,
            location,
            mainCharacter: pc,
            left: party,
            right: right.Where(c => c.State != CharacterState.KILLED).ToArray(),
            onEnter: onEnter,
            onExit: onExit,
            onTick: onTick,
            buttonables: processes);
    }

    private BattlePage Bp(string text,
    string location,
    string musicLoc,
    IList<Character> right,
    Page victory = null,
    Page flee = null,
    Action onEnter = null,
    Action onTick = null) {
        return new BattlePage(
            text: text,
            location: location,
            musicLoc: musicLoc,
            mainCharacter: pc,
            left: party,
            right: right,
            onVictory: () => this.Page.ActionGrid = new Process[] { new Process("Continue", action: () => Page = victory) },
            onEnter: () => {
                if (onEnter != null) {
                    onEnter.Invoke();
                }
                party.ForEach(c => c.Mercies.Add(new Flee(flee)));
            },
            onExit: () => {
                party.ForEach(c => c.Mercies.Remove(new Flee(flee)));
            },
            onTick: onTick
            );
    }

    private BattlePage Bp(
    string location,
    params Character[] right) {
        return new BattlePage(
            text: "",
            location: location,
            musicLoc: "Hero Immortal",
            mainCharacter: pc,
            left: party,
            right: right,
            onVictory: () => this.Page.ActionGrid = new Process[] { new Process("Continue", action: () => Page = Hub) },
            onEnter: () => {
                party.ForEach(c => c.Mercies.Add(new Flee(Hub)));
            },
            onExit: () => {
                party.ForEach(c => c.Mercies.Remove(new Flee(Hub)));
            });
    }

    private BattlePage Bp(string text,
    string location,
    string musicLoc,
    IList<Character> right,
    Page victory,
    Action onEnter =
    null,
    Action onTick = null,
    Func<bool> victoryCondition = null) {
        return new BattlePage(
            text: text,
            location: location,
            musicLoc: musicLoc,
            mainCharacter: pc,
            left: party,
            right: right,
            isVictory: victoryCondition,
            onEnter: () => {
                if (onEnter != null) {
                    onEnter.Invoke();
                }
            },
            onVictory: () => this.Page.ActionGrid = new Process[] { new Process("Continue", action: () => Page = victory) },
            onTick: onTick
            );
    }

    public override void Init() {

    }
}
