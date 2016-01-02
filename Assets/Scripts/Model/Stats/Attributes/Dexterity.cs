using UnityEngine;
using System;

public class Dexterity : Attribute {
    public const string SHORT_DESCRIPTION = "Increases Action Point regeneration.";
    public const string LONG_DESCRIPTION_PRIMARY = "Increases Charge Time regeneration and evasion rate.";
    public const string LONG_DESCRIPTION_SECOND = "Increases critical hit rate and accuracy.";
    public static readonly Color ASSOCIATED_COLOR = Color.green;
    public const AttributeType ATTRIBUTE_TYPE = AttributeType.DEXTERITY;

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
