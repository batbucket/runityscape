using UnityEngine;
using System.Collections;

public class Skill : Resource {
    public const string DESCRIPTION = "Regenerates on basic attacks. Used for some spells.";
    public const string LONG_NAME = "Skill";
    public const string SHORT_NAME = "SP";
    public static readonly Color OVER_COLOR = Color.yellow; //Formerly Util.hexToColor("FFA500")
    public static readonly Color UNDER_COLOR = Color.black;
    public const ResourceType RESOURCE_TYPE = ResourceType.SKILL;

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
