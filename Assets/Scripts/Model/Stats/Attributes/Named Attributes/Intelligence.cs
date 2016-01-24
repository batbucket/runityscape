using UnityEngine;
using System;

public class Intelligence : Attribute {
    public const string NAME = "Intelligence";
    public const string SHORT_NAME = "INT";
    public const string DESCRIPTION_PRIMARY = "Increases spell effects and minimum basic attack damage.";
    public const string DESCRIPTION_SECOND = "Increases critical hit rate.";
    public const string SHORT_DESCRIPTION = "Increases spell effects.";
    public static readonly Color ASSOCIATED_COLOR = Color.blue;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.INTELLIGENCE;

    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string PrimaryDescription { get { return DESCRIPTION_PRIMARY; } }
    public override string SecondaryDescription { get { return DESCRIPTION_SECOND; } }
    public override string ShortDescription { get { return SHORT_DESCRIPTION; } }
    public override AttributeType Type { get { return ATTRIBUTE_TYPE; } }
    public override Color Color { get { return ASSOCIATED_COLOR; } }
}
