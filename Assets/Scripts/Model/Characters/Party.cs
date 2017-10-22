using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Represents a grouping of the user's Characters.
    /// Everyone's inventory references the leader's inventory upon
    /// being added to a party.
    /// </summary>
    public class Party : IEnumerable<Character>, ISaveable<PartySave> {

        /// <summary>
        /// This being a set prevents multiples of the same character from being added.
        /// </summary>
        private HashSet<Character> members;

        /// <summary>
        /// Shared inventory among all members.
        /// </summary>
        private Inventory shared;

        /// <summary>
        /// Construct a party with empty fields.
        /// </summary>
        public Party() {
            this.members = new HashSet<Character>(new IdNumberEqualityComparer<Character>());
            this.shared = new Inventory();
        }

        /// <summary>
        /// Get the shared inventory.
        /// </summary>
        public Inventory Shared {
            get {
                return shared;
            }
        }

        /// <summary>
        /// Get the default character, good if you need any party member for something.
        /// </summary>
        public Character Default {
            get {
                return members.FirstOrDefault();
            }
        }

        /// <summary>
        /// Grabbing the party members should be done through this,
        /// since it cannot be modified.
        /// </summary>
        public ICollection<Character> Collection {
            get {
                return new ReadOnlyCollection<Character>(members.ToList());
            }
        }

        public Character GetCharacter(Func<Character, bool> predicate) {
            return members.Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Adds a member and makes its inventory
        /// reference the shared one.
        /// </summary>
        /// <param name="partyMemberToAdd"></param>
        public void AddMember(Character partyMemberToAdd) {
            this.members.Add(partyMemberToAdd);
            partyMemberToAdd.Inventory = Shared;
        }

        /// <summary>
        /// Converts the party into a serializable object.
        /// </summary>
        /// <returns>A serializable object representing the state of the party.</returns>
        public PartySave GetSaveObject() {
            InventorySave inventory = Shared.GetSaveObject();
            List<CharacterSave> mems = members.Select(c => c.GetSaveObject()).ToList();

            // Go through all characters
            for (int i = 0; i < mems.Count; i++) {
                CharacterSave c = mems[i];

                //Go through particular character's buffs
                foreach (BuffSave pb in c.Buffs) {
                    bool isPartyMemberFound = false;

                    // Search party list to see if any party member is the caster
                    for (int j = 0; j < mems.Count && !isPartyMemberFound; j++) {
                        // Id match -- Ref party member mode: we want to reference the member again on Load.
                        if (mems[j].Id.Equals(pb.CasterCharacterId)) {
                            pb.SetupAsCasterInParty(j);
                            isPartyMemberFound = true;
                        }
                    }

                    // Caster is not a party member -- Spoof stats mode: we want to reference the caster's stats only to avoid infinite loops.
                    if (!isPartyMemberFound) {
                        pb.SetupAsCasterNotInParty();
                    }
                }
            }

            return new PartySave(mems, inventory);
        }

        /// <summary>
        /// Setup the PartySave from the serialized object.
        /// </summary>
        /// <param name="saveObject">Save object representing the state of the party.</param>
        public void InitFromSaveObject(PartySave saveObject) {
            List<Character> memList = new List<Character>();

            // Inventory setup
            this.Shared.InitFromSaveObject(saveObject.Inventory);

            // Setup everything EXCEPT buffs
            for (int i = 0; i < saveObject.Characters.Count; i++) {
                CharacterSave charSave = saveObject.Characters[i];
                Character restored = new Character(this.Shared); // Inventory
                restored.InitFromSaveObject(charSave); // Everything else but buffs

                memList.Add(restored);
            }

            // Setup buffs (which need members inited to make)
            // memList index should match saveObject.Characters index!
            for (int i = 0; i < memList.Count; i++) {
                Character c = memList[i];
                c.Equipment.SetupTemporarySaveFields(memList);
                c.Equipment.InitFromSaveObject(saveObject.Characters[i].Equipment);
                c.Buffs.SetupTemporarySaveFields(memList);
                c.Buffs.InitFromSaveObject(saveObject.Characters[i].Buffs); // Assumes matching indices
            }

            // Add members
            foreach (Character c in memList) {
                members.Add(c);
            }
        }

        /// <summary>
        /// Removes the character from the party.
        /// </summary>
        /// <param name="memberToRemove">Character to be removed.</param>
        public void RemoveMember(Character memberToRemove) {
            members.Remove(memberToRemove);
        }

        /// <summary>
        /// Checks if characters are equal and inventories.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var item = obj as Party;

            if (item == null) {
                return false;
            }
            HashSet<Character> thisSet = new HashSet<Character>(members);
            HashSet<Character> thatSet = new HashSet<Character>(item.members);

            return this.Shared.Equals(item.Shared) && thisSet.SetEquals(thatSet);
        }

        /// <summary>
        /// Party has no hashable content since every element can change.
        /// </summary>
        /// <returns>Hash</returns>
        public override int GetHashCode() {
            return 0;
        }

        IEnumerator<Character> IEnumerable<Character>.GetEnumerator() {
            return members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return members.GetEnumerator();
        }
    }
}