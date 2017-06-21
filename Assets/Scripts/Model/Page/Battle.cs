using Scripts.Game.Defined.Serialized.Characters;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Pages {

    public class Battle : Page {
        private const string BATTLE_START = "The battle begins.";
        private const string ROUND_START = "Turn {0}";
        private const string PLAYER_QUESTION = "What will <color=yellow>{0}</color> do?";
        private const string RESOLVED = "The <color=yellow>{1}</color> side is victorious after <color=yellow>{0}</color> turns.";
        private const string CHARACTER_DEATH = "<color=yellow>{0}</color> has been <color=red>defeated.</color>.";
        private const string BUFF_AFFECT = "<color=yellow>{0}</color> is affected by <color=yellow>{1}</color>.";
        private const string BUFF_FADE = "<color=yellow>{0}</color>'s {1} fades.";

        private int turnCount;

        public Battle(string location, IList<Character> left, IList<Character> right) : base(location) {
            turnCount = 0;
            foreach (Character c in left) {
                Left.Add(c);
            }
            foreach (Character c in right) {
                Right.Add(c);
            }
            OnEnter += () => Presenter.Main.Instance.StartCoroutine(startBattle());
        }

        private bool IsResolved {
            get {
                return Left.All(c => c.Stats.State == State.DEAD) || Right.All(c => c.Stats.State == State.DEAD);
            }
        }

        private IEnumerator startBattle() {
            AddText(BATTLE_START);
            while (!IsResolved) {
                yield return startRound();
                yield return new WaitForSeconds(1);
            }
            AddText(string.Format(RESOLVED, turnCount, Left.All(c => c.Stats.State == State.DEAD) ? "right" : "left"));
        }

        private IEnumerator startRound() {
            AddText(string.Format(Util.ColorString(ROUND_START, Color.grey), turnCount));
            List<IPlayable> plays = new List<IPlayable>();
            List<Character> chars = GetAll();
            HashSet<Character> playerActionSet = new HashSet<Character>();

            yield return DetermineCharacterActions(chars, plays, playerActionSet);
            yield return PerformActions(plays);
            yield return GoThroughBuffs(chars);
            EndRound();
        }

        private IEnumerator DetermineCharacterActions(IList<Character> chars, IList<IPlayable> plays, HashSet<Character> playerActionSet) {
            ICollection<PooledBehaviour> declarations = new List<PooledBehaviour>();
            for (int i = 0; i < chars.Count; i++) {
                yield return new WaitForSeconds(0.25f);
                Character c = chars[i];

                if (c.Stats.State == State.ALIVE) {
                    PooledBehaviour textbox = null;

                    // Only show when player controlled is asked to make a move
                    bool brainIsPlayer = (c.Brain is Player);

                    // Helps show user which character is doing stuff
                    if (brainIsPlayer) {
                        textbox = AddText(string.Format(PLAYER_QUESTION, c.Look.DisplayName));
                    }

                    // Wait until this is true until we ask the next character what action they want to perform
                    bool isActionTaken = false;
                    c.Brain.PageSetup(this);

                    // Pass to the Brain a func that lets that lets them pass us what action they want to take
                    c.Brain.DetermineAction((p) => {
                        if (!playerActionSet.Contains(c)) {
                            plays.Add(p);
                            playerActionSet.Add(c);
                            ClearActionGrid();
                            isActionTaken = true;
                        } else {
                            Util.Assert(false, string.Format("{0}'s brain adds more than one IPlayable objects in its DetermineAction().", c.Look.DisplayName));
                        }
                    });

                    // Wait until they choose a move. (This so the player can wait as long as they want)
                    yield return new WaitWhile(() => !isActionTaken);

                    ObjectPoolManager.Instance.Return(textbox); // Remove "What will X do?" textbox, reduce clutter

                    if (brainIsPlayer) {
                        Spell spell = plays.Last().MySpell;
                        declarations.Add(AddText(spell.SpellDeclareText)); // "X will do Y" helper textbox
                    }

                }
            }

            // Remove all "X will do Y" texts
            foreach (PooledBehaviour pb in declarations) {
                ObjectPoolManager.Instance.Return(pb);
            }
        }

        private bool CharacterCanCast(Character c) {
            return c.Stats.State == State.ALIVE;
        }

        private IEnumerator PerformActions(List<IPlayable> plays) {
            plays.Sort();
            for (int i = 0; i < plays.Count; i++) {
                yield return new WaitForSeconds(0.25f);
                IPlayable play = plays[i];

                // Dead characters cannot unleash spells
                if (CharacterCanCast(play.MySpell.Caster)) { // Death check
                    bool wasPlayable = play.IsPlayable;
                    yield return play.Play();
                    AddText(play.Text);

                    // If play didn't occur (e.g the target became dead before its cast, don't announce death)
                    if (wasPlayable) {
                        Character caster = play.MySpell.Caster;
                        Character target = play.MySpell.Target;

                        // Caster somehow kills themself, don't say they died twice
                        if (caster.Equals(target) && caster.Stats.State == State.DEAD) {
                            AddText(string.Format(CHARACTER_DEATH, play.MySpell.Caster.Look.DisplayName));
                        } else {
                            if (caster.Stats.State == State.DEAD) {
                                AddText(string.Format(CHARACTER_DEATH, play.MySpell.Caster.Look.DisplayName));
                            }
                            if (target.Stats.State == State.DEAD) {
                                AddText(string.Format(CHARACTER_DEATH, play.MySpell.Target.Look.DisplayName));
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator GoThroughBuffs(IList<Character> chars) {
            //End of round buff interactions
            foreach (Character c in chars) {
                yield return new WaitForSeconds(0.25f);
                Characters.Buffs buffs = c.Buffs;
                IList<Buff> timedOut = new List<Buff>();
                foreach (Buff myB in buffs) {
                    yield return new WaitForSeconds(0.1f);
                    Buff b = myB;
                    AddText(string.Format(BUFF_AFFECT, c.Look.DisplayName, b.Name));
                    b.OnEndOfTurn(c.Stats);
                    if (b.IsTimedOut) {
                        timedOut.Add(b);
                    }
                }
                foreach (Buff myB in timedOut) {
                    yield return new WaitForSeconds(0.1f);
                    Buff b = myB;
                    AddText(string.Format(BUFF_FADE, c.Look.DisplayName, b.Name));
                    buffs.RemoveBuff(RemovalType.TIMED_OUT, b);
                }
            }
        }

        private void EndRound() {
            turnCount++;
        }
    }
}