[System.Serializable]
public struct PairedValueSave {
    public int True;
    public float False;

    public PairedValueSave(int _true, float _false) {
        True = _true;
        False = _false;
    }
}