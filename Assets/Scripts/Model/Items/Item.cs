using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Item : Spell
{
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ALLY;
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    protected int count;

    public Item(Character caster, string name, string description, int count) : base(caster, name, description, SPELL_TYPE, TARGET_TYPE, COSTS) {
        Util.assert(count > 0);
        this.count = count;
    }

    public override string getNameAndInfo() {
        string nameAndCount = string.Format("{0} x {1}", getName(), count);
        return canCast() ? nameAndCount : "<color=red>" + nameAndCount + "</color>";
    }

    public override void consumeResources() {
        base.consumeResources();
        caster.getInventory().remove(this);
    }

    public override void undo() {
        caster.getInventory().add(this);
    }

    public override bool canCast() {
        return base.canCast() && count > 0;
    }

    public void addCount(int amount) {
        Util.assert(amount > 0);
        count += amount;
    }

    public void decCount() {
        Util.assert(count > 0);
        count--;
    }

    public int getCount() {
        return count;
    }
}
