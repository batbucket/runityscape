using UnityEngine;
using System;

public class Vitality : Attribute {
    public const string NAME = "Vitality";
    public const string SHORT_NAME = "VIT";
    public const string DESCRIPTION_PRIMARY = "Increases health and healing recieved.";
    public const string DESCRIPTION_SECOND = "Increases armor and magical resistances.";
    public const string SHORT_DESCRIPTION = "Increases health.";
    public static readonly Color ASSOCIATED_COLOR = Color.magenta;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.VITALITY;

    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string PrimaryDescription { get { return DESCRIPTION_PRIMARY; } }
    public override string SecondaryDescription { get { return DESCRIPTION_SECOND; } }
    public override string ShortDescription { get { return SHORT_DESCRIPTION; } }
    public override AttributeType Type { get { return ATTRIBUTE_TYPE; } }
    public override Color Color { get { return ASSOCIATED_COLOR; } }
}
