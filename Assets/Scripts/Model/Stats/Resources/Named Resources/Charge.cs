using UnityEngine;
using System.Collections;
using System;

public class Charge : Resource {
    public const string DESCRIPTION = "Consumed on an action. Regenerates quickly over time.";
    public const string NAME = "Charge Time";
    public const string SHORT_NAME = "CT";
    public static readonly Color OVER_COLOR = Color.white;
    public static readonly Color UNDER_COLOR = Color.gray;
    public const ResourceType RESOURCE_TYPE = ResourceType.CHARGE;

    public override Color OverColor { get { return OVER_COLOR; } }
    public override Color UnderColor { get { return UNDER_COLOR; } }
    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string Description { get { return DESCRIPTION; } }
    public override ResourceType Type { get { return RESOURCE_TYPE; } }
}
