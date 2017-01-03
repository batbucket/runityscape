using Scripts.Model.Characters;
using System.Collections.Generic;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct PartySave : IRestorable<Party> {
        public InventorySave Inventory;
        public CharacterSave Leader;
        public CharacterSave[] NonleaderMembers;

        public PartySave(Party p) {
            Leader = new CharacterSave(p.Leader);
            NonleaderMembers = new CharacterSave[p.Members.Count - 1];
            int index = 0;
            foreach (Character c in p.Members) {
                if (c != p.Leader) {
                    NonleaderMembers[index++] = new CharacterSave(c);
                }
            }
            Inventory = new InventorySave(p.Inventory);
        }

        public Party Restore() {
            Character restoredLeader = Leader.Restore();
            restoredLeader.Inventory = Inventory.Restore();
            IList<Character> restoredNonleaderMembers = new List<Character>();
            foreach (CharacterSave c in NonleaderMembers) {
                restoredNonleaderMembers.Add(c.Restore());
            }
            return new Party(restoredLeader, restoredNonleaderMembers);
        }
    }
}