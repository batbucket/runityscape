using UnityEngine;
using System.Collections;
using System;

public class Illusion : SpellFactory {
    private readonly int str;
    private readonly int intel;
    private readonly int dex;
    private readonly int vit;

    public Illusion(int str, int intel, int dex, int vit) : base("Illusion", "Pretend to have less stats.", SpellType.BOOST, TargetType.SELF, abbreviation: "???", color: Color.white) {
        this.str = str;
        this.intel = intel;
        this.dex = dex;
        this.vit = vit;
    }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            isIndefinite: (c, t, o) => t.State == CharacterState.ALIVE || t.State == CharacterState.DEFEAT,
            onStart: (c, t, o) => {
                t.AddToAttribute(AttributeType.STRENGTH, false, -t.GetAttributeCount(AttributeType.STRENGTH, false) + str);
                t.AddToAttribute(AttributeType.INTELLIGENCE, false, -t.GetAttributeCount(AttributeType.INTELLIGENCE, false) + intel);
                t.AddToAttribute(AttributeType.AGILITY, false, -t.GetAttributeCount(AttributeType.AGILITY, false) + dex);
                t.AddToAttribute(AttributeType.VITALITY, false, -t.GetAttributeCount(AttributeType.VITALITY, false) + vit);
            },
            onEnd: (c, t, o) => {
                t.State = CharacterState.ALIVE;
                t.Presenter.PortraitView.ClearEffects();
                t.AddToAttribute(AttributeType.STRENGTH, false, t.GetAttributeCount(AttributeType.STRENGTH, true) - str);
                t.AddToAttribute(AttributeType.INTELLIGENCE, false, t.GetAttributeCount(AttributeType.INTELLIGENCE, true) - intel);
                t.AddToAttribute(AttributeType.AGILITY, false, t.GetAttributeCount(AttributeType.AGILITY, true) - dex);
                t.AddToAttribute(AttributeType.VITALITY, false, t.GetAttributeCount(AttributeType.VITALITY, true) - vit);
                Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0}'s illusion was shattered!", t.DisplayName), TextEffect.FADE_IN));
                t.AddToResource(ResourceType.HEALTH, false, t.GetResourceCount(ResourceType.HEALTH, true) - t.GetResourceCount(ResourceType.HEALTH, false));
                t.AddToResource(ResourceType.CHARGE, false, t.GetResourceCount(ResourceType.CHARGE, true));
            },
            sfx: (c, t, calc, o) => new CharacterEffect[0]
            );
    }
}
