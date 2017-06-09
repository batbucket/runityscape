namespace Scripts.Model.Characters {
    public static class CharacterList {
        public class Kitsune : Character {
            public Kitsune() : base(new StatList.Kitsune(), new LookList.Kitsune(), new BrainList.Player(), new CharacterSpellsList.DebugSpells()) { }
        }
    }
}