using System;
using Scripts.Model.Characters;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;

namespace Scripts.Game.Serialized {

    /// <summary>
    /// Used for saving the world (entire game state).
    /// </summary>
    /// <seealso cref="Scripts.Model.SaveLoad.ISaveable{Scripts.Model.SaveLoad.SaveObjects.WorldSave}" />
    public class World : ISaveable<WorldSave> {
        /// <summary>
        /// The party
        /// </summary>
        public Party Party;
        /// <summary>
        /// The flags
        /// </summary>
        public Flags Flags;

        /// <summary>
        /// Gets the save object. A save object contains the neccessary
        /// information to initialize a clean class to its saved state.
        /// A save object is also serializable.
        /// </summary>
        /// <returns></returns>
        public WorldSave GetSaveObject() {
            return new WorldSave(
                Party.GetSaveObject(),
                Flags.GetSaveObject());
        }

        /// <summary>
        /// Initializes from save object.
        /// </summary>
        /// <param name="saveObject">The save object.</param>
        public void InitFromSaveObject(WorldSave saveObject) {
            Party restoredParty = new Party();
            restoredParty.InitFromSaveObject(saveObject.Party);
            this.Party = restoredParty;
            this.Flags = saveObject.Flags.Flags;
            Flags.InitFromSaveObject(saveObject.Flags);
        }
    }
}