using Scripts.Model.Spells;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct SpellSave : IRestorable<SpellFactory> {
        public string Name;

        public SpellSave(SpellFactory s) {
            Name = Util.GetClassName(s);
        }

        public SpellFactory Restore() {
            return Util.StringToObject<SpellFactory>(Name);
        }
    }
}