using Scripts.Model.Items;
using Scripts.Model.Pages;
using Scripts.Model.Spells;
using Scripts.Presenter;
using UnityEngine;

namespace Scripts.Model.Characters {

    /// <summary>
    /// ComputerCharacters are Characters that are controlled via AI.
    /// </summary>
    public abstract class ComputerCharacter : Character {
        public const bool CONTROLLABLE_CPU = false;

        /// <summary>
        /// Seconds before the AI can issue a command.
        /// This is to simulate human input and reaction time.
        /// </summary>
        private const int DEFAULT_MAX_DELAY = 3;

        private readonly float maxDelay;
        protected float delay;

        public ComputerCharacter(Displays displays, StartStats attributes, Items items)
            : base(CONTROLLABLE_CPU, displays, attributes, items) {
            this.maxDelay = DEFAULT_MAX_DELAY;
            this.delay = UnityEngine.Random.Range(0, maxDelay);
        }

        public ComputerCharacter(Displays displays, StartStats attributes)
            : this(displays, attributes, new Items(new Item[0], new EquippableItem[0])) { }

        protected override void Act() {
            if (State == CharacterState.NORMAL && IsCharged && (delay -= Time.deltaTime) <= 0) {
                DecideSpell();
                delay = UnityEngine.Random.Range(0, maxDelay);
            }
        }

        protected abstract void DecideSpell();

        protected void QuickCast(SpellFactory spell, Character target = null) {
            Page page = Game.Instance.CurrentPage;
            if (!spell.IsCastable(this)) {
                return;
            }
            if (target != null) {
                spell.TryCast(this, target);
            } else {
                switch (spell.TargetType) {
                    case TargetType.SINGLE_ALLY:
                        Character ally = page.GetRandomAlly(this);
                        spell.TryCast(this, page.GetRandomAlly(this));
                        break;

                    case TargetType.SINGLE_ENEMY:
                        spell.TryCast(this, page.GetRandomEnemy(this));
                        break;

                    case TargetType.SELF:
                        spell.TryCast(this, this);
                        break;

                    case TargetType.ALL_ALLY:
                        spell.TryCast(this, page.GetAllies(this));
                        break;

                    case TargetType.ALL_ENEMY:
                        spell.TryCast(this, page.GetEnemies(this));
                        break;

                    case TargetType.ALL:
                        spell.TryCast(this, page.GetAll());
                        break;
                }
            }
        }
    }
}