using System;
using Scripts.Model.Characters;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;

namespace Scripts.Game.Serialized {

    public class World : ISaveable<WorldSave> {
        public Party Party;
        public Flags Flags;

        public WorldSave GetSaveObject() {
            return new WorldSave(
                Party.GetSaveObject(),
                Flags.GetSaveObject());
        }

        public void InitFromSaveObject(WorldSave saveObject) {
            Party restoredParty = new Party();
            restoredParty.InitFromSaveObject(saveObject.Party);
            this.Party = restoredParty;
            this.Flags = saveObject.Flags.Flags;
            Flags.InitFromSaveObject(saveObject.Flags);
        }
    }
}