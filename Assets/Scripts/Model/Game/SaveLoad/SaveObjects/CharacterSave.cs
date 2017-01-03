using Scripts.Model.Characters;
using System.Linq;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct CharacterSave : IRestorable<Character> {
        public SpellListSave Actions;
        public SpellSave Attack;
        public AttributeListSave Attributes;
        public EquipmentSave Equipment;
        public string Name;
        public ResourceListSave Resources;
        public SpellListSave Spells;
        public string Type;

        public CharacterSave(Character c) {
            Type = Util.GetClassName(c);
            Name = c.Name;
            Attributes = new AttributeListSave(c.Attributes.Values.ToList());
            Resources = new ResourceListSave(c.Resources.Values.ToList());
            Attack = new SpellSave(c.Attack);
            Spells = new SpellListSave(c.Spells);
            Actions = new SpellListSave(c.Actions);
            Equipment = new EquipmentSave(c.Equipment);
        }

        public Character Restore() {
            Character c = Util.StringToObject<Character>(Type);
            c.Name = this.Name;
            c.Equipment = Equipment.Restore();
            c.Attributes = Attributes.Restore();
            c.Resources = Resources.Restore();
            c.Attack = Attack.Restore();
            c.Spells = Spells.Restore();
            c.Actions = Actions.Restore();
            return c;
        }
    }
}