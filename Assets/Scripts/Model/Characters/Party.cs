using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Represents a grouping of the user's Characters.
    /// Everyone's inventory references the leader's inventory upon
    /// being added to a party.
    /// </summary>
    public class Party : IEnumerable<Character>, ISaveable<PartySave> {
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
            throw new NotImplementedException();
        }

        public void InitFromSaveObject(PartySave saveObject) {
            throw new NotImplementedException();
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