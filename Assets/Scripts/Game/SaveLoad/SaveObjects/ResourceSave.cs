public struct ResourceSave {
    public bool IsDependent {
        get {
            return string.IsNullOrEmpty(DependentAttribute);
        }
    }

    public PairedValueSave Paired;
    public string ResourceName;
    public string DependentAttribute;
}