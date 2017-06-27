using Scripts.Game.Defined.Serialized.Items.Consumables;
using Scripts.Game.Defined.Serialized.Items.Equipment;
using Scripts.Game.Defined.Serialized.Items.Misc;
using Scripts.Model.Buffs;
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
        public HashSet<Character> members;
        public Inventory shared;

        public Party() {
            this.members = new HashSet<Character>(new IdNumberEqualityComparer<Character>());
            this.shared = new Inventory();

            this.shared.Add(new Apple());
            this.shared.Add(new Apple());
            this.shared.Add(new Apple());
            this.shared.Add(new Apple());
            this.shared.Add(new PoisonArmor());
            this.shared.Add(new Money(), 100);
        }

        public Character Default {
            get {
                return members.FirstOrDefault();
            }
        }

        public ICollection<Character> Collection {
            get {
                return new ReadOnlyCollection<Character>(members.ToList());
            }
        }

        public void AddMember(Character c) {
            this.members.Add(c);
            c.Inventory = shared;
        }

        public PartySave GetSaveObject() {
            InventorySave inventory = shared.GetSaveObject();
            List<CharacterSave> mems = members.Select(c => c.GetSaveObject()).ToList();

            // Go through all characters
            for (int i = 0; i < mems.Count; i++) {
                CharacterSave c = mems[i];

                //Go through particular character's buffs
                foreach (BuffSave pb in c.Buffs) {
                    bool isPartyMemberFound = false;

                    // Search party list to see if any party member is the caster
                    for (int j = 0; j < mems.Count && !isPartyMemberFound; j++) {

                        // Id match -- Party ref mode
                        if (mems[j].Id.Equals(pb.CasterCharacterId)) {
                            pb.SetupAsCasterInParty(j);
                            isPartyMemberFound = true;
                        }
                    }

                    // No member found, -- Use stat values mode
                    if (!isPartyMemberFound) {
                        pb.SetupAsCasterNotInParty();
                    }
                }
            }

            return new PartySave(mems, inventory);
        }

        public void InitFromSaveObject(PartySave saveObject) {
            List<Character> memList = new List<Character>();

            // Inventory setup
            this.shared.InitFromSaveObject(saveObject.Inventory);

            // Setup everything EXCEPT buffs
            for (int i = 0; i < saveObject.Characters.Count; i++) {
                CharacterSave charSave = saveObject.Characters[i];
                Character restored = new Character(this.shared); // Inventory
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

            foreach (Character c in memList) {
                members.Add(c);
            }
        }

        public void RemoveMember(Character c) {
            members.Remove(c);
        }

        public override bool Equals(object obj) {
            var item = obj as Party;

            if (item == null) {
                return false;
            }
            HashSet<Character> thisSet = new HashSet<Character>(members);
            HashSet<Character> thatSet = new HashSet<Character>(item.members);

            return this.shared.Equals(item.shared) && thisSet.SetEquals(thatSet);
        }

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