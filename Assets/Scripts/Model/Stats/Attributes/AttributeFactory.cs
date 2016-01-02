using UnityEngine;
using System.Collections;

public static class AttributeFactory {
    public static Attribute createAttribute(AttributeType attributeType, int value) {
        Attribute attribute = null;
        switch(attributeType) {
            case AttributeType.STRENGTH:
                attribute = new Strength();
                break;
            case AttributeType.INTELLIGENCE:
                attribute = new Intelligence();
                break;
            case AttributeType.DEXTERITY:
                attribute = new Dexterity();
                break;
            case AttributeType.VITALITY:
                attribute = new Vitality();
                break;
        }
        attribute.setBoth(value);
        return attribute;
    }
}
