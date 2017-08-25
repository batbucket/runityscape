using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Dungeons {
    public struct Encounter {
        public readonly Character[] Enemies;
        public readonly Music Music;

        public Encounter(Character[] enemies, Music music) {
            this.Enemies = enemies;
            this.Music = music;
        }
    }
}