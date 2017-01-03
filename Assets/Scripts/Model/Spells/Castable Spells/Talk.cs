namespace Scripts.Model.Spells.Named {

    public class Talk : SpellFactory {
        public const string NAME = "Talk";
        public const SpellType SPELL_TYPE = SpellType.ACT;
        public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
        public static readonly string DESCRIPTION = string.Format("Attempt to talk to a foe.");
        private string text;

        public Talk(string text) : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE) {
            this.text = text;
        }

        public override Hit CreateHit() {
            return new Hit(
                createText: (c, t, calc, o) => {
                    return string.Format(text, c.DisplayName, t.DisplayName);
                }
            );
        }
    }
}