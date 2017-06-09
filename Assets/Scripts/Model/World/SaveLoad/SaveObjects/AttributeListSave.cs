using Scripts.Model.Characters;
using Scripts.Model.Stats;
using System.Collections.Generic;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct AttributeListSave : IRestorable<Characters.Stats> {
        public StatSave[] Stats;

        public AttributeListSave(IList<Stat> list) {
            Stats = new StatSave[list.Count];

            int index = 0;
            foreach (Stat s in list) {
                Stats[index++] = new StatSave(s);
            }
        }

        public Characters.Stats Restore() {
            Characters.Stats stats = new Characters.Stats();
            foreach (StatSave s in Stats) {
                Stat restoredStat = s.Restore();
                stats.AddStat(restoredStat);
            }
            return stats;
        }
    }
}