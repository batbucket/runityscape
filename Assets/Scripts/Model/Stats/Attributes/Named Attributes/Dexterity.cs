using UnityEngine;
using System;

public class Dexterity : Attribute {
    public const string NAME = "Dexterity";
    public const string SHORT_NAME = "DEX";
    public const string DESCRIPTION_PRIMARY = "Increases Charge Time regeneration and evasion rate.";
    public const string DESCRIPTION_SECOND = "Increases critical hit rate and accuracy.";
    public const string SHORT_DESCRIPTION = "Increases Action Point regeneration.";
    public static readonly Color ASSOCIATED_COLOR = Color.green;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.DEXTERITY;

    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string PrimaryDescription { get { return DESCRIPTION_PRIMARY; } }
    public override string SecondaryDescription { get { return DESCRIPTION_SECOND; } }
    public override string ShortDescription { get { return SHORT_DESCRIPTION; } }
    public override AttributeType Type { get { return ATTRIBUTE_TYPE; } }
    public override Color Color { get { return ASSOCIATED_COLOR; } }
}
