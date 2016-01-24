using UnityEngine;
using System;

public class Strength : Attribute {
    public const string NAME = "Strength";
    public const string SHORT_NAME = "STR";
    public const string DESCRIPTION_PRIMARY = "Increases basic attack damage and minimum spell effects.";
    public const string DESCRIPTION_SECOND = "Increases health.";
    public const string SHORT_DESCRIPTION = "Increases basic attack damage.";
    public static readonly Color ASSOCIATED_COLOR = Color.red;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.STRENGTH;

    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string PrimaryDescription { get { return DESCRIPTION_PRIMARY; } }
    public override string SecondaryDescription { get { return DESCRIPTION_SECOND; } }
    public override string ShortDescription { get { return SHORT_DESCRIPTION; } }
    public override AttributeType Type { get { return ATTRIBUTE_TYPE; } }
    public override Color Color { get { return ASSOCIATED_COLOR; } }
}
