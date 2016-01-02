using UnityEngine;
using System.Collections;

public class Mana : Resource {
    public const string DESCRIPTION = "Needed for some spells. Replenishes over time.";
    public const string LONG_NAME = "Mana";
    public const string SHORT_NAME = "MP";
    public static readonly Color OVER_COLOR = Color.blue;
    public static readonly Color UNDER_COLOR = Util.hexToColor("8B7B8B"); //Violet
    public const ResourceType RESOURCE_TYPE = ResourceType.MANA;

    public override string getDescription() {
        return DESCRIPTION;
    }

    public override string getLongName() {
        return LONG_NAME;
    }

    public override string getShortName() {
        return SHORT_NAME;
    }

    public override Color getOverColor() {
        return OVER_COLOR;
    }

    public override Color getUnderColor() {
        return UNDER_COLOR;
    }

    public override ResourceType getResourceType() {
        return RESOURCE_TYPE;
    }
}
