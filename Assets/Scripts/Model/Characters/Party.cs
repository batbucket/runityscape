using Scripts.Model.Buffs;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Represents a grouping of the user's Characters.
    /// Everyone's inventory references the leader's inventory upon
    /// being added to a party.
    /// </summary>
    public class Party : IEnumerable<Character>, ISaveable<PartySave, PartySave> {
        private HashSet<Character> members;
        private Inventory shared;

        public Party() {
            this.members = new HashSet<Character>();
            this.shared = new Inventory();
        }

        public void AddMember(Character c) {
            this.members.Add(c);
        }

        public PartySave GetSaveObject() {
            InventorySave inventory = shared.GetSaveObject();
            List<PartialCharacterSave> mems = members.Select(c => c.GetSaveObject()).ToList();
            List<FullCharacterSave> fullChars = new List<FullCharacterSave>();

            // Go through all characters
            for (int i = 0; i < mems.Count; i++) {
                PartialCharacterSave c = mems[i];
                List<FullBuffSave> buffList = new List<FullBuffSave>();

                //Go through particular character's buffs
                foreach (PartialBuffSave pb in c.Buffs) {
                    FullBuffSave fb = null;
                    bool isPartyMemberFound = false;

                    // Search party list to see if any party member is the caster
                    for (int j = 0; j < mems.Count && !isPartyMemberFound; i++) {

                        // Id match -- Party ref mode
                        if (mems[j].Id.Equals(pb.CasterCharacterId)) {
                            fb = new FullBuffSave(pb.Type, pb.TurnsRemaining, j);
                            isPartyMemberFound = true;
                        }
                    }

                    // No member found, -- Use stat values mode
                    if (!isPartyMemberFound) {
                        fb = new FullBuffSave(pb.Type, pb.TurnsRemaining, pb.StatCopy);
                    }

                    buffList.Add(fb);
                }

                fullChars.Add(new FullCharacterSave(c.Stats, new FullCharacterBuffsSave(buffList), c.Look, c.Spells, c.Brain, c.Equipment));
            }

            return new PartySave(fullChars, inventory);
        }

        public void InitFromSaveObject(PartySave saveObject) {
            List<Character> memList = new List<Character>();

            // Inventory setup
            this.shared.InitFromSaveObject(saveObject.Inventory);

            // Setup everything EXCEPT buffs
            for (int i = 0; i < saveObject.Characters.Count; i++) {
                FullCharacterSave charSave = saveObject.Characters[i];
                Character restored = new Character(this.shared); // Inventory
                restored.InitFromSaveObject(charSave); // Everything else but buffs

                memList.Add(restored);
            }

            // Setup buffs (which need members inited to make)
            // memList index should match saveObject.Characters index!
            for (int i = 0; i < memList.Count; i++) {
                Character c = memList[i];
                c.Buffs.SetupTemporarySaveFields(memList);
                c.Buffs.InitFromSaveObject(saveObject.Characters[i].Buffs); // Assumes matching indices
            }
        }

        public void RemoveMember(Character c) {
            members.Remove(c);
        }

        IEnumerator<Character> IEnumerable<Character>.GetEnumerator() {
            return members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return members.GetEnumerator();
        }
    }
}