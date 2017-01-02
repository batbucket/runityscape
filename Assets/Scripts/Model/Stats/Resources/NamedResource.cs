using UnityEngine;
using System.Collections;

public sealed class NamedResource {
    public sealed class Health : DependentResource {
        public Health() : base(AttributeType.VITALITY, ResourceType.HEALTH, true) { }
    }

    public sealed class Mana : DependentResource {
        public Mana() : base(AttributeType.INTELLIGENCE, ResourceType.MANA, true) { }
    }

    public sealed class Skill : Resource {
        public Skill() : base(0, ResourceType.SKILL, true) { }
    }

    public sealed class Charge : Resource {
        public Charge() : base(0, ResourceType.CHARGE, true) {
            this.False = 0;
        }
    }

    public sealed class Corruption : Resource {
        public Corruption(int maxCap) : base(maxCap, ResourceType.CORRUPTION, true) {
            this.False = 0;
        }
    }

    public sealed class Experience : DependentResource {
        public Experience() : base(AttributeType.LEVEL, ResourceType.EXPERIENCE, false) {
            this.False = 0;
        }
    }

    public sealed class DeathExperience : Resource {
        public DeathExperience(int amount) : base(0, amount, ResourceType.DEATH_EXP, false) {
            this.IsVisible = false;
        }
    }
}