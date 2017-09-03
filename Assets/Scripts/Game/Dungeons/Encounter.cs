using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Dungeons {
    public struct Encounter {
        public readonly Character[] Enemies;
        public readonly Music Music;

        public Encounter(Music music, params Character[] enemies) {
            this.Enemies = enemies;
            this.Music = music;
        }

        public Encounter(params Character[] enemies) : this(Music.NORMAL, enemies) {}
    }
}