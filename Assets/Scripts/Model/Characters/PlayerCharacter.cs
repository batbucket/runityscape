namespace Scripts.Model.Characters {

    /// <summary>
    /// Character who is commandable on a BattlePage by the user.
    /// </summary>
    public abstract class PlayerCharacter : Character {
        public const bool DISPLAYABLE = true;

        public PlayerCharacter(Displays displays, StartStats attributes, Items items)
            : base(DISPLAYABLE, displays, attributes, items) {
            this.IsShowingBarCounts = true;
        }

        public PlayerCharacter(Displays displays, StartStats attributes)
            : base(DISPLAYABLE, displays, attributes) {
            this.IsShowingBarCounts = true;
        }

        protected override void Act() {
        }

        protected override void WhileFullCharge() {
        }
    }
}