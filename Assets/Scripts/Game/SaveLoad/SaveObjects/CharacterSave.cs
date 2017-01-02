using System;
using System.Linq;

[System.Serializable]
public struct CharacterSave : IRestorable<Character> {
    public string Type;
    public string Name;
    public AttributeListSave Attributes;
    public ResourceListSave Resources;
    public SpellSave<SpellFactory> Attack;
    public SpellListSave<SpellFactory> Spells;
    public SpellListSave<SpellFactory> Actions;
    public EquipmentSave Equipment;

    public CharacterSave(Character c) {
        Type = Util.GetClassName(c);
        Name = c.Name;
        Attributes = new AttributeListSave(c.Attributes.Values.ToList());
        Resources = new ResourceListSave(c.Resources.Values.ToList());
        Attack = new SpellSave<SpellFactory>(c.Attack);
        Spells = new SpellListSave<SpellFactory>(c.Spells);
        Actions = new SpellListSave<SpellFactory>(c.Actions);
        Equipment = new EquipmentSave(c.Equipment);
    }

    public Character Restore() {
        Character c = Util.StringToObject<Character>(Type);
        c.Name = this.Name;
        c.Attributes = Attributes.Restore();
        c.Resources = Resources.Restore();
        c.Attack = Attack.Restore();
        c.Spells = Spells.Restore();
        c.Actions = Actions.Restore();
        c.Equipment = Equipment.Restore();
        return c;
    }
}
