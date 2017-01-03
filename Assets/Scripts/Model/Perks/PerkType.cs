namespace Scripts.Model.Perks {

    public sealed class PerkType {

        public enum Character {
            SLEEP,
            TICK,
            ON_FULL_CHARGE,
            BATTLE_START,
            BATTLE_END,
            BATTLE_VICTORY,
            SELF_DEFEAT,
            SELF_KILLED,
            ENEMY_DEFEAT,
            ENEMY_KILLED
        }

        public enum React {
            RECIEVE_SPELL,
            CAST_SPELL
        }
    }
}