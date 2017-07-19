using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Spells {
    public class Result {
        public ResultType Type;

        private readonly List<IEnumerator> sfx;
        private readonly List<SpellEffect> effects;

        public Result() {
            this.effects = new List<SpellEffect>();
            this.sfx = new List<IEnumerator>();
        }

        public IList<IEnumerator> SFX {
            get {
                return sfx;
            }
        }
        public IList<SpellEffect> Effects {
            get {
                return effects;
            }
        }

        public void AddEffect(SpellEffect se) {
            Effects.Add(se);
        }

        public void AddEffects(IEnumerable<SpellEffect> effects) {
            this.effects.AddRange(effects);
        }

        public void AddSFX(IEnumerator sfx) {
            SFX.Add(sfx);
        }

        public void AddSFX(ICollection<IEnumerator> sfx) {
            this.sfx.AddRange(sfx);
        }
    }
}