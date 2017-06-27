using Scripts.Model.Characters;
using Scripts.Model.Stats;
using Scripts.Presenter;
using System.Linq;

using UnityEngine;

namespace Scripts.Game.Defined.Serialized.Statistics {
    public class Strength : Stat {
        public Strength(int mod, int max) : base(mod, max, StatType.STRENGTH) { }

        public Strength() : base(0, 0, StatType.STRENGTH) { }
    }

    public class Agility : Stat {
        public Agility(int mod, int max) : base(mod, max, StatType.AGILITY) { }

        public Agility() : base(0, 0, StatType.AGILITY) { }
    }

    public class Intellect : Stat {
        public Intellect(int mod, int max) : base(mod, max, StatType.INTELLECT) { }

        public Intellect() : base(0, 0, StatType.INTELLECT) { }
    }

    public class Vitality : Stat {
        public Vitality(int mod, int max) : base(mod, max, StatType.VITALITY) { }

        public Vitality() : base(0, 0, StatType.VITALITY) { }
    }

    public class Health : Stat {
        private const int VIT_TO_HEALTH = 10;

        public Health(int mod, int max) : base(mod, max, StatType.HEALTH) { }

        public Health() : base(0, 0, StatType.HEALTH) { }

        public override void Update(Character holderOfThisAttribute) {
            this.Max = holderOfThisAttribute.Stats.GetStatCount(Stats.Get.MOD, StatType.VITALITY) * VIT_TO_HEALTH;
        }
    }

    public class Skill : Stat {
        public Skill(int mod, int max) : base(mod, max, StatType.SKILL) { }

        public Skill() : base(0, 0, StatType.SKILL) { }

        public override void Update(Character c) {
            this.Max = c.Spells.HighestSkillCost;
        }
    }

    public class Experience : Stat {
        public Experience(int mod) : base(mod, 0, StatType.EXPERIENCE) { }

        public Experience() : base(0, 0, StatType.EXPERIENCE) { }
        public override void Update(Character c) {
            Max = GetExpForLevel(c.Stats.Level);
            if (c.Stats.CanLevelUp) {
                DoLevelUp(c.Stats);
                Main.Instance.Sound.PlaySound("orchestra");
            }
        }

        public static int GetExpForLevel(int level) {
            return 1 + (int)Mathf.Pow(2, level);
        }

        public static int GetExpDiffForLevel(int levelCurrent, int levelNext) {
            return GetExpForLevel(levelNext) - GetExpForLevel(levelCurrent);
        }

        private void DoLevelUp(Stats stats) {
            Mod = 0;
            stats.StatPoints++;
            stats.Level++;
            stats.AddSplat(new Presenter.SplatDetails(Color.white, "LEVEL+"));
            DoLevelUpStatBoost(stats, 1);
        }

        private static void DoLevelUpStatBoost(Stats stats, int amount) {
            foreach (StatType assignable in StatType.ASSIGNABLES) {
                stats.AddToStat(assignable, Stats.Set.MAX, amount);
            }
        }
    }
}