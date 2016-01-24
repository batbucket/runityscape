using UnityEngine;
using System.Collections;
using System;

public class Health : Resource {
    public const string DESCRIPTION = "Replenished by some spells and items.";
    public const string NAME = "Health";
    public const string SHORT_NAME = "HP";
    public static readonly Color OVER_COLOR = Color.green;
    public static readonly Color UNDER_COLOR = Color.red;
    public const ResourceType RESOURCE_TYPE = ResourceType.HEALTH;

    public override Color OverColor { get { return OVER_COLOR; } }
    public override Color UnderColor { get { return UNDER_COLOR; } }
    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string Description { get { return DESCRIPTION; } }
    public override ResourceType Type { get { return RESOURCE_TYPE; } }
}
