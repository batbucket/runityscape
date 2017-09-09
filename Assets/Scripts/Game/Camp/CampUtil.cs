using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Stats;
using System.Linq;
using UnityEngine;

namespace Scripts.Game.Pages {

    /// <summary>
    /// Utility methods for Camp.
    /// </summary>
    public static class CampUtil {
        /// <summary>
        /// Missing percentage to restore when resting
        /// </summary>
        private const float MISSING_RESTORE_PERCENTAGE = .2f;

        /// <summary>
        /// Minimum amount a stat is restored by.
        /// </summary>
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

        /// <summary>
        /// Attempts to remove buffs by dispelling them.
        /// </summary>
        /// <param name="partyMemberBuffs">Buffs of a party member.</param>
        public static void CureMostBuffs(Buffs partyMemberBuffs) {
            Buff[] allBuffs = partyMemberBuffs.ToArray();
            foreach (Buff buff in allBuffs) {
                partyMemberBuffs.RemoveBuff(RemovalType.DISPEL, buff);
            }
        }
    }
}