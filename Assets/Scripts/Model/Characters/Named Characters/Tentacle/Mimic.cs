using UnityEngine;
using System.Collections;

/// <summary>
/// Don't have a mortal damage state.
/// Can be summoned.
/// </summary>
public abstract class Mimic : ComputerCharacter {
    protected bool isTransformed;
    private const float TRANSFORM_LIFE_PERCENT = 0.5f;
    private Displays real;

    public Mimic(Displays real, Displays fake, Attributes attributes)
        : base(fake, attributes, 3) {
        this.real = real;
    }

    protected override void Act() {
        base.Act();
        if (((float)GetResourceCount(ResourceType.HEALTH, false) / GetResourceCount(ResourceType.HEALTH, true)) <= TRANSFORM_LIFE_PERCENT && !isTransformed && State == CharacterState.NORMAL) {
            Transform();
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} sheds its disguise!", DisplayName)));
        }
    }

    private void Transform() {
        isTransformed = true;
        this.SpriteLoc = real.Loc;
        this.Name = real.Name;
        this.checkText = real.Check;
    }

    public override void OnDefeat() {
        base.OnKill();
        this.State = CharacterState.KILLED;
    }

    public abstract Mimic Summon();
}
