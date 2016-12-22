using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class PageUtil {

    public static ReadPage GenerateExplorePage(Party party, IList<PageGenerator> exploreablePlaces, Page hub) {
        return Rp(
           text: "Where will you explore?",
           location: "Camp",
           tooltip: "",
           party: party,
           processes: GenerateExploreButtons(exploreablePlaces, hub)
           );
    }

    public static IList<IButtonable> GenerateExploreButtons(IList<PageGenerator> exploreablePlaces, Page hub) {
        IButtonable[] buttonable = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
        for (int i = 0; i < exploreablePlaces.Count; i++) {
            buttonable[i] = exploreablePlaces[i];
        }
        buttonable[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = hub);
        return buttonable;
    }

    public static ReadPage GeneratePlacesPage(Party party, IList<ReadPage> places, Page hub) {
        return Rp(
           text: "Where will you go?",
           location: "Camp",
           tooltip: "",
           processes: GeneratePlacesProcesses(places, hub),
           party: party,
           onEnter: () => {
           }
           );
    }

    public static IList<IButtonable> GeneratePlacesProcesses(IList<ReadPage> places, Page hub) {
        IButtonable[] buttonable = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        for (int i = 0; i < places.Count; i++) {
            buttonable[i] = new Process(places[i].Location, "Visit this area.", () => Game.Instance.CurrentPage = places[i]);
        }
        buttonable[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = hub);
        return buttonable;
    }

    public static ReadPage GenerateCharactersPage(Party party, Page hub) {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Character myc in party) {
            Character c = myc;
            processes[index++] = new Process(c.DisplayName, action: () => Game.Instance.CurrentPage = GenerateCharacterPage(c, hub));
        }
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = hub);

        return Rp(
            "Which character would you like to view?",
            "Camp",
            processes,
            party
            );
    }

    public static ReadPage GenerateCharacterPage(Character c, Page hub) {
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
                        Game.Instance.CurrentPage = LevelUpPage(c, GenerateCharacterPage(c, hub));
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
                new Process("Back", "", () => Game.Instance.CurrentPage = hub)
            }
            );
    }

    public static string StatText(Character target) {
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

    public const int ATT_INCREASE_AMOUNT = 1;
    public static ReadPage LevelUpPage(Character c, Page character) {
        ReadPage perkAssign =
        new ReadPage(
            "Select a perk.",
            "",
            string.Format("{0}'s Level {1} → Level {2}", c.DisplayName, c.Level.False - 1, c.Level.False),
            mainCharacter: c,
            left: new Character[] { c },
            right: new Character[] { },
            buttonables:
            new Process[] {
                new Process("None", action: () => Game.Instance.CurrentPage = GenerateCharacterPage(c, character))
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
                        Game.Instance.CurrentPage = perkAssign;
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
                mainCharacter: c,
                left: new Character[] { c },
                right: new Character[] { },
                buttonables: attributes
                );

        return attributeAssign;
    }

    public static ReadPage GenerateItemManagementPage(Party party, Inventory items, Page hub) {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Character myC in party) {
            Character c = myC;
            processes[index++] = new Process(c.DisplayName, string.Format("View and unequip {0}'s equipment.", c.DisplayName), () => Game.Instance.CurrentPage = GenerateCharacterEquipPage(c, party, items, hub));
        }

        processes[ActionGridView.TOTAL_BUTTON_COUNT - 2] = new Process("Inventory", "Use items in inventory.", () => Game.Instance.CurrentPage = GenerateInventoryPage(party, items, hub));
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = hub);

        return Rp(
            "",
            "Item and Equipment Manager",
            processes,
            party,
            "Unequip a specific character's items, or use items in inventory."
            );
    }

    public static ReadPage GenerateCharacterEquipPage(Character c, Party party, Inventory items, Page hub) {
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (EquippableItem myE in c.Equipment) {
            EquippableItem e = myE;
            processes[index++] = e != null ? CreateUnequipProcess(c, e, party, items, hub) : new Process();
        }
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = GenerateItemManagementPage(party, items, hub));

        return new ReadPage(
            "",
            string.Format("View and unequip equipment from {0}.", c.DisplayName),
            string.Format("{0}'s Equipment", c.DisplayName),
            mainCharacter: c,
            left: new Character[] { c },
            buttonables: processes
            );
    }


    public static Process CreateUnequipProcess(Character caster, EquippableItem item, Party party, Inventory items, Page hub) {
        Process p = new Process(Util.Color(item.Name, Color.yellow), item.Description, () => {
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.", caster.DisplayName, item.Name), Color.white, TextEffect.FADE_IN));
            item.UnequipItemInSlot(caster);
            Game.Instance.CurrentPage = GenerateCharacterEquipPage(caster, party, items, hub);
        });
        return p;
    }

    public static ReadPage GenerateInventoryPage(Party party, Inventory items, Page hub) {
        return Rp(
            "",
            "Inventory",
            GenerateInventoryProcesses(party, items, hub),
            party,
            "Use and equip items on particular characters."
            );
    }

    public static IList<IButtonable> GenerateInventoryProcesses(Party party, Inventory items, Page hub) {
        IButtonable[] itemButtons = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Item myI in items) {
            Item i = myI;
            itemButtons[index++] = (new Process(string.Format("{0}", i.Name), i.Description, () => Game.Instance.CurrentPage = GenerateUseItemOnCharacterPage(i, party, items, hub)));
        }
        itemButtons[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = GenerateItemManagementPage(party, items, hub));
        return itemButtons;
    }

    public static ReadPage GenerateUseItemOnCharacterPage(Item item, Party party, Inventory items, Page hub) {
        return Rp(
            "",
            "Items",
            GenerateUseItemProcesses(item, party, items, hub),
            party,
            string.Format("Who will use {0}?\n{1}", item.Name, item.Description)
            );
    }

    public static IList<IButtonable> GenerateUseItemProcesses(Item item, Party party, Inventory items, Page hub) {
        IButtonable[] useButtons = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
        int index = 0;
        foreach (Character myC in party) {
            Character c = myC;
            useButtons[index++] = (new Process(c.DisplayName, string.Format("{0} will use {1}.\n{2}", c.DisplayName, item.Name, item.Description),
                () => {
                    item.Cast(c, c);
                    Game.Instance.CurrentPage = GenerateInventoryPage(party, items, hub);
                }
                ));
        }
        useButtons[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Use a different item.", () => Game.Instance.CurrentPage = GenerateInventoryPage(party, items, hub));
        return useButtons;
    }

    public static PageGenerator Pg(
        string name,
        string description,
        params Encounter[] encounters
        ) {
        return new PageGenerator(p => { Game.Instance.CurrentPage = p; }, name, description, encounters);
    }

    public static ReadPage Rp(
    string text,
    string location,
    IList<IButtonable> processes,
    Party party,
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
            mainCharacter: party.Main,
            left: party.Members,
            right: right.Where(c => c.State != CharacterState.KILLED).ToArray(),
            onEnter: onEnter,
            onExit: onExit,
            onTick: onTick,
            buttonables: processes);
    }

    public static BattlePage Bp(
        Page hub,
        string text,
    string location,
    string musicLoc,
    Party party,
    IList<Character> right,
    Page victory = null,
    Page flee = null,
    Action onEnter = null,
    Action onTick = null) {
        return new BattlePage(
            text: text,
            location: location,
            musicLoc: musicLoc,
            mainCharacter: party.Main,
            left: party.Members,
            right: right,
            victory: victory,
            onEnter: () => {
                if (onEnter != null) {
                    onEnter.Invoke();
                }
                foreach (Character c in party) {
                    c.Mercies.Add(new Flee(hub));
                }
            },
            onExit: () => {
                foreach (Character c in party) {
                    c.Mercies.Remove(new Flee(hub));
                }
            },
            onTick: onTick
            );
    }

    public static BattlePage Bp(
    Page hub,
    string location,
    Party party,
    params Character[] right) {
        return new BattlePage(
            text: "",
            location: location,
            musicLoc: "Hero Immortal",
            mainCharacter: party.Main,
            left: party.Members,
            right: right,
            victory: hub,
            onEnter: () => {
                foreach (Character c in party) {
                    c.Mercies.Add(new Flee(hub));
                }
            },
            onExit: () => {
                foreach (Character c in party) {
                    c.Mercies.Remove(new Flee(hub));
                }
            });
    }

    public static BattlePage Bp(string text,
    string location,
    string musicLoc,
    Party party,
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
            mainCharacter: party.Main,
            left: party.Members,
            right: right,
            isVictory: victoryCondition,
            onEnter: () => {
                if (onEnter != null) {
                    onEnter.Invoke();
                }
            },
            victory: victory,
            onTick: onTick
            );
    }
}
