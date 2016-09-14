using UnityEngine;
using System.Collections;

public sealed class NamedResource {
    public sealed class Health : DependentResource {
        public Health(NamedAttribute.Vitality vitality) : base(vitality, ResourceType.HEALTH) { }
    }

    public sealed class Mana : DependentResource {
        public Mana(NamedAttribute.Intelligence intelligence) : base(intelligence, ResourceType.MANA) { }
    }

    public sealed class Skill : Resource {
        public Skill() : base(0, ResourceType.SKILL) { }
    }

    public sealed class Charge : Resource {
        public Charge() : base(0, ResourceType.CHARGE) {
            this.False = 0;
        }
    }

    public sealed class Corruption : Resource {
        public Corruption(int maxCap) : base(maxCap, ResourceType.CORRUPTION) {
            this.False = 0;
        }
    }
}