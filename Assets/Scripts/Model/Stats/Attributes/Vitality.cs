using UnityEngine;
using System;

public class Vitality : Attribute {
    public const string SHORT_DESCRIPTION = "Increases health.";
    public const string LONG_DESCRIPTION_PRIMARY = "Increases health and healing recieved.";
    public const string LONG_DESCRIPTION_SECOND = "Increases armor and magical resistances.";
    public static readonly Color ASSOCIATED_COLOR = Color.magenta;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.VITALITY;

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
