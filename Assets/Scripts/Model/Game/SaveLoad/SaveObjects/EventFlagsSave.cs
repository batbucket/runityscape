

using Scripts.Model.World.Flags;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct EventFlagsSave : IRestorable<EventFlags> {
        public bool[] Bools;
        public int[] Ints;

        public EventFlagsSave(EventFlags f) {
            Ints = f.Ints;
            Bools = f.Bools;
        }

        public EventFlags Restore() {
            return new EventFlags(Ints, Bools);
        }
    }
}