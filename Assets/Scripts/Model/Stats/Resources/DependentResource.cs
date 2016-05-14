using UnityEngine;

/**
 * This class represents a Resource
 * That is dependent on the value of a particular Attribute
 * For example: Health is directly tied to a Character's Vitality
 */
public abstract class DependentResource : Resource {
    Attribute attribute;

    public override sealed float False { get { return base.False; } set { base.False = Mathf.Clamp(value, LESSER_CAP, True); } }
    public override sealed int True { get { return base.True; } set { base.True = Mathf.Clamp(value, LESSER_CAP, 99999); } }

    public DependentResource(Attribute attribute, ResourceType type) : base(0, type) {
        this.attribute = attribute;
    }

    public override void Calculate() {
        Type.Calculation(attribute, this);
    }
}
