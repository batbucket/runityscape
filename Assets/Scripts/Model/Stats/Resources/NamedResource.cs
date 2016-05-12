using UnityEngine;
using System.Collections;

public sealed class NamedResource {
    public class Health : DependentResource {
        public Health(NamedAttribute.Vitality vitality) : base(vitality, ResourceType.HEALTH) { }
    }

    public class Mana : DependentResource {
        public Mana(NamedAttribute.Intelligence intelligence) : base(intelligence, ResourceType.MANA) { }
    }

    public class Skill : Resource {
        public Skill() : base(0, ResourceType.SKILL) { }
    }

    public class Charge : Resource {
        public Charge() : base(0, ResourceType.CHARGE) {
            this.False = 0;
        }
    }
}