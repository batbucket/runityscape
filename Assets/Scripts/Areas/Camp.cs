using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Model.BattlePage;

public class Camp : Area {
    public Page Hub;
    public Page Places;
    public Page Character;
    public Page Inventory;
    public Page Explore;
    public Page Stash;

    public IList<ReadPage> VisitablePlaces;

    public IList<Character> party;
    public PlayerCharacter pc;

    public Camp(PlayerCharacter pc) {
        this.party = new List<Character>();
        this.VisitablePlaces = new List<ReadPage>();
        this.pc = pc;
        party.Add(pc);

        CreateCamp();
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
                        new Process("Inventory", "Manage items and equipment."),
                        new Process("+Exp", "Boosted", () => pc.AddToResource(ResourceType.EXPERIENCE, false, 3)),

                        new Process(),
                        new Process(),
                        new Process("Rest", "Take a short rest."),
                        new Process("Sleep", "End the day.")
            },
            onEnter: () => {
            }
            );
    }

    // TODO
    private ReadPage GenerateExplorePage() {
        return Rp(
           text: "Where will you explore?",
           location: "Camp",
           tooltip: "",
           processes: GeneratePlacesProcesses(VisitablePlaces),
           onEnter: () => {
           }
           );
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

    private IList<Process> GeneratePlacesProcesses(IList<ReadPage> places) {
        IList<Process> processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        for (int i = 0; i < places.Count; i++) {
            processes[i] = new Process(places[i].Location, "Visit this area.", () => Page = places[i]);
        }
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to Camp.", () => Page = Hub);
        return processes;
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
            mainCharacter: c,
            left: new Character[] { c },
            processes: new Process[] {
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
            "Camp",
            mainCharacter: pc,
            left: new Character[] { c },
            right: new Character[] { },
            processes:
            new Process[] {
                        new Process("None", action: () => Page = GenerateCharacterPage(c))
            }
            );

        IList<Process> attributes = new List<Process>();
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
                "Camp",
                false,
                mainCharacter: pc,
                left: new Character[] { c },
                right: new Character[] { },
                processes: attributes
                );

        return attributeAssign;
    }

    private ReadPage GenerateInventoryPage() {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Character myC in party) {
            Character c = myC;
            processes[index++] = new Process(c.DisplayName, "", () => Page = GenerateCharacterEquipPage(c));
        }

        processes[ActionGridView.TOTAL_BUTTON_COUNT - 2] = new Process("Inventory", "Manage items in inventory.");
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to camp.", () => Page = Hub);

        return null;
    }

    private ReadPage GenerateCharacterEquipPage(Character c) {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (EquippableItem myE in c.Equipment) {
            EquippableItem e = myE;
            processes[index++] = CreateUnequipProcess(c, e);
        }
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to camp.", () => Page = Inventory);

        return new ReadPage(
            string.Format("View and unequip equipment from {0}.", c.DisplayName),
            "",
            "Camp",
            mainCharacter: pc,
            left: new Character[] { c },
            processes: processes
            );
    }

    private Process CreateUnequipProcess(Character caster, EquippableItem item) {
        return new Process(Util.Color(item.Name, Color.yellow), item.Description, () => {
            Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.", caster.DisplayName, item.Name), Color.white, TextEffect.FADE_IN));
            caster.Equipment.Remove(item);
            item.CancelBonus(caster);
        });
    }



    private ReadPage Rp(
    string text,
    string location,
    IList<Process> processes,
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
            processes: processes);
    }

    private BattlePage Bp(string text,
    string location,
    string musicLoc,
    IList<Character> right,
    Page flee,
    Page victory,
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
                pc.Mercies.Add(new Flee(flee));
            },
            onExit: () => {
                pc.Mercies.Remove(new Flee(flee));
            },
            onTick: onTick
            );
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
