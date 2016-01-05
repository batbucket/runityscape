using UnityEngine;
using System.Collections;

public class Steve : PlayerCharacter {
    public Steve() : base(Util.getSprite("laughing_shinx"), "Steve", 0, 5, 5, 5, 5) {
        addResources(ResourceFactory.createResource(ResourceType.SKILL, 3));
        getFight().Add("Attack");
        getFight().Add("Meditate");
    }
}
