using System;
using Scripts.Model.Characters;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;

namespace Scripts.Game.Serialized {

    public class Game : ISaveable<GameSave> {
        public Party Party;
        public Flags Flags;

        public GameSave GetSaveObject() {
            return new GameSave(
                Party.GetSaveObject(),
                Flags);
        }

        public void InitFromSaveObject(GameSave saveObject) {
            Party restoredParty = new Party();
            restoredParty.InitFromSaveObject(saveObject.Party);
            this.Party = restoredParty;
            this.Flags = saveObject.Flags;
        }
    }
}