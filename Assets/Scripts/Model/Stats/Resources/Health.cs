using UnityEngine;
using System.Collections;
using System;

public class Health : Resource {
    public const string DESCRIPTION = "Replenished by some spells and items.";
    public const string LONG_NAME = "Health";
    public const string SHORT_NAME = "HP";
    public static readonly Color OVER_COLOR = Color.green;
    public static readonly Color UNDER_COLOR = Color.red;
    public const ResourceType RESOURCE_TYPE = ResourceType.HEALTH;

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
