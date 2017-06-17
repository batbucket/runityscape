namespace Scripts.Model.Spells {
    public sealed class ResultType {
        public readonly string Text;

        private ResultType(string text) {
            this.Text = text;
        }

        public static readonly ResultType HIT = new ResultType("");
        public static readonly ResultType CRITICAL = new ResultType("\nA critical strike!");
        public static readonly ResultType MISS = new ResultType("\nBut it misses!");
        public static readonly ResultType FAILED = new ResultType("\nBut it failed!");

        public override bool Equals(object obj) {
            return obj == this;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}