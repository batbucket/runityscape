using UnityEngine;
using System.Linq;

public class Guardian : ComputerCharacter {
    enum State {
        REQ_ATTACK,
        REQ_SPELL,
        GIVE_ORB,
        REQ_SPELL_2,
        SANDBOX,
        MERCY
    }

    State? state;

    OneShotProcess attackMePls;
    OneShotProcess niceMissMang;
    OneShotProcess niceHitMang;
    OneShotProcess niceCritMang;
    OneShotProcess howAboutASpell;
    OneShotProcess tfwNoSpells;
    OneShotProcess heresYourStuff;
    OneShotProcess oldStuffSorry;
    OneShotProcess spellTime;
    OneShotProcess hitWithSpell;
    OneShotProcess iWillSpareYou;
    OneShotProcess spareBackstab;

    public Guardian() : base("Icons/angel-wings", "Alestre", 10, 10, 10, 10, 10, Color.yellow, 0) {
        state = State.REQ_ATTACK;
        attackMePls = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => Game.Instance.MainCharacter.IsCharging = false),
                new Game.Event(new RightBox(this, string.Format("Show me an attack, {0}.", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => Game.Instance.MainCharacter.IsCharging = true)
            );
        }
        );

        niceMissMang = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => Game.Instance.MainCharacter.IsCharging = false),
                new Game.Event(new RightBox(this, "...")),
                new Game.Event(new RightBox(this, string.Format("You cannot hold back.", Game.Instance.MainCharacter.Name))),
                new Game.Event(new RightBox(this, string.Format("The creatures you will fight will not be as merciful.", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => state = State.REQ_SPELL)
            );
        }
        );

        niceHitMang = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => Game.Instance.MainCharacter.IsCharging = false),
                new Game.Event(new RightBox(this, string.Format("Well done.", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => state = State.REQ_SPELL)
            );
        }
        );

        niceCritMang = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => Game.Instance.MainCharacter.IsCharging = false),
                new Game.Event(new RightBox(this, string.Format("Splendid. I see you still remember.", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => state = State.REQ_SPELL)
            );
        }
        );

        howAboutASpell = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(new RightBox(this, string.Format("Let's try a spell. Are your memories sufficient?", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => Game.Instance.MainCharacter.IsCharging = true)
            );
        }
        );

        tfwNoSpells = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(new RightBox(this, string.Format("It seems you have forgotten.", Game.Instance.MainCharacter.Name)), 2),
                new Game.Event(() => state = State.GIVE_ORB)
            );
        });

        heresYourStuff = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(new RightBox(this,
                    string.Format(
                        "Your past belongings are inside this. Perhaps equipping them may help your memory.",
                        Game.Instance.MainCharacter.Name))),
                new Game.Event(new TextBox(
                    string.Format(
                        "Time Capsule was added to {0}'s items.",
                        Game.Instance.MainCharacter.Name))),
                new Game.Event(() => {
                    Game.Instance.MainCharacter.Selections[Selection.ITEM].Add(new TimeCapsule(1));
                })
            );
        }
);

        oldStuffSorry = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(new RightBox(this, string.Format("Your equipment has not aged well, but it will be sufficient.")))
            );
        }
        );

        spellTime = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => state = State.REQ_SPELL_2),
                new Game.Event(new TextBox(string.Format("{0} remembered a spell.", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => Game.Instance.MainCharacter.Selections[Selection.SPELL].Add(new Smite())),
                new Game.Event(new RightBox(this, string.Format("Build up your <color=yellow>skill</color> by attacking, then show me your spell at its strongest.", Game.Instance.MainCharacter.Name)))
            );
        }
        );

        hitWithSpell = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => state = State.SANDBOX),
                new Game.Event(new RightBox(this, string.Format("I have tought you all that I can.", Game.Instance.MainCharacter.Name))),
                new Game.Event(new RightBox(this, string.Format("You may test your abilities further if you wish.", Game.Instance.MainCharacter.Name))),
                new Game.Event(new RightBox(this, string.Format("Perform the action of talking to me when you are done.", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => Game.Instance.MainCharacter.Selections[Selection.ACT].Add(new Talk("{0} tells {1} that they are done with their training.")))
            );
        }
        );

        iWillSpareYou = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => state = State.MERCY),
                new Game.Event(() => QuickCast(new Spare(), Game.Instance.MainCharacter), 2),
                new Game.Event(new RightBox(this, string.Format("And you will do the same.", Game.Instance.MainCharacter.Name))),
                new Game.Event(() => Game.Instance.MainCharacter.Selections[Selection.MERCY].Add(new Spare()))
            );
        }
        );

        spareBackstab = new OneShotProcess(() => {
            Game.Instance.OrderedEvents(
                new Game.Event(() => Game.Instance.Sound.StopAll(), 0.5f),
                new Game.Event(new RightBox(this, string.Format("...", Game.Instance.MainCharacter.Name)), 0)
            );
        }
        );
    }



    public override void Act() {
        base.Act();
        switch (state) {
            case State.REQ_ATTACK:
                attackMePls.Play();
                break;
            case State.REQ_SPELL:
                howAboutASpell.Play();
                if ((Game.Instance.PagePresenter.Page as BattlePage).CurrentSelection == Selection.SPELL) {
                    tfwNoSpells.Play();
                }
                break;
            case State.GIVE_ORB:
                heresYourStuff.Play();
                Equipment e = Game.Instance.MainCharacter.Selections[Selection.EQUIP] as Equipment;
                if (e.ContainsEquipment(EquipmentType.WEAPON) && e.ContainsEquipment(EquipmentType.ARMOR)) {
                    spellTime.Play();
                }
                break;
            case State.REQ_SPELL_2:
                break;
            case State.SANDBOX:
                break;
            case State.MERCY:
                break;
        }
    }

    public override void React(Spell spell, Result res, Calculation calc) {
        switch (state) {
            case State.REQ_ATTACK:
                if (spell.SpellFactory.Name == "Attack") {
                    switch (res.type) {
                        case Result.Type.MISS:
                            niceMissMang.Play();
                            break;
                        case Result.Type.HIT:
                            niceHitMang.Play();
                            break;
                        case Result.Type.CRITICAL:
                            niceCritMang.Play();
                            break;
                    }
                }
                break;
            case State.REQ_SPELL:
                break;
            case State.GIVE_ORB:
                break;
            case State.REQ_SPELL_2:
                if (spell.SpellFactory.Name == "Smite" && calc.TargetResources[ResourceType.HEALTH].False == -Smite.DAMAGE_PER_SKILL * Game.Instance.MainCharacter.GetResourceCount(ResourceType.SKILL, true)) {
                    hitWithSpell.Play();
                }
                break;
            case State.SANDBOX:
                if (spell.SpellFactory.Name == "Talk" && (new Spare()).IsCastable(this)) {
                    iWillSpareYou.Play();
                }
                break;
            case State.MERCY:
                break;
        }
    }

    public override void Witness(Spell spell, Result res, Calculation calc) {
        switch (state) {
            case State.REQ_ATTACK:
                break;
            case State.REQ_SPELL:
                break;
            case State.GIVE_ORB:
                if (spell.SpellFactory.Name == "TimeCapsule") {
                    oldStuffSorry.Play();
                }
                break;
            case State.REQ_SPELL_2:
                break;
            case State.SANDBOX:
                break;
        }
    }

    public override void OnDefeat(bool isSilent = false) {
        base.OnDefeat(isSilent);
        spareBackstab.Play();
    }

    int timesMissed;
    bool youCheckedMe;

    public override void OnBattleStart() {
        base.OnBattleStart();
    }

    public override void OnBattleEnd() {
        base.OnBattleEnd();
    }

    protected override void DecideSpell() {
        if ((Resources[ResourceType.HEALTH].False / Resources[ResourceType.HEALTH].True) < 0.10f) {
            Game.Instance.AddTextBox(new TextBox(string.Format("With a wave of her hand, {0} effortlessly heals herself to full health.", this.Name)));
            Game.Instance.AddTextBox(new RightBox(this, "Did you really think I wouldn't notice you chipping away at me?"));
            AddToResource(ResourceType.HEALTH, false, 100, true);
            Game.Instance.Sound.Play("Sounds/Zip_0");
        } else if (state == State.MERCY) {

        }
    }

    protected override void WhileFullCharge() {
        //throw new NotImplementedException();
    }
}
