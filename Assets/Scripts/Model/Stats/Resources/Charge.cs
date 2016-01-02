using UnityEngine;
using System.Collections;
using System;

public class Charge : Resource {
    public const string DESCRIPTION = "Consumed on an action. Regenerates quickly over time.";
    public const string LONG_NAME = "Charge Time";
    public const string SHORT_NAME = "CT";
    public static readonly Color OVER_COLOR = Color.white;
    public static readonly Color UNDER_COLOR = Color.gray;
    public const ResourceType RESOURCE_TYPE = ResourceType.CHARGE;

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
