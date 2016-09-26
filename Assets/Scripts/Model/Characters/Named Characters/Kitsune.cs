using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Kitsune : ComputerCharacter {
    public const int STR = 100;
    public const int INT = 200;
    public const int DEX = 50;
    public const int VIT = 20;
    private const int MAX_CORRUPTION = 35; // Timed with music
    private const int ACTUAL_HEALTH = 100; // Health when she punches you out.

    private S? s;
    public Kitsune() : base("", "Kitsune", 50, STR, INT, DEX, VIT, Color.magenta, 1, null) {
        AddResource(new NamedResource.Corruption(MAX_CORRUPTION));
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
                if (GetResourceCount(ResourceType.HEALTH, false) <= ACTUAL_HEALTH) {
                    s = S.PUNCH_1;
                }
                AddToResource(ResourceType.CORRUPTION, false, Time.deltaTime);
                break;
            case S.SUMMON:
                nextBackTime += Time.deltaTime;
                if (!Game.Instance.PagePresenter.Page.GetAllies(this).Any(c => c is Tentacle)) {
                    s = null;
                    Game.Instance.Ordered(
                        new Event(() => this.IsTargetable = true),
                        new Event(() =>
                            Talk("So that wasn't enough, huh?",
                                 "The next batch will end you.",
                                 "Murdering all these innocent tentacles...",
                                 "...Do you feel like a hero yet?",
                                 "I'll make the next ones stronger!"
                                )
                        ),
                        new Event(() => s = S.FIGHT)
                        );
                }
                break;
            case S.PUNCH_1:
                if (IsCharged) {
                    this.IsTargetable = true;
                    s = null;
                    Game.Instance.Ordered(
                        new Event(new TextBox(string.Format("{0} unequipped <color=yellow>PaperFan</color>.", this.DisplayName)), 1f),
                        new Event(() => (new PaperFan(1)).CancelBonus(this)),
                        new Event(() => QuickCast(new Attack()), 2f),
                        new Event(() => Talk("Did you think you had a chance...?")),
                        new Event(() => s = S.PUNCH_2)
                        );
                }
                break;
            case S.PUNCH_2:
                break;
        }
    }

    public override void React(Spell spell) {
        if (Buffs.Any(b => b.SpellFactory.Name == "BackTurn") && spell.SpellFactory.Name == "Attack") {
            Talk(
                "I didn't actually think you'd it.",
                "Please keep attacking me while my back is turned.",
                "Huh? You actually attacked me?",
                "Keep on doing that.",
                "Smiting me is impolite. Only use attacks on me."
                );
        }
    }

    // Fighting details
    private const float BACK_INTERVAL_MIN = 26;
    private const float BACK_INTERVAL_MAX = 56;
    private float nextBackTime;

    // Summoning details
    private int summonCounter;
    private static readonly SummonTent[] SUMMONS = new SummonTent[] {
        new SummonTent(new Regenerator(), new Lasher()),
        new SummonTent(new Lasher(), new Lasher()),
        new SummonTent(new Regenerator(), new Regenerator()),
        new SummonTent(new Lasher(), new Regenerator(), new Lasher()),
        new SummonTent(new Regenerator(), new Lasher(), new Regenerator()),
        new SummonTent(new Lasher(), new Lasher(), new Lasher(), new Lasher())
    };
    protected override void DecideSpell() {
        switch (s) {
            case S.INTRO:
                s = null;
                Game.Instance.Ordered(
                new Event(action: () => Talk("Will Alestre's hero fall to...")),
                new Event(action: () => QuickCast(new PaperFan(1), this), delay: 2.0f),
                new Event(action: () => Talk("...this deadly paper fan?")),
                new Event(action: () => s = S.FIGHT)
                );
                break;
            case S.FIGHT:
                if (IsCharged && GetResourceCount(ResourceType.CORRUPTION, false) >= MAX_CORRUPTION) {
                    s = null;
                    Game.Instance.Ordered(
                        new Event(
                            () =>
                            Talk(
                                "Why don't you meet my friends?",
                                "Tentacles, heed my call!",
                                "Tentacles, heed the Earthkeeper's call!",
                                "Tentacles, assist me!",
                                "Arise, my tentacles!",
                                "The Earthkeeper calls for your aid!"
                            )),
                        new Event(
                            delay: 2,
                            action: () => {
                                QuickCast(SUMMONS[Mathf.Min(summonCounter++, SUMMONS.Length - 1)]);
                            }
                        ),
                        new Event(() =>
                        this.IsTargetable = false),
                        new Event(new TextBox(string.Format("{0} hides behind her tentacles.", this.DisplayName)), 0.5f),
                        new Event(() =>
                        s = S.SUMMON)
                    );
                } else if (BattleTimer >= nextBackTime && !Buffs.Any(b => b.SpellFactory.Name == "BackTurn")) {
                    nextBackTime = BattleTimer + Random.Range(BACK_INTERVAL_MIN, BACK_INTERVAL_MAX);
                    Util.Log("Next back: " + (nextBackTime - BattleTimer));
                    QuickCast(new BackTurn());
                    Talk("Hehehehe...",
                        "Feel free to count them. By hand.",
                        "Have you seen my tails yet?",
                        "Will you attack...?",
                        "Why don't you meet my tails?",
                        "How will you handle this, hero?",
                        "I suggest attacking right now.",
                        "Oh no. I'm in prime position to be backstabbed.",
                        "I hope no one attacks me while I'm turned around."
                        );
                } else {
                    QuickCast(new Attack());
                }
                break;
        }
    }
}
