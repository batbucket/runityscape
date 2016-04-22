public class Scimitar : EquippableItem {
    const string NAME = "D. Scimitar";
    const string DESCRIPTION = "A vicious, curved sword.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

    //Bonus stats go here

    public Scimitar(int count) : base(NAME, DESCRIPTION, count, EQUIPMENT_TYPE) { }
}