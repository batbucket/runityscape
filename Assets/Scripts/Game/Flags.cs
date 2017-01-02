public class Flags {

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

    public Flags() {
        this.ints = new int[1000];
        this.bools = new bool[1000];
    }

    public Flags(int[] ints, bool[] bools) {
        this.ints = ints;
        this.bools = bools;
    }
}
