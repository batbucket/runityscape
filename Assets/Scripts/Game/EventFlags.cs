public class EventFlags {
    private const int MAX_LENGTH = 50;

    private int[] ints;
    private bool[] bools;

    public int[] Ints {
        get {
            return ints;
        }
    }

    public bool[] Bools {
        get {
            return bools;
        }
    }

    public EventFlags() {
        this.ints = new int[MAX_LENGTH];
        this.bools = new bool[MAX_LENGTH];
    }

    public EventFlags(int[] ints, bool[] bools) {
        this.ints = ints;
        this.bools = bools;
    }
}
