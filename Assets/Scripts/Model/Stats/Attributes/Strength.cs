using UnityEngine;
using System;

public class Strength : Attribute {
    public const string SHORT_DESCRIPTION = "Increases basic attack damage.";
    public const string LONG_DESCRIPTION_PRIMARY = "Increases basic attack damage and minimum spell effects.";
    public const string LONG_DESCRIPTION_SECOND = "Increases health.";
    public static readonly Color ASSOCIATED_COLOR = Color.red;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.STRENGTH;

    public override Color getColor() {
        return ASSOCIATED_COLOR;
    }

    public override string getShortDescription() {
        return SHORT_DESCRIPTION;
    }

    public override string getLongDescription() {
        return "PRIMARY: " + LONG_DESCRIPTION_PRIMARY + "\nSECONDARY: " + LONG_DESCRIPTION_SECOND;
    }

    public override AttributeType getAttributeType() {
        return ATTRIBUTE_TYPE;
    }
}
