using Scripts.Game.Serialized;
using System;

namespace Scripts.Game.Shopkeeper {
    /// <summary>
    /// Various talks one can have with a shopkeeper
    /// </summary>
    public class Talk {
        /// <summary>
        /// The name
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The big
        /// </summary>
        public readonly string Big;
        /// <summary>
        /// The condition
        /// </summary>
        public Func<Flags, bool> Condition;
        /// <summary>
        /// The set flags
        /// </summary>
        public Action<Flags> SetFlags;
        /// <summary>
        /// The end action
        /// </summary>
        public Action EndAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Talk"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="big">The big.</param>
        public Talk(string name, string big) {
            this.Name = name;
            this.Big = big;
            this.Condition = f => true;
            this.SetFlags = f => { };
            this.EndAction = () => { };
        }

        /// <summary>
        /// Adds the end action.
        /// </summary>
        /// <param name="endAction">The end action.</param>
        /// <returns></returns>
        public Talk AddEndAction(Action endAction) {
            this.EndAction = endAction;
            return this;
        }

        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public Talk AddCondition(Func<Flags, bool> condition) {
            this.Condition = condition;
            return this;
        }

        /// <summary>
        /// Adds the set flags.
        /// </summary>
        /// <param name="setFlags">The set flags.</param>
        /// <returns></returns>
        public Talk AddSetFlags(Action<Flags> setFlags) {
            this.SetFlags = setFlags;
            return this;
        }
    }
}