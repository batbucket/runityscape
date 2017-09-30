using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Model.Stats {
    public enum BoundType {
        RESOURCE,
        ASSIGNABLE,
        LEVEL
    }

    public struct Bounds {
        public readonly int Low;
        public readonly int High;

        public Bounds(int low, int high) {
            this.Low = low;
            this.High = high;
        }
    }

    public static class StatUtil {
        public static int GetDifference(StatType type, Characters.Stats a, Characters.Stats b) {
            return a.GetStatCount(Characters.Stats.Get.TOTAL, type) - b.GetStatCount(Characters.Stats.Get.TOTAL, type);
        }

        public static string ShowSigns(int num) {
            return num.ToString("+#;-#;0");
        }
    }
}