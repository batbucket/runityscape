using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Kitsune : ComputerCharacter {
    public const int STR = 100;
    public const int INT = 200;
    public const int DEX = 50;
    public const int VIT = 66;
    private const int MAX_CORRUPTION = 35; // Timed with music

    public const int F_STR = 4;
    public const int F_INT = 100;
    public const int F_DEX = 2;
    public const int F_VIT = 10;

    private static readonly Tentacle[] SUMMONABLES = { new Lasher(), new Regenerator() };
    private const int SUMMON_MIN = 2;
    private const int SUMMON_MAX = 5;

    // Fighting details
    private const float BACK_INTERVAL_MIN = 16;
    private const float BACK_INTERVAL_MAX = 46;

    private float nextBackTime;

    // Summoning details
    private int summonCounter;

    private S? s;

    private bool fakeDefeated;

    private bool battleEnded;
    public bool IsBattleEnded { get { return battleEnded; } }

    public Kitsune() : base("Icons/fox-head", "Kitsune", 50, STR, INT, DEX, VIT, Color.magenta, 1, null) {
        AddResource(new NamedResource.Corruption(MAX_CORRUPTION));

        this.summonCounter = SUMMON_MIN;
        this.s = S.INTRO;
        this.AddToResource(ResourceType.CORRUPTION, false, MAX_CORRUPTION);
    }

    private enum S {
        INTRO,
        FIGHT,
        COUNTER,
        SUMMON,
        BATTLE_END,
        PUNCH_2,
    }

    public override void OnBattleStart() {
        AddToAttribute(AttributeType.STRENGTH, false, -GetAttributeCount(AttributeType.STRENGTH, false) + F_STR);
        AddToAttribute(AttributeType.INTELLIGENCE, false, -GetAttributeCount(AttributeType.INTELLIGENCE, false) + F_INT);
        AddToAttribute(AttributeType.AGILITY, false, -GetAttributeCount(AttributeType.AGILITY, false) + F_DEX);
        AddToAttribute(AttributeType.VITALITY, false, -GetAttributeCount(AttributeType.VITALITY, false) + F_VIT);
    }

    public override void OnKill() {
        State = CharacterState.ALIVE;
        Presenter.PortraitView.ClearEffects();
        AddToAttribute(AttributeType.STRENGTH, false, GetAttributeCount(AttributeType.STRENGTH, true) - F_STR);
        AddToAttribute(AttributeType.INTELLIGENCE, false, GetAttributeCount(AttributeType.INTELLIGENCE, true) - F_INT);
        AddToAttribute(AttributeType.AGILITY, false, GetAttributeCount(AttributeType.AGILITY, true) - F_DEX);
        AddToAttribute(AttributeType.VITALITY, false, GetAttributeCount(AttributeType.VITALITY, true) - F_VIT);
        Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0}'s illusion was shattered!", DisplayName)));
        AddToResource(ResourceType.HEALTH, false, GetResourceCount(ResourceType.HEALTH, true) - GetResourceCount(ResourceType.HEALTH, false));
        AddToResource(ResourceType.CHARGE, false, GetResourceCount(ResourceType.CHARGE, true));
        fakeDefeated = true;
    }

    public override void Act() {
        base.Act();

        switch (s) {
            case S.FIGHT:
                AddToResource(
                    ResourceType.CORRUPTION,
                    false,
                    Time.deltaTime);
                break;
            case S.SUMMON:
                nextBackTime += Time.deltaTime;
                if (!Game.Instance.CurrentPage.GetAllies(this).Any(c => c is Tentacle)) {
                    s = null;
                    Game.Instance.Ordered(
                        new Event(() => this.IsTargetable = true),
                        new Event(() =>
                            Talk("The moves of a hero.",
                                 "Nice moves.",
                                 "Murdering all these innocent tentacles...",
                                 "Do you feel like a hero yet?",
                                 "I am quaking in fear.",
                                 "Is it time for me to flee?",
                                 "An impressive display of strength.",
                                 "An impressive display of intelligence.",
                                 "As to be expected from the hero.",
                                 "An impressive display of dexterity.",
                                 "An impressive display of vitality, I guess. You didn't die.",
                                 "And the hero emerges victorious."
                                )
                        ),
                        new Event(() => s = S.FIGHT)
                        );
                }
                break;
            case S.BATTLE_END:
                battleEnded = true;
                break;
        }
    }

    public override void OnVictory() {
        Talk("Hardly a challenge.");
    }

    public override void OnDefeat() {
        base.OnDefeat();
        Game.Instance.Sound.StopAll();
        Talk("...!");
    }

    public override void React(Spell spell) {

        if (Buffs.Any(s => s.SpellFactory.Name == "BackTurn") && spell.Caster.State == CharacterState.ALIVE) {
            if (spell.SpellFactory.Name == "Attack") {
                Talk(
                    "I didn't actually think you'd do it.",
                    "Really? You actually attacked me?",
                    "Really?",
                    "Keep on doing that.",
                    "A hero's strength.",
                    "A decision full of intelligence.",
                    "A dexterous strike.",
                    "A healthy idea.",
                    "Ouch.",
                    "Oh no. I have been hurt.",
                    "What happened to honor?"
                    );
            } else if (spell.SpellFactory.Name == "Smite") {
                Talk(
                    "No fair.",
                    "Why don't you refrain from doing that?",
                    "I suggest attacking into my tails instead.",
                    "I wish I had the heavens to call on.",
                    "How many times are you going to do that?"
                    );
            }
        }
    }

    public override void Witness(Spell spell) {
        if (Buffs.Any(s => s.SpellFactory.Name == "BackTurn") && spell.Caster.State == CharacterState.ALIVE) {
            if (spell.SpellFactory.Name == "LifePotion") {
                Talk(
                    "...",
                    "A wise decision.",
                    "Drink while you still can, hero.",
                    "How many of those do you have left?",
                    "You'll run out eventually.",
                    "Why don't you attack me instead?",
                    "No fair.",
                    "I suggest attacking instead."
                    );
            }
        }
    }

    protected override void DecideSpell() {
        switch (s) {
            case S.INTRO:
                s = null;
                Game.Instance.Ordered(
                new Event(action: () => s = S.FIGHT)
                );
                break;
            case S.FIGHT:
                if (fakeDefeated) {
                    s = null;
                    Game.Instance.Ordered(
                        new Event(() => QuickCast(new Attack(canMiss: false)), 2f),
                        new Event(() => Talk("Did you think you had a chance...?")),
                        new Event(() => s = S.BATTLE_END)
                        );
                } else if (GetResourceCount(ResourceType.CORRUPTION, false) >= MAX_CORRUPTION) {
                    s = null;
                    Game.Instance.Ordered(
                        new Event(
                            action: () => {
                                IList<Tentacle> summon = new List<Tentacle>() { new Lasher() };
                                int clampedSummonCounter = Mathf.Clamp(summonCounter, SUMMON_MIN, SUMMON_MAX) - summon.Count;
                                for (int i = 0; i < clampedSummonCounter; i++) {
                                    summon.Add(SUMMONABLES.PickRandom());
                                }
                                QuickCast(new SummonTent(summon));
                                summonCounter++;
                            }
                        ),
                        new Event(() =>
                        this.IsTargetable = false),
                        new Event(
                            () =>
                            Talk(
                                "Why don't you meet my friends?",
                                "You killed a few of these on your way here. Here, have some more.",
                                "Remember these?",
                                "How good is your memory?",
                                "Do you remember how to defeat these?",
                                "Do you still remember how to defeat these?",
                                "Here, have some more tentacles.",
                                "Here, have some tentacles.",
                                "Let's see that strength of yours.",
                                "Let's see that intelligence of yours.",
                                "Let's see that dexterity of yours.",
                                "Let's see that vitality of yours."
                            )),
                        new Event(() =>
                        s = S.SUMMON)
                    );
                } else if (BattleTimer >= nextBackTime && !Buffs.Any(b => b.SpellFactory is BackTurn)) {
                    nextBackTime = BattleTimer + Random.Range(BACK_INTERVAL_MIN, BACK_INTERVAL_MAX);
                    QuickCast(new BackTurn());
                    Talk(
                        "Have you seen my tails yet?",
                        "Will you attack...?",
                        "Why don't you meet my tails?",
                        "I suggest attacking right now.",
                        "Oh no. I'm in prime position to be backstabbed.",
                        "I hope no one attacks me while I'm turned around.",
                        "Please refrain from attacking me.",
                        "Excuse me while I turn around for no reason."
                        );
                } else {
                    QuickCast(new Attack(canCrit: false));
                }
                break;
        }
    }
}
