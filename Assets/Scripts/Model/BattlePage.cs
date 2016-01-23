using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BattlePage : Page {
    public const int CHARGE_PER_TICK = 1;

    public BattlePage(string text = "", string tooltip = "", List<Character> left = null, List<Character> right = null,
        Action onFirstEnter = null, Action onEnter = null, Action onFirstExit = null, Action onExit = null,
        List<Process> actionGrid = null) : base(text, tooltip, left, right, onFirstEnter, onEnter, onFirstExit, onExit, actionGrid, null) {

    }

    void battleTick() {

    }

    public override void tick() {
        base.tick();
    }

    void updateCharacters(List<Character> characters) {
        foreach (Character c in characters) {
            int chargeNeededToAct = (int)(120 * ((float)(leftCharacters[0].getAttribute(AttributeType.DEXTERITY).getFalse())) / c.getAttribute(AttributeType.DEXTERITY).getFalse());
            c.getResource(ResourceType.CHARGE).setTrue(chargeNeededToAct);
            c.act(CHARGE_PER_TICK, null);
        }
    }
}
