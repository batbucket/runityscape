namespace Scripts.Model.Stats {

    /// <summary>
    /// Integer paired with a float.
    /// </summary>
    public class PairedValue {
        private float _false;
        private int _true;

        public virtual float False { get { return _false; } set { _false = value; } }
        public virtual int True { get { return _true; } set { _true = value; } }

        public PairedValue(int initial) : this(initial, initial) {
        }

        public PairedValue(int trueValue, int falseValue) {
            _true = trueValue;
            _false = falseValue;
        }
    }
}