namespace Scripts.Model.Spells.Named {

    public class Spare : SpellFactory {
        public const string NAME = "Spare";
        public const SpellType SPELL_TYPE = SpellType.MERCY;
        public const string SUCCESS_TEXT = "{0} is sparing {1}.";
        public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
        public static readonly string DESCRIPTION = string.Format("Attempt to spare a foe.");

        public Spare() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE) {
        }

        public override Hit CreateHit() {
            return new Hit(
                sound: (c, t, calc) => {
                    return "Blip_0";
                }
            );
        }
    }
}