using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Spells {
    public class Result {
        public ResultType Type;
        public IList<IEnumerator> SFX;
        public IList<SpellEffect> Effects;

        public Result() {
            this.Effects = new List<SpellEffect>();
            this.SFX = new List<IEnumerator>();
        }

        public void AddEffect(SpellEffect se) {
            Effects.Add(se);
        }

        public void AddSFX(IEnumerator sfx) {
            SFX.Add(sfx);
        }

        public void AddSFX(ICollection<IEnumerator> sfx) {
            foreach (IEnumerator item in sfx) {
                SFX.Add(item);
            }
        }
    }
}