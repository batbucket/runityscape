using UnityEngine;
using System.Collections;
using System;

public class Guardian : ComputerCharacter {
    enum State {
        TELL_ATTACK,
        REACT_ATTACK,
        TELL_WIELD,
        WAIT_WIELD,
        REACT_WIELD,
        SANDBOX,
        TALKED_TO,
        SPARING_PLAYER
    }

    State? state;

    public Guardian() : base(Util.GetSprite("placeholder"), "Alestre", 10, 10, 10, 10, 10, Color.yellow, 0) {
        state = State.TELL_ATTACK;
    }

    public override void Act() {
        base.Act();
        switch (state) {
            case State.TELL_ATTACK:
                state = null;
                Game.Instance.OrderedEvents(
                    new Game.Event(() => Game.Instance.MainCharacter.IsCharging = false),
                    new Game.Event(new RightBox(this, string.Format("Do not hold back on your attack, Redeemer {0}...", Game.Instance.MainCharacter.Name)), 0),
                    new Game.Event(() => {
                        Game.Instance.MainCharacter.Attack = new Attack();
                        Game.Instance.MainCharacter.IsCharging = true;
                        Game.Instance.Sound.Play("Music/Hero Immortal");
                        state = State.REACT_ATTACK;
                    }, 0)
                );

                break;
            case State.TELL_WIELD:
                state = null;
                Game.Instance.OrderedEvents(
                    new Game.Event(new TextBox("You can feel the empty space in your inventory get smaller.")),
                    new Game.Event(() => {
                        Game.Instance.MainCharacter.Selections[Selection.ITEM].Add(new LightSword(1));
                        Game.Instance.MainCharacter.Selections[Selection.ITEM].Add(new LightArmor(1));
                    }),
                    new Game.Event(new RightBox(this, string.Format("They are not paragons of their kinds, but our resources run short.")), 2),
                    new Game.Event(() => state = State.WAIT_WIELD)
                );
                break;
        }

        Equipment e = (Equipment)Game.Instance.MainCharacter.Selections[Selection.EQUIP];
        bool playerEquippedWeapon = e.ContainsEquipment(EquipmentType.WEAPON);
        bool playerEquippedArmor = e.ContainsEquipment(EquipmentType.ARMOR);

        if (state == State.WAIT_WIELD && playerEquippedWeapon && playerEquippedArmor) {
            state = State.REACT_WIELD;
            Game.Instance.OrderedEvents(
                new Game.Event(new RightBox(this, "Again with that sword. Do not hold back.")),
                new Game.Event(() => Game.Instance.MainCharacter.Attack = new Attack())
            );
        }
        if (state == State.REACT_WIELD) {
            if (!playerEquippedArmor && !wtfArmor) {
                wtfArmor = true;
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, "Is it uncomfortable? That is your own choice."))
                );
            }
        }
    }

    bool wtfArmor;
    bool wtfWeapon;

    public override void React(Spell spell, Result res, Calculation calc) {
        if (spell.SpellFactory.Name == "Attack") {
            if (state == State.REACT_ATTACK) {
                Game.Instance.MainCharacter.IsCharging = false;
                TextBox t = null;
                switch (res.type) {
                    case Result.Type.MISS:
                        t = new RightBox(this, "Did you miss... on purpose?");
                        break;
                    case Result.Type.HIT:
                        t = new RightBox(this, "It seems you held back...");
                        break;
                    case Result.Type.CRITICAL:
                        t = new RightBox(this, "Even without hesitation...");
                        break;
                };
                Game.Instance.OrderedEvents(
                    new Game.Event(t, 1),
                    new Game.Event(new RightBox(this, "Perhaps you would be more determined if you dressed the part...")),
                    new Game.Event(new RightBox(this, "...and if you weren't using your fists.")),
                    new Game.Event(() => {
                        Game.Instance.MainCharacter.IsCharging = true;
                        Game.Instance.MainCharacter.Attack = null;
                        state = State.TELL_WIELD;
                    })
                );
            }

            Equipment e = (Equipment)Game.Instance.MainCharacter.Selections[Selection.EQUIP];
            bool playerEquippedWeapon = e.ContainsEquipment(EquipmentType.WEAPON);
            bool playerEquippedArmor = e.ContainsEquipment(EquipmentType.ARMOR);
            if (state == State.REACT_WIELD) {
                if (!playerEquippedWeapon) {
                    Game.Instance.OrderedEvents(
                        new Game.Event(new RightBox(this, "Attack with the weapon. Redeemer."))
                    );
                } else {
                    if (res.type == Result.Type.MISS) {
                        timesMissed++;
                        if (timesMissed == 1) {
                            Game.Instance.OrderedEvents(new Game.Event(new RightBox(this, "Again.")));
                        } else if (timesMissed == 2) {
                            Game.Instance.OrderedEvents(new Game.Event(new RightBox(this, "Unfortunate.")));
                        } else if (timesMissed == 3) {
                            Game.Instance.OrderedEvents(new Game.Event(new RightBox(this, "...")));
                        } else {
                            Game.Instance.OrderedEvents(new Game.Event(new RightBox(this, "ARE YOU SERIOUS?!")));
                        }
                    } else {
                        Game.Instance.MainCharacter.IsCharging = false;
                        TextBox t = null;
                        switch (res.type) {
                            case Result.Type.HIT:
                                int damage = -(int)calc.TargetResources[ResourceType.HEALTH].False;
                                Debug.Log(damage);
                                if (damage <= 1) {
                                    t = new RightBox(this, "<color=red>Brute force</color> is not all there is to an attack. <color=blue>Knowledge</color> of when and where to strike is just as valuable. The weapon you hold cannot improve that.");
                                } else {
                                    t = new RightBox(this, string.Format("Well done, {0}.", Game.Instance.MainCharacter.Name));
                                }
                                break;
                            case Result.Type.CRITICAL:
                                t = new RightBox(this, "A perfect strike.");
                                break;
                        }
                        Game.Instance.OrderedEvents(
                            new Game.Event(t, 1),
                            new Game.Event(new RightBox(this, "You may do whatever you want now, Redeemer. Perform the action of talking to me when you are done.")),
                            new Game.Event(() => {
                                Game.Instance.MainCharacter.IsCharging = true;
                                state = State.SANDBOX;
                            })
                        );
                    }
                }
            }
        }
        if (spell.SpellFactory.Name == "Check" && !youCheckedMe) {
            youCheckedMe = true;
            Game.Instance.OrderedEvents(
                new Game.Event(new RightBox(this, string.Format("Do not be intimidated, Redeemer {0}.", Game.Instance.MainCharacter.Name)))
            );
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
            Game.Instance.AddTextBox(new RightBox(this, "Did you really think I'd let you kill me?"));
            AddToResource(ResourceType.HEALTH, false, 100, true);
            Game.Instance.Sound.Play("Sounds/Zip_0");
        }
    }

    protected override void WhileFullCharge() {
        //throw new NotImplementedException();
    }
}
