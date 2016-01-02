using UnityEngine;
using System;

public class Intelligence : Attribute {
    public const string SHORT_DESCRIPTION = "Increases spell effects.";
    public const string LONG_DESCRIPTION_PRIMARY = "Increases spell effects and minimum basic attack damage.";
    public const string LONG_DESCRIPTION_SECOND = "Increases critical hit rate.";
    public static readonly Color ASSOCIATED_COLOR = Color.blue;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.INTELLIGENCE;

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
