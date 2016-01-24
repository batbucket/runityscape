using System;
using UnityEngine;

public class Skill : Resource {
    public const string DESCRIPTION = "Regenerates on basic attacks. Used for some spells.";
    public const string NAME = "Skill";
    public const string SHORT_NAME = "SP";
    public static readonly Color OVER_COLOR = Color.yellow;
    public static readonly Color UNDER_COLOR = Color.black;
    public const ResourceType RESOURCE_TYPE = ResourceType.SKILL;

    public override Color OverColor { get { return OVER_COLOR; } }
    public override Color UnderColor { get { return UNDER_COLOR; } }
    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string Description { get { return DESCRIPTION; } }
    public override ResourceType Type { get { return RESOURCE_TYPE; } }
}
