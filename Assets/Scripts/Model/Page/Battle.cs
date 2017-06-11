using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Presenter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Pages {

    public class Battle : Page {
        private const string DEATH_MESSAGE = "[{0}] has <color=red>bit the dust</color>.";

        private int turnCount;

        public Battle(string location, IList<Character> left, IList<Character> right) : base(location) {
            turnCount = 0;
            foreach (Character c in left) {
                Left.Add(c);
            }
            foreach (Character c in right) {
                Right.Add(c);
            }
            OnEnter += () => Game.Instance.StartCoroutine(startBattle());
        }

        private bool IsResolved {
            get {
                return Left.All(c => c.Stats.State == State.DEAD) || Right.All(c => c.Stats.State == State.DEAD);
            }
        }

        private IEnumerator startBattle() {
            AddText("The battle begins.");
            while (!IsResolved) {
                yield return startRound();
                yield return new WaitForSeconds(1);
            }
            AddText(string.Format("The [{1}] side is victorious after [{0}] turns.", turnCount, Left.All(c => c.Stats.State == State.DEAD) ? "left" : "right"));
        }

        private IEnumerator startRound() {
            AddText(string.Format(Util.ColorString("<Turn {0} START>", Color.grey), turnCount));
            List<IPlayable> plays = new List<IPlayable>();
            List<Character> chars = GetAll();
            HashSet<Character> set = new HashSet<Character>();
            AddText(string.Format(Util.ColorString("<Setup Phase>", Color.grey)));
            for (int i = 0; i < chars.Count; i++) {
                yield return new WaitForSeconds(0.25f);
                Character c = chars[i];
                if (c.Stats.State == State.ALIVE) {
                    AddText(string.Format("[{0}] is thinking...", c.Look.DisplayName));
                    c.Brain.PageSetup(this);
                    c.Brain.DetermineAction((p) => {
                        if (!set.Contains(c)) {
                            plays.Add(p);
                            set.Add(c);
                            ClearActionGrid();
                        } else {
                            Util.Assert(false, string.Format("{0}'s brain adds more than one IPlayable objects in its DetermineAction().", c.Look.DisplayName));
                        }
                    });
                    while (plays.Count <= i) {
                        yield return null;
                    }
                }
            }
            plays.Sort();
            AddText(string.Format(Util.ColorString("<Action Phase>", Color.grey)));
            for (int i = 0; i < plays.Count; i++) {
                yield return new WaitForSeconds(0.25f);
                IPlayable play = plays[i];
                if (play.IsPlayable) { // Death check
                    AddText(play.Text);
                    yield return play.Play();

                    Character caster = play.MySpell.Caster;
                    Character target = play.MySpell.Target;

                    // Caster somehow kills themself, don't say they died twice
                    if (caster.Equals(target) && caster.Stats.State == State.DEAD) {
                        AddText(string.Format(DEATH_MESSAGE, play.MySpell.Caster.Look.DisplayName));
                    } else {
                        if (caster.Stats.State == State.DEAD) {
                            AddText(string.Format(DEATH_MESSAGE, play.MySpell.Caster.Look.DisplayName));
                        }
                        if (target.Stats.State == State.DEAD) {
                            AddText(string.Format(DEATH_MESSAGE, play.MySpell.Target.Look.DisplayName));
                        }
                    }
                }
            }
            //End of round buff interactions
            AddText(string.Format(Util.ColorString("<Buff Phase>", Color.grey)));
            foreach (Character c in chars) {
                yield return new WaitForSeconds(0.25f);
                Characters.Buffs buffs = c.Buffs;
                ICollection<Buff> buffCollection = buffs.Collection;
                IList<Buff> timedOut = new List<Buff>();
                foreach (Buff myB in buffCollection) {
                    yield return new WaitForSeconds(0.1f);
                    Buff b = myB;
                    AddText(string.Format("[{0}] is affected by [{1}].", c.Look.DisplayName, b.Name));
                    b.OnEndOfTurn();
                    if (b.IsTimedOut) {
                        timedOut.Add(b);
                    }
                }
                foreach (Buff myB in timedOut) {
                    yield return new WaitForSeconds(0.1f);
                    Buff b = myB;
                    AddText(string.Format("[{0}]'s [{1}] fades.", c.Look.DisplayName, b.Name));
                    buffs.RemoveBuff(RemovalType.TIMED_OUT, b);
                }
            }
            AddText(string.Format(Util.ColorString("<Turn {0} END>", Color.grey), turnCount));
            turnCount++;
        }
    }
}