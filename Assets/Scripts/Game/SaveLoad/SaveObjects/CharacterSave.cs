[System.Serializable]
public struct CharacterSave {
    public string Name;
    public AttributeListSave Attributes;
    public ResourceListSave Resources;
    public SpellSave Attack;
    public SpellListSave Spells;
    public SpellListSave Actions;
    public EquipmentSave Equipment;
}
