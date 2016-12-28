using UnityEngine;

/**
 * This class represents a Resource
 * That is dependent on the value of a particular Attribute
 * For example: Health is directly tied to a Character's Vitality
 */
public abstract class DependentResource : Resource {
    Attribute attribute;

    public DependentResource(Attribute attribute, ResourceType type, bool isFalseCappedAtTrue) : base(0, type, isFalseCappedAtTrue) {
        this.attribute = attribute;
    }

    public override void Calculate() {
        Type.Calculation(attribute, this);
    }
}
