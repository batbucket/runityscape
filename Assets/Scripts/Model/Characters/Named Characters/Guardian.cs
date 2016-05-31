using UnityEngine;
using System.Linq;

public class Guardian : ComputerCharacter {
    enum State {
        ATTACK_ME,
        WIELD_EQUIPS,
        ATTACK_ME_2
    }

    State? state;

    readonly OneShotProcess attackMe;

    readonly OneShotProcess reactStart;
    readonly OneShotProcess missReact;
    readonly OneShotProcess hitReact;
    readonly OneShotProcess critReact;
    readonly OneShotProcess reactEnd;

    readonly OneShotProcess wieldEquips;
    readonly OneShotProcess wieldSwordFirst;
    readonly OneShotProcess wieldArmorFirst;

    readonly OneShotProcess reactStart2;
    readonly OneShotProcess attackNoWep;
    readonly OneShotProcess missReact20;
    readonly OneShotProcess missReact21;
    readonly OneShotProcess missReact22;
    readonly OneShotProcess missReact23;
    readonly OneShotProcess hitReact2low;
    readonly OneShotProcess hitReact2;
    readonly OneShotProcess critReact2;
    readonly OneShotProcess reactEnd2;

    public Guardian() : base("placeholder", "Alestre", 10, 10, 10, 10, 10, Color.yellow, 0) {
        state = State.ATTACK_ME;

        attackMe = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(() => Game.Instance.MainCharacter.IsCharging = false),
                    new Game.Event(new RightBox(this, string.Format("Attack me, {0}...", Game.Instance.MainCharacter.Name))),
                    new Game.Event(() => {
                        Game.Instance.MainCharacter.Attack = new Attack();
                        Game.Instance.MainCharacter.IsCharging = true;
                    }, 0)
                );
            }
        );

        reactStart = new OneShotProcess(
            action: () => {
                Game.Instance.MainCharacter.IsCharging = false;
            }
        );

        missReact = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "Did you miss, on purpose...?")),
                    new Game.Event(() => reactEnd.Play())
                );
            }
        );

        hitReact = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "Are you holding back...?")),
                    new Game.Event(() => reactEnd.Play())
                );
            }
        );

        critReact = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "Without hesitation, and yet...")),
                    new Game.Event(() => reactEnd.Play())
                );
            }
        );

        reactEnd = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("You would be more determined if you dressed the part, {0}...", Game.Instance.MainCharacter.Name))),
                    new Game.Event(new RightBox(this, string.Format("...and if you weren't using your fists."))),
                    new Game.Event(() => {
                        Game.Instance.MainCharacter.IsCharging = true;
                        state = State.WIELD_EQUIPS;
                    })
                );
            }
        );

        wieldEquips = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new TextBox("You feel the empty space in your inventory get smaller.")),
                    new Game.Event(() => {
                        Game.Instance.MainCharacter.Selections[Selection.ITEM].Add(new LightSword(1));
                        Game.Instance.MainCharacter.Selections[Selection.ITEM].Add(new LightArmor(1));
                    }),
                    new Game.Event(new RightBox(this, string.Format("They are not paragons of their kinds, but our resources run short.")), 2)
                );
            }
        );

        wieldSwordFirst = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("Even the strongest statue will be worn down over time.")))
                );
            }
        );

        wieldArmorFirst = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("You understand your mortality.")))
                );
            }
        );

        reactStart2 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(() => Game.Instance.MainCharacter.Attack = new Attack()),
                    new Game.Event(new RightBox(this, "Again. With your sword."))
                );
            }
        );

        attackNoWep = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("Do not hold back, {0}."))),
                    new Game.Event(() => reactEnd2.Play())
                );
            }
        );

        missReact20 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "An unlucky occurrance. Try again."))
                );
            }
        );

        missReact21 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "Very unfavorable outcome. Again."))
                );
            }
        );

        missReact22 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "..."))
                );
            }
        );

        missReact23 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "ARE YOU SERIOUS?!"))
                );
            }
        );

        hitReact2low = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("{0} is not {1} there is to an attack.", Util.Color("Power", AttributeType.STRENGTH.Color), Util.Color("all", AttributeType.INTELLIGENCE.Color)))),
                    new Game.Event(() => reactEnd2.Play())
                );
            }
        );

        hitReact2 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("Well done, {0}.", Game.Instance.MainCharacter.Name))),
                    new Game.Event(() => reactEnd2.Play())
                );
            }
        );

        critReact2 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("Your memories are returning, {0}.", Game.Instance.MainCharacter.Name))),
                    new Game.Event(() => reactEnd2.Play())
                );
            }
        );

        reactEnd2 = new OneShotProcess(
            action: () => {
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("You may test out your abilities further if you wish, {0}. Talk to me when you are done.", Game.Instance.MainCharacter.Name))),
                    new Game.Event(() => state = null)
                );
            }
        );
    }

    public override void Act() {
        base.Act();
        switch (state) {
            case State.ATTACK_ME:
                attackMe.Play();
                break;
            case State.WIELD_EQUIPS:
                wieldEquips.Play();
                Equipment e = (Equipment)(Game.Instance.MainCharacter.Selections[Selection.EQUIP]);
                if (e.ContainsEquipment(EquipmentType.WEAPON) && e.ContainsEquipment(EquipmentType.ARMOR)) {
                    state = State.ATTACK_ME_2;
                }
                if (e.ContainsEquipment(EquipmentType.WEAPON) && !e.ContainsEquipment(EquipmentType.ARMOR)) {
                    wieldSwordFirst.Play();
                }
                if (!e.ContainsEquipment(EquipmentType.WEAPON) && e.ContainsEquipment(EquipmentType.ARMOR)) {
                    wieldArmorFirst.Play();
                }
                break;
            case State.ATTACK_ME_2:
                reactStart2.Play();
                break;
        }
    }

    public override void React(Spell spell, Result res, Calculation calc) {
        switch (state) {
            case State.ATTACK_ME:
                if (spell.SpellFactory.Name == "Attack") {
                    Game.Instance.MainCharacter.Attack = null;
                    Game.Instance.MainCharacter.IsCharging = false;
                    switch (res.type) {
                        case Result.Type.MISS:
                            missReact.Play();
                            break;
                        case Result.Type.HIT:
                            hitReact.Play();
                            break;
                        case Result.Type.CRITICAL:
                            critReact.Play();
                            break;
                    }
                }
                break;
            case State.ATTACK_ME_2:
                Equipment e = (Equipment)Game.Instance.MainCharacter.Selections[Selection.EQUIP];
                bool playerEquippedWeapon = e.ContainsEquipment(EquipmentType.WEAPON);
                bool playerEquippedArmor = e.ContainsEquipment(EquipmentType.ARMOR);
                if (!playerEquippedWeapon) {
                    attackNoWep.Play();
                } else {
                    switch (res.type) {
                        case Result.Type.MISS:
                            timesMissed++;
                            if (timesMissed == 1) {
                                missReact20.Play();
                            } else if (timesMissed == 2) {
                                missReact21.Play();
                            } else if (timesMissed == 3) {
                                missReact22.Play();
                            } else {
                                missReact23.Play();
                            }
                            break;
                        case Result.Type.HIT:
                            int damage = (int)-calc.TargetResources[ResourceType.HEALTH].False;
                            if (damage <= 1) {
                                hitReact2low.Play();
                            } else {
                                hitReact2.Play();
                            }
                            break;
                        case Result.Type.CRITICAL:
                            critReact2.Play();
                            break;
                    }
                }
                break;
        }

        if (spell.SpellFactory.Name == "Check" && !youCheckedMe) {
            Debug.Log("lul no reaction yet");
        }
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
        }
    }

    protected override void WhileFullCharge() {
        //throw new NotImplementedException();
    }
}
