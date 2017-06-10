using Scripts.Model.Characters;
using System.Linq;

using UnityEngine;

namespace Scripts.Model.Stats {
    public class Strength : Stat {
        public Strength(float mod, int max) : base(mod, max, StatType.STRENGTH) { }
    }

    public class Agility : Stat {
        public Agility(float mod, int max) : base(mod, max, StatType.AGILITY) { }
    }

    public class Intellect : Stat {
        public Intellect(float mod, int max) : base(mod, max, StatType.INTELLECT) { }
    }

    public class Vitality : Stat {
        public Vitality(float mod, int max) : base(mod, max, StatType.VITALITY) { }
    }

    public class Health : Stat {
        private const int VIT_TO_HEALTH = 10;

        public Health(float mod, int max) : base(mod, max, StatType.HEALTH) { }

        public override void Update(Character holderOfThisAttribute) {
            this.Max = holderOfThisAttribute.Stats.GetStatCount(Value.MOD, StatType.VITALITY) * VIT_TO_HEALTH;
        }
    }

    public class Skill : Stat {
        public Skill(float mod, int max) : base(mod, max, StatType.SKILL) { }
        public override void Update(Character c) {
            this.Max = c.Spells.Set.Select(s => s.Costs[StatType.SKILL]).Max();
        }
    }

    public class Experience : Stat {
        public Experience(float mod, int max) : base(mod, max, StatType.EXPERIENCE) { }
        public override void Update(Character c) {
            Max = 1 + (int)Mathf.Pow(2, c.Stats.GetStatCount(Value.MOD, StatType.LEVEL));
        }
    }

    public class Level : Stat {
        public Level(float mod) : base(mod, StatType.LEVEL.Bounds.High, StatType.LEVEL) { }
        public override void Update(Character c) {

        }
    }
}