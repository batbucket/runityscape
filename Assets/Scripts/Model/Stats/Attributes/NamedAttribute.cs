public sealed class NamedAttribute {
    public class Level : Attribute {
        public const int LEVEL_CAP = 99;
        public Level(int level) : base(LEVEL_CAP, AttributeType.LEVEL) {
            this.False = level;
        }

        public Level() : this(Attribute.LESSER_CAP) { }
    }

    public class Strength : Attribute {
        public Strength(int initial) : base(initial, AttributeType.STRENGTH) { }
        public Strength() : this(Attribute.LESSER_CAP) { }
    }

    public class Intelligence : Attribute {
        public Intelligence(int initial) : base(initial, AttributeType.INTELLIGENCE) { }
        public Intelligence() : this(Attribute.LESSER_CAP) { }
    }

    public class Agility : Attribute {
        public Agility(int initial) : base(initial, AttributeType.AGILITY) { }
        public Agility() : this(Attribute.LESSER_CAP) { }
    }

    public class Vitality : Attribute {
        public Vitality(int initial) : base(initial, AttributeType.VITALITY) { }
        public Vitality() : this(Attribute.LESSER_CAP) { }
    }
}
