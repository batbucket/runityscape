using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class Game : MonoBehaviour {
    static Game _instance;
    public static Game Instance { get { return _instance; } }

    [SerializeField]
    TimeView _time;
    public TimeView Time { get { return _time; } }

    [SerializeField]
    HeaderView _header;
    public HeaderView Header { get { return _header; } }

    [SerializeField]
    TextBoxHolderView _textBoxHolder;
    public TextBoxHolderView TextBoxHolder { get { return _textBoxHolder; } }

    [SerializeField]
    PortraitHolderView _leftPortraits;
    public PortraitHolderView LeftPortraits { get { return _leftPortraits; } }

    [SerializeField]
    PortraitHolderView _rightPortraits;
    public PortraitHolderView RightPortraits { get { return _rightPortraits; } }

    [SerializeField]
    ActionGridView _actionGrid;
    public ActionGridView ActionGrid { get { return _actionGrid; } }

    [SerializeField]
    TooltipManager _tooltip;
    public TooltipManager Tooltip { get { return _tooltip; } }

    [SerializeField]
    EffectsManager _effects;
    public EffectsManager Effect { get { return _effects; } }

    [SerializeField]
    SoundView _sound;
    public SoundView Sound { get { return _sound; } }

    [SerializeField]
    HotkeyButton _menuButton;
    public HotkeyButton MenuButton { get { return _menuButton; } }

    public PagePresenter PagePresenter { get; private set; }
    public Character MainCharacter { get; private set; }

    Dictionary<string, bool> boolFlags;
    Dictionary<string, int> intFlags;

    public const float NORMAL_TEXT_SPEED = 0.001f;
    public static string DERP = "because I'm not paying a bunch of money to take <color=red>shitty</color> humanity classes";

    IDictionary<string, Page> pages;
    IDictionary<string, string> pageLinks;

    static IDictionary<string, RightBox> FORBIDDEN_NAMES = new Dictionary<string, RightBox>() {
        { "Alestre", new RightBox("placeholder", "Redeemer. Please do not do that.", Color.yellow) },
    };

    string pageID;
    string PageID {
        set {
            Util.Assert(pages.ContainsKey(value), string.Format("Page: \"{0}\" does not exist!", value));
            StopAllCoroutines();
            Effect.CancelEffects();
            if (pageID != null && !pageLinks.ContainsKey(value)) {
                pageLinks.Add(value, pageID);
            }
            pageID = value;
            PagePresenter.SetPage(pages[value]);
        }
    }
    Page Page {
        get { return PagePresenter.Page; }
    }

    void Start() {
        _instance = this;
        boolFlags = new Dictionary<string, bool>();
        PagePresenter = new PagePresenter();
        pages = new Dictionary<string, Page>();
        Time.IsEnabled = false;
        pageLinks = new Dictionary<string, string>();

        Header.Blurb = "";
        Header.Chapter = "";
        Header.Location = "";

        MenuButton.Hotkey = KeyCode.None;
        MenuButton.Process = new Process("Main Menu", "Return to the Main Menu.", () => { Start(); PageID = "primary"; });

        pages["primary"] = new ReadPage(
            tooltip: "Welcome to RunityScape.",
            processes: new Process[] {
                new Process("New Game", "Start a new game.", () => PageID = "newGame-Name"),
                new Process("Load Game", "Load a saved game.", condition: () => false),
                new Process("Debug", "Enter the debug page. ENTER AT YOUR OWN RISK.", () => PageID = "debug"),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process("Exit", "Exit the application.", () => Application.Quit())
        });

        CreateWorld();

        PageID = "primary";//"tutorial-battle0";
    }

    void CreateWorld() {
        CreateNewGame();
        CreateIntro();
        CreateTutorial();
        CreateDebug();
    }

    void CreateNewGame() {
        const int STARTING_ATTRIBUTE_COUNT = 1;
        Hero hero = new Hero(STARTING_ATTRIBUTE_COUNT, STARTING_ATTRIBUTE_COUNT, STARTING_ATTRIBUTE_COUNT, STARTING_ATTRIBUTE_COUNT);
        foreach (Resource r in hero.Resources.Values) {
            r.IsVisible = false;
        }
        pages["newGame-Name"] = new ReadPage(
            hasInputField: true,
            left: new Character[] { hero },
            processes: ProcessesWithBack(
                new Process(
                    "Confirm",
                    "Use this name.",
                    () => {
                        PageID = "newGame-AttSel";
                    },
                    () => {
                        return Page.InputtedString.Length > 0 && !FORBIDDEN_NAMES.Keys.Any(s => string.Compare(Page.InputtedString, s, StringComparison.OrdinalIgnoreCase) == 0);
                    }
                )
            ),
            onTick: () => {
                hero.Name = Page.InputtedString;
                pages["newGame-Name"].Tooltip = FORBIDDEN_NAMES.Keys.Any(s => string.Compare(Page.InputtedString, s, StringComparison.OrdinalIgnoreCase) == 0) ? "That name is forbidden." : "What is your name?";
            }
        );

        const int ASSIGNABLE_POINTS = 2;
        int currentPoints = ASSIGNABLE_POINTS;
        Process[] attributeAdder = new Process[AttributeType.ALL.Count];
        int index = 0;
        foreach (AttributeType myAttributeType in AttributeType.ALL) {
            AttributeType attributeType = myAttributeType;
            attributeAdder[index++] = new Process(
                attributeType.Name,
                attributeType.ShortDescription,
                () => {
                    if (currentPoints > 0) {
                        Sound.Play("Sounds/Blip_0");
                        hero.AddToAttribute(attributeType, false, 1);
                        hero.AddToAttribute(attributeType, true, 1, true);
                        AddTextBox(new TextBox(string.Format("You have assigned a point to {0}.", attributeType.Name)));
                        AddTextBox(new TextBox(string.Format("Your current Attribute distribution is:\n{0}", hero.AttributeDistribution)));
                        AddTextBox(new TextBox(string.Format(--currentPoints > 0 ? "You have {0} point{1} remaining." : "You have no points remaining.", currentPoints, currentPoints == 1 ? "" : "s")));
                    }
                },
                () => currentPoints > 0
            );
        }
        foreach (Resource r in hero.Resources.Values) {
            r.IsVisible = true;
        }
        pages["newGame-AttSel"] = new ReadPage(
            text: string.Format("Choose {0} Attributes to assign points to.", ASSIGNABLE_POINTS),
            left: new Character[] { hero },
            processes: ProcessesWithBack(
                attributeAdder,
                new Process(
                    "Confirm",
                    "Confirm your assigned Attribute points.",
                    () => {
                        hero.FillResources();
                        PageID = "intro-HelloWorld";
                    },
                    () => currentPoints <= 0
                ),
                new Process(
                        "Reset",
                        "Reset your assigned Attribute points.",
                        () => {
                            Sound.Play("Sounds/Zip_1");
                            AddTextBox(new TextBox("You have reset your Attribute point assignments."));
                            AddTextBox(new TextBox(string.Format("Choose {0} Attributes to assign points to.", ASSIGNABLE_POINTS)));
                            currentPoints = ASSIGNABLE_POINTS;
                            foreach (Attribute a in hero.Attributes.Values) {
                                a.False = STARTING_ATTRIBUTE_COUNT;
                                a.True = STARTING_ATTRIBUTE_COUNT;
                            }
                            Effect.CreateHitsplat("RESET", Color.white, hero);
                        },
                        () => currentPoints < 2
                )
            ),
            onTick: () => {
                hero.AddToResource(ResourceType.HEALTH, false, 0.5f);
            }
        );
        MainCharacter = hero;
    }

    void CreateIntro() {
        Guardian g = new Guardian();
        g.Resources.Remove(ResourceType.HEALTH);
        g.Name = "";
        Action normal = () => OrderedEvents(
            new Event(new RightBox(g, "I am Goddess Alestre, gardener of this world.")),
            new Event(() => g.Name = "Alestre"),
            new Event(new RightBox(g, "For millenia I have tended to this blue marble, but I may not be able to for much longer.")),
            new Event(new RightBox(g, "My followers created six purifiers to hallow the elements in your mortal lands.")),
            new Event(new RightBox(g, "Celestial beings are... quite sensitive to impurities.")),
            new Event(new RightBox(g, "Exposure to even a single fleck of corruption would irreversibly taint my body and soul, dooming this world.")),
            new Event(new RightBox(g, "But this world is also inhabited by other <i>creatures</i>, who are not as light-touched as your kind. These abhorrent beings have begun sealing these purifiers, seeking to end my——humanity's presence here forever.")),
            new Event(new RightBox(g, "These abhorrent beings have begun sealing these purifiers, seeking to end my——humanity's presence here forever.")),
            new Event(new RightBox(g, "The purifier for light is below this temple. It is the last unsealed purifier in this world.")),
            new Event(new RightBox(g, "In life, you led humanity against these terrible creatures.")),
            new Event(new RightBox(g, "But as with all good heroes, your stay was too brief for this world. Cut down by one of your own.")),
            new Event(new RightBox(g, "I have given you a second chance to save humanity.")),
            new Event(new RightBox(g, string.Format("O faithful Redeemer, awaken from your slumber and come to me...", MainCharacter.Name))),
            new Event(() => pages["intro-HelloWorld"].ActionGrid = new Process[] { new Process("Awaken", action: () => PageID = "temple-waterRoom") })
        );

        pages["intro-HelloWorld"] = new ReadPage(
            left: new Character[] { MainCharacter },
            right: new Character[] { g },
            onFirstEnter: () => {
                OrderedEvents(
                    new Event(new TextBox("You feel weightless..."), 0),
                    new Event(new RightBox("placeholder", string.Format("{0}...", MainCharacter.Name), Color.yellow), 1),
                    new Event(new RightBox("placeholder", string.Format("O redeemed {0}...", MainCharacter.Name), Color.yellow), 1),
                    new Event(new RightBox("placeholder", string.Format("Can you hear my voice?", MainCharacter.Name), Color.yellow), 1),
                    new Event(() => pages["intro-HelloWorld"].ActionGrid
                            = new Process[] {
                            new Process(
                                "I can!",
                                action: () => {
                                    pages["intro-HelloWorld"].ActionGrid = new Process[0];
                                    OrderedEvents(
                                        new Event(new LeftBox("placeholder", "I can!", Color.white), 1),
                                        new Event(normal, 1)
                                    );
                                }
                            ),
                            new Process(
                                "Nope.",
                                action: () => {
                                    pages["intro-HelloWorld"].ActionGrid = new Process[0];
                                    OrderedEvents(
                                        new Event(new LeftBox("placeholder", "Nope.", Color.white), 1),
                                        new Event(new RightBox("placeholder", "...", Color.yellow), 1),
                                        new Event(new RightBox("placeholder", "Then why did you respond?", Color.yellow), 1),
                                        new Event(new LeftBox("placeholder", "...", Color.white), 1),
                                        new Event(normal, 1)
                                    );
                                }
                            ),
                            new Process(
                                "...",
                                action: () => {
                                    pages["intro-HelloWorld"].ActionGrid = new Process[0];
                                    OrderedEvents(
                                        new Event(new LeftBox("placeholder", "...", Color.white), 1),
                                        new Event(new RightBox("placeholder", "...", Color.yellow), 1),
                                        new Event(new LeftBox("placeholder", "......", Color.white), 1),
                                        new Event(new RightBox("placeholder", "......", Color.yellow), 1),
                                        new Event(new LeftBox("placeholder", "...........?", Color.white), 1),
                                        new Event(() => {
                                            MainCharacter.OnDefeat(true);
                                            MainCharacter.AddToResource(ResourceType.HEALTH, false, Int32.MinValue, true);
                                            AddTextBox(new TextBox(string.Format("A streak of divine lightning hits {0} for <color=red>2147483648</color> damage!", MainCharacter.Name)));
                                            Game.Instance.Sound.Play("Sounds/Boom_1");
                                        }, 0),
                                        new Event(() => MainCharacter.OnKill(true)),
                                        new Event(() => {
                                            Page.Tooltip = "You have died.";
                                            pages["intro-HelloWorld"].ActionGrid = new Process[] { new Process("Main Menu", "Return to the main menu.", () => { Start(); PageID = "primary"; }) };
                                        }, 1),
                                        new Event(new RightBox("placeholder", "Perhaps they were too afraid to speak?", Color.red), 10),
                                        new Event(new RightBox("placeholder", "Or unwise?", Color.grey), 2),
                                        new Event(new RightBox("placeholder", "Then they could not have saved us.", Color.yellow), 2)
                                    );
                                }
                            )
                        }
                    )
                );
            }
            );
    }

    void CreateTutorial() {

        Process[] waterRoomSubsequent = new Process[] {
                        new Process("Leave", action: () => PageID = "temple-hallway"),
                        new OneShotProcess("Look around", action: () =>
                            AddTextBox(
                                new TextBox("The pleasantly warm room is tiny. The pool of water is just large enough for one person. Water drips into and exits the pool from an unknown source, providing a pleasant ambience sure to lull anyone to sleep.")
                            )
                        )
        };
        pages["temple-waterRoom"] = new ReadPage(
            left: new Character[] { MainCharacter },
            mainCharacter: MainCharacter,
            location: "Last Temple - Rebirth Room",
            processes: waterRoomSubsequent,
            onFirstEnter: () => {
                AddTextBox(new TextBox("As you wake up, you realize that you are floating on water. The water matches your temperature perfectly, making you feel as if you are suspended in nothingness. A soft golden light reflects off the walls."));
                Header.Blurb = "Go to Alestre.";
                Header.Chapter = "Chapter 0";
                pages["temple-waterRoom"].ActionGrid = new Process[] {
                    new OneShotProcess("Get up", action: () =>
                    {
                        AddTextBox(new TextBox("You climb out of the pool."));
                        pages["temple-waterRoom"].ActionGrid = waterRoomSubsequent;
                    })
                };
            }
        );

        pages["temple-hallway"] = new ReadPage(
            text: "The hallway is absolutely massive.",
            tooltip: "",
            left: new Character[] { MainCharacter },
            right: new Character[] { },
            mainCharacter: MainCharacter,
            location: "Last Temple - Hallway",
            processes: new Process[] {
                new Process("Throne", action: () => PageID = "temple-throne"),
                new Process("Rebirth", action: () => PageID = "temple-waterRoom"),
                new Process("Corrupted Entrance", action: () => PageID = "temple-corruptRoom0", condition: () => false),
                new OneShotProcess("Exit", ("Leave the temple."),
                () => {
                    OrderedEvents(
                        new Event(new TextBox("As you try to leave the temple, a voice begins to echo through your head...")),
                        new Event(new RightBox("placeholder", "This isn't implemented yet...", Color.cyan, soundLocation: "Sounds/Attack_0"))
                    );
                    }
                )
            }
        );

        Guardian g = new Guardian();
        pages["temple-throne"] = new ReadPage(
            text: "Goddess Alestre is sitting on her throne.",
            tooltip: "",
            left: new Character[] { MainCharacter },
            right: new Character[] { },
            mainCharacter: MainCharacter,
            location: "Last Temple - Throne Room",
            processes: new Process[] {
                new Process("Approach", action: () => {
                    pages["temple-throne"].ActionGrid = new Process[0];
                    pages["temple-throne"].RightCharacters.Add(g);
                    OrderedEvents(
                        new Event(new TextBox("Some appearance description."), 0),
                        new Event(new RightBox(g, "When you are ready, I will teach you how to defend yourself against these wicked creatures."), 2),
                        new Event(() => {
                            pages["temple-throne"].ActionGrid = new Process[] {
                                new OneShotProcess("I'm ready.", action: () => {
                                    pages["temple-throne"].ActionGrid = new Process[0];
                                    OrderedEvents(
                                        new Event(new LeftBox(MainCharacter, "I'm ready."), 0),
                                        new Event(new RightBox(g, string.Format("Very well. Let's see what you're made of... Redeemer {0}!", MainCharacter.Name)), 1),
                                        new Event(() => {
                                            PageID = "tutorial-battle0";
                                        }, 1)
                                    );
                                }),
                                new OneShotProcess("Not yet.", action: () => {
                                    pages["temple-throne"].ActionGrid = new Process[0];
                                    OrderedEvents(
                                        new Event(new RightBox(g, string.Format("When you are ready, Redeemer {0}.", MainCharacter.Name)), 0),
                                        new Event(() => {
                                            pages["temple-throne"].ResetActionGrid();
                                        }, 0)
                                    );
                                })
                            };
                        })
                    );
                }
                ),
                new Process("Hallway", action: () => {
                    pageID = "temple-hallway";
                })
            },
            onFirstEnter: () => {
                AddTextBox(new RightBox(g, string.Format("Hello, {0}.", MainCharacter.Name)));
            }
        );

        pages["tutorial-battle0"] = new BattlePage(
            text: "You are fighting Alestre!",
            location: "Last Temple - Throne Room",
            mainCharacter: MainCharacter,
            left: new Character[] { MainCharacter },
            right: new Character[] { g }
        );

    }

    void CreateDebug() {
        pages["debug"] = new ReadPage("What", "Hello world", mainCharacter: new Amit(), left: new Character[] { new Amit() }, right: new Character[] { new Steve(), new Steve() },
            processes: new Process[] {
                new Process("Normal TextBox", "Say Hello world",
                    () => AddTextBox(
                        new TextBox(DERP, Color.white, TextEffect.FADE_IN, "Sounds/Blip_0", .1f))),
                new Process("LeftBox", "Say Hello world",
                    () => AddTextBox(
                        new LeftBox("crying_mudkip", DERP, Color.white))),
                new Process("RightBox", "Say Hello world",
                    () => AddTextBox(
                        new RightBox("crying_mudkip", DERP, Color.white))),
                new Process("InputBox", "Type something",
                    () => TextBoxHolder.AddInputBoxView()),
                new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => PagePresenter.SetPage(pages["debug1"])),
                new Process("Steve Massacre", "Steve. It was nice to meet you. Goodbye.", () => { PagePresenter.SetPage(pages["debug2"]); Sound.Play("Music/99P"); }),
                new Process("Shake yourself", "Literally U******E", () => { Effect.ShakeEffect(MainCharacter, .05f); }),
                new Process("Fade yourself", "Literally U******E x2", () => { Effect.CharacterDeath(MainCharacter, 1.0f); }),
                new OneShotProcess("Test OneShotProcess", action: () => Debug.Log("ya did it!")),
                new Process("deadm00se", "Text not displaying right.", () => AddTextBox(new RightBox("placeholder", "But this world is also inhabited by other <i>creatures</i>, who are not as light-touched as your kind. These abhorrent beings have begun sealing these purifiers, seeking to end my——humanity's presence here forever.", Color.white))),
                new Process(),
                new Process("Back", "Go back to the main menu.", () => { PagePresenter.SetPage(pages["primary"]); })
            }
        );
        pages["debug1"] = new BattlePage(text: "Hello world!", mainCharacter: new Amit(), left: new Character[] { new Amit(), new Amit() }, right: new Character[] { new Steve(), new Steve() });
        pages["debug2"] = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() }, right: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() });
    }

    public struct Event {
        public readonly Action action;
        public readonly float delay;
        public readonly Func<bool> hasEnded;

        public Event(Action action, float delay = 0, Func<bool> endCondition = null) {
            this.action = action;
            this.hasEnded = endCondition ?? (() => true);
            this.delay = delay;
        }

        //Special Text-Event builders

        public Event(TextBox t, float delay = 1) {
            this.action = () => Instance.AddTextBox(t);
            this.delay = delay;
            this.hasEnded = () => t.IsDone;
        }
    }
    public void OrderedEvents(params Event[] events) {
        StartCoroutine(Timeline(events));
    }

    IEnumerator Timeline(Event[] events) {
        foreach (Event e in events) {
            float timer = 0;
            while ((timer += UnityEngine.Time.deltaTime) < e.delay) {
                yield return null;
            }
            e.action.Invoke();
            while (!e.hasEnded.Invoke()) {
                yield return null;
            }
        }
    }

    BattlePage CreateBattle(string text = "",
                            string tooltip = "",
                            string location = "",
                            Character mainCharacter = null,
                            Character[] left = null,
                            Character[] right = null,
                            Action onFirstEnter = null,
                            Action onEnter = null,
                            Action onFirstExit = null,
                            Action onExit = null,
                            Action onTick = null
                            ) {
        return new BattlePage(text, tooltip, location, MainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick);
    }

    ReadPage CreateRead(string text = "",
                        string tooltip = "",
                        string location = "",
                        bool hasInputField = false,
                        Character mainCharacter = null,
                        Character[] left = null,
                        Character[] right = null,
                        Action onFirstEnter = null,
                        Action onEnter = null,
                        Action onFirstExit = null,
                        Action onExit = null,
                        Action onTick = null,
                        Process[] processes = null
                        ) {
        return new ReadPage(text, tooltip, location, hasInputField, MainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, processes);
    }

    void GoToLastPage() {
        PageID = pageLinks[pageID];
    }

    public void AddTextBox(TextBox t, Action callBack = null) {
        PagePresenter.AddTextBox(t, callBack);
    }

    Process[] ProcessesWithBack(Process[] processes0, params Process[] processes1) {
        Util.Assert(processes0.Length + processes1.Length < ActionGridView.TOTAL_BUTTON_COUNT - 1, "Too many processes!");
        Process[] full = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        processes0.CopyTo(full, 0);
        processes1.CopyTo(full, processes0.Length);
        full[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Go back to the previous page.", GoToLastPage);
        return full;
    }

    Process[] ProcessesWithBack(params Process[] processes) {
        Util.Assert(processes.Length < ActionGridView.TOTAL_BUTTON_COUNT - 1, "Too many processes!");
        Process[] full = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        processes.CopyTo(full, 0);
        full[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Go back to the previous page.", GoToLastPage);
        return full;
    }

    // Update is called once per frame
    void Update() {
        PagePresenter.Tick();
    }

    public PortraitHolderView GetPortraitHolder(bool isRightSide) {
        if (!isRightSide) {
            return LeftPortraits;
        } else {
            return RightPortraits;
        }
    }

    void SetFlag(string key, bool value) {
        boolFlags.Add(key, value);
    }

    bool GetFlag(string key) {
        bool res;
        boolFlags.TryGetValue(key, out res);
        return res;
    }

    void CreateGraph() {

    }
}
