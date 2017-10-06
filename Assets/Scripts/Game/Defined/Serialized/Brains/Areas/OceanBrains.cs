using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System;
using System.Collections.Generic;

namespace Scripts.Game.Serialized.Brains {

    public class BlackShuck : PriorityBrain {
        public static readonly SetupCounter COUNTER = new SetupCounter();
        public static readonly Attack ATTACK = new Attack();

        protected override IList<Func<IPlayable>> SetupPriorityPlays() {
            return new Func<IPlayable>[] {
                    CastOnRandom(COUNTER),
                    CastOnRandom(ATTACK)
                };
        }

        public override string StartOfRoundDialogue() {
            if (currentBattle.TurnCount == 0) {
                return "(It barks menacingly at you.)";
            }
            return string.Empty;
        }
    }

    public class Siren : BasicBrain {

        public static readonly SpellBook[] DEBUFF_LIST = new SpellBook[] {
            new SingStrengthSong(),
            new SingAgilitySong(),
            new SingIntellectSong(),
            new SingVitalitySong()
        };

        protected override IPlayable GetPlay() {
            return CastOnRandom(DEBUFF_LIST.ChooseRandom());
        }
    }
}