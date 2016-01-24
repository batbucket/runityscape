using UnityEngine;
using System.Collections;

public class Mana : Resource {
    public const string DESCRIPTION = "Needed for some spells. Replenishes over time.";
    public const string NAME = "Mana";
    public const string SHORT_NAME = "MP";
    public static readonly Color OVER_COLOR = Color.blue;
    public static readonly Color UNDER_COLOR = Util.HexToColor("8B7B8B"); //Violet
    public const ResourceType RESOURCE_TYPE = ResourceType.MANA;

    public override Color OverColor { get { return OVER_COLOR; } }
    public override Color UnderColor { get { return UNDER_COLOR; } }
    public override string Name { get { return NAME; } }
    public override string ShortName { get { return SHORT_NAME; } }
    public override string Description { get { return DESCRIPTION; } }
    public override ResourceType Type { get { return RESOURCE_TYPE; } }
}
