using Scripts.Model.Characters;
using Scripts.Model.World.Flags;
using Scripts.Model.World.Pages;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct GameSave : IRestorable<Camp> {
        public EventFlagsSave Flags;
        public PartySave Party;

        public GameSave(Camp camp) {
            Party = new PartySave(camp.Party);
            Flags = new EventFlagsSave(camp.Flags);
        }

        public Camp Restore() {
            Party p = Party.Restore();
            EventFlags f = Flags.Restore();
            return new Camp(p, f);
        }
    }
}