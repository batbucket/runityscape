using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Stats;
using System.Linq;
using UnityEngine;

namespace Scripts.Game.Pages {
    public static class CampUtil {
        private const float MISSING_RESTORE_PERCENTAGE = .2f;
        private const int MINIMUM_RESTORE_AMOUNT = 1;

        public static void RestoreStats(StatType type, Stats stats, bool isLastTime) {
            int missing = stats.GetStatCount(Stats.Get.MAX, type) - stats.GetStatCount(Stats.Get.MOD, type);
            int restoreAmount = 0;
            if (!isLastTime) {
                restoreAmount = Mathf.Max((int)(missing * MISSING_RESTORE_PERCENTAGE), MINIMUM_RESTORE_AMOUNT);
            } else {
                restoreAmount = missing;
            }
            if (missing > 0) {
                stats.AddToStat(type, Stats.Set.MOD, restoreAmount);
            }
        }

        public static void CureMostBuffs(Buffs buffs) {
            Buff[] allBuffs = buffs.ToArray();
            foreach (Buff buff in allBuffs) {
                buffs.RemoveBuff(RemovalType.DISPEL, buff);
            }
        }
    }
}