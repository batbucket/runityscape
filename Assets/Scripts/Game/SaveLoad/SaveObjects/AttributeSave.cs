public struct AttributeSave : IRestorable<Attribute> {
    public PairedValueSave Paired;
    public string Type;

    public AttributeSave(Attribute a) {
        Paired = new PairedValueSave(a.True, a.False);
        Type = Util.GetClassName(a);
    }

    public Attribute Restore() {
        Attribute a = Util.StringToObject<Attribute>(Type);
        a.True = Paired.True;
        a.False = Paired.False;
        return a;
    }
}