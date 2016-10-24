public sealed class NamedAttribute {
    public class Level : Attribute {
        public const int LEVEL_CAP = 99;
        public Level() : base(LEVEL_CAP, AttributeType.LEVEL) {
            this.False = 1;
        }
    }

    public class Strength : Attribute {
        public Strength(int initial) : base(initial, AttributeType.STRENGTH) { }
    }

    public class Intelligence : Attribute {
        public Intelligence(int initial) : base(initial, AttributeType.INTELLIGENCE) { }
    }

    public class Dexterity : Attribute {
        public Dexterity(int initial) : base(initial, AttributeType.DEXTERITY) { }
    }

    public class Vitality : Attribute {
        public Vitality(int initial) : base(initial, AttributeType.VITALITY) { }
    }
}
