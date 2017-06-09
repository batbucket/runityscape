using Scripts.Model.Stats;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct StatSave : IRestorable<Stat> {
        public float Mod;
        public int Max;
        public string Type;

        public StatSave(Stat s) {
            Mod = s.Mod;
            Max = s.Max;
            Type = Util.GetClassName(s);
        }

        public Stat Restore() {
            Stat s = Util.StringToObject<Stat>(Type);
            s.Max = Max;
            s.SetMod(Mod, false);
            return s;
        }
    }
}