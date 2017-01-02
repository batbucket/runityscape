[System.Serializable]
public struct EventFlagsSave : IRestorable<EventFlags> {
    public int[] Ints;
    public bool[] Bools;

    public EventFlagsSave(EventFlags f) {
        Ints = f.Ints;
        Bools = f.Bools;
    }

    public EventFlags Restore() {
        return new EventFlags(Ints, Bools);
    }
}