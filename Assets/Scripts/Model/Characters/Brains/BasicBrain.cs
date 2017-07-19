using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Spells;
using System;
using System.Linq;

namespace Scripts.Model.Characters {
    public abstract class BasicBrain : Brain {
        private static readonly SpellBook DEFAULT_SPELL = new Wait();

        public sealed override void DetermineAction() {
            handlePlay(GetPlay());
        }

        protected abstract IPlayable GetPlay();

        protected IPlayable CastOnRandom(SpellBook sb) {
            return CastOnTargetMeetingCondition(sb, c => true);
        }

        // performs wait if sb is not castable for any reason
        protected IPlayable CastOnTargetMeetingCondition(SpellBook sb, Func<Character, bool> requirement) {
            Character specificTarget =
                sb
                .TargetType
                .GetTargets(owner.Character, currentBattle)
                .Where(
                    c => sb.IsCastable(owner, new SpellParams(c, currentBattle)) && requirement(c))
                .PickRandom();

            if (specificTarget != null) {
                SpellBook castableSpell = null;
                if (sb.IsCastable(owner, new SpellParams(specificTarget, currentBattle))) {
                    castableSpell = sb;
                } else {
                    castableSpell = DEFAULT_SPELL;
                }
                return owner.Spells.CreateSpell(sb, owner, new SpellParams(specificTarget, currentBattle));
            }
            return null;
        }
    }
}