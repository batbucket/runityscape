using UnityEngine;
using System.Collections;
using System;

public abstract class ConsumableItem : Item
{
    protected bool consumed;

    public ConsumableItem(Character caster, string name) : base(caster, name) {
    }

    public override void onFailure(Game game)
    {
        throw new NotImplementedException();
    }

    public virtual void onSuccess(Game game)
    {
        consumed = true;
    }

    public override void calculateSuccessRate()
    {
        successRate = consumed ? 0 : 1; //wow rohan's first ternary expression ^(0u0)^
    }
    public virtual void undo(Game game)
    {
        consumed = false;
    }
}
