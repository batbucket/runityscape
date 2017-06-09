using Scripts.Model.Characters;
using System.Linq;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct CharacterSave : IRestorable<Character> {
        public AttributeListSave Attributes;
        public string Name;
        public string Type;

        public CharacterSave(Character c) {
            Type = Util.GetClassName(c);
            Name = c.Look.Name;
            Attributes = new AttributeListSave(c.Stats.Dict.Values.ToList());
        }

        public Character Restore() {
            Character c = Util.StringToObject<Character>(Type);
            c.Look.Name = this.Name;
            return c;
        }
    }
}