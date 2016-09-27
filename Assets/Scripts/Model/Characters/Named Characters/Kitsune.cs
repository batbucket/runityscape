using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Kitsune : ComputerCharacter {
    public const int STR = 100;
    public const int INT = 200;
    public const int DEX = 50;
    public const int VIT = 60;
    private const int MAX_CORRUPTION = 35; // Timed with music
    private const int ACTUAL_HEALTH = 100; // She punches you out after she loses this much health.

    private static readonly Tentacle[] SUMMONABLES = { new Lasher(), new Regenerator() };
    private const int SUMMON_MIN = 2;
    private const int SUMMON_MAX = 5;

    // Fighting details
    private const float BACK_INTERVAL_MIN = 16;
    private const float BACK_INTERVAL_MAX = 26;

    private float nextBackTime;

    // Summoning details
    private int summonCounter;

    private S? s;

    public Kitsune() : base("Icons/fox-head", "Kitsune", 50, STR, INT, DEX, VIT, Color.magenta, 1, null) {
        AddResource(new NamedResource.Corruption(MAX_CORRUPTION));

        this.summonCounter = SUMMON_MIN;
        this.s = S.INTRO;
    }

    private enum S {
        INTRO,
        FIGHT,
        COUNTER,
        SUMMON,
        PUNCH_1,
        PUNCH_2,
    }

    public override void Act() {
        //this.IsShowingBarCounts = true;
        base.Act();

        switch (s) {
            case S.FIGHT:
                AddToResource(ResourceType.CORRUPTION, false, Time.deltaTime);
                break;
            case S.SUMMON:
                nextBackTime += Time.deltaTime;
                if (!Game.Instance.PagePresenter.Page.GetAllies(this).Any(c => c is Tentacle)) {
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
            case S.PUNCH_1:
                break;
            case S.PUNCH_2:
                break;
        }
    }

    public override void OnVictory() {
        Talk("Got any more heroes to send my way, Alestre?");
    }

    public override void React(Spell spell) {
        if (Buffs.Any(b => b.SpellFactory.Name == "BackTurn") && spell.Caster.State == CharacterState.ALIVE) {
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
            } else if (spell.SpellFactory.Name == "LifePotion") {
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
                new Event(action: () => Talk("Death by fan.")),
                new Event(action: () => QuickCast(new WoodFan(1), this), delay: 2.0f),
                new Event(action: () => s = S.FIGHT)
                );
                break;
            case S.FIGHT:
                if (GetResourceCount(ResourceType.HEALTH, false) <= GetResourceCount(ResourceType.HEALTH, true) - ACTUAL_HEALTH) {
                    s = null;
                    Game.Instance.Ordered(
                        new Event(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.", this.DisplayName, (new WoodFan(1).Name))), 1f),
                        new Event(() => (new WoodFan(1)).CancelBonus(this)),
                        new Event(() => QuickCast(new Attack(canMiss: false)), 2f),
                        new Event(() => Talk("Did you think you had a chance...?")),
                        new Event(() => s = S.PUNCH_2)
                        );
                } else if (GetResourceCount(ResourceType.CORRUPTION, false) >= MAX_CORRUPTION) {
                    s = null;
                    Game.Instance.Ordered(
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
                        new Event(
                            delay: 2,
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
                        new Event(new TextBox(string.Format("{0} hides behind its tentacles.", this.DisplayName)), 0.5f),
                        new Event(() =>
                        s = S.SUMMON)
                    );
                } else if (BattleTimer >= nextBackTime && !Buffs.Any(b => b.SpellFactory.Name == "BackTurn")) {
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
