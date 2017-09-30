using Scripts.Model.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.SaveLoad;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Describes the type of removal for a buff
    /// </summary>
    public enum RemovalType {
        TIMED_OUT, // Buff ran out of time
        DISPEL, // Buff is being prematurely dispelled
    }

    /// <summary>
    /// Represents Buffs on a certain character.
    /// </summary>
    public class Buffs : IEnumerable<Buff>, ISaveable<CharacterBuffsSave> {

        /// <summary>
        /// Set in character.
        /// </summary>
        public Stats Stats;

        /// <summary>
        /// Adds a splat detailing something about the buff.
        /// </summary>
        public Action<SplatDetails> AddSplat;

        /// <summary>
        /// Set of buffs. Used to avoid duplicates.
        /// </summary>
        private HashSet<Buff> set;

        // Temporary fields used in serializable, will be null otherwise
        private List<Character> partyMembers;

        private bool isSetupTempFieldsBefore;

        public Buffs() {
            set = new HashSet<Buff>(new IdNumberEqualityComparer<Buff>());
            this.AddSplat = (a => { });
        }

        /// <summary>
        /// Adds a buff to this character.
        /// </summary>
        /// <param name="buff">Buff to add.</param>
        /// <param name="caster">Buff's caster.</param>
        public void AddBuff(Buff buff, Character caster) {
            buff.Caster = new BuffParams(caster.Stats, caster.Id);
            AddBuff(buff);
        }

        /// <summary>
        /// No caster variant. Buff already has caster set.
        /// </summary>
        /// <param name="buff">Buff to add.</param>
        public void AddBuff(Buff buff) {
            Util.Assert(buff.BuffCaster != null, "Buff's caster is null.");
            buff.OnApply(Stats);
            set.Add(buff);
            AddSplat(new SplatDetails(Color.green, string.Format("+"), buff.Sprite));
        }

        /// <summary>
        /// Removes a buff by various means.
        /// </summary>
        /// <param name="type">Type of removal.</param>
        /// <param name="buff">Buff to remove.</param>
        public void RemoveBuff(RemovalType type, Buff buff) {
            switch (type) {
                case RemovalType.TIMED_OUT:
                    buff.OnTimeOut(Stats);
                    break;

                case RemovalType.DISPEL:
                    buff.OnDispell(Stats);
                    break;

                default:
                    Util.Assert(false, "Unknown removal type: " + type);
                    break;
            }

            // Remove if possible
            if (buff.IsDispellable || type == RemovalType.TIMED_OUT) {
                set.Remove(buff);
                AddSplat(new SplatDetails(Color.red, string.Format("-"), buff.Sprite));
            }
        }

        /// <summary>
        /// Dispels all buffs.
        /// </summary>
        public void DispelAllBuffs() {
            Buff[] allBuffs = set.ToArray();
            foreach (Buff buff in allBuffs) {
                RemoveBuff(RemovalType.DISPEL, buff);
            }
        }

        /// <summary>
        /// Does the character have a buff of this type?
        /// </summary>
        /// <typeparam name="T">Type of buff to find</typeparam>
        /// <returns>True if character has this kind of buff.</returns>
        public bool HasBuff<T>() where T : Buff {
            return set.Any(b => b is T);
        }

        IEnumerator<Buff> IEnumerable<Buff>.GetEnumerator() {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return set.GetEnumerator();
        }

        /// <summary>
        /// Serialize all the buffs being held.
        /// </summary>
        /// <returns>Serializable object with all the buffs being held.</returns>
        public CharacterBuffsSave GetSaveObject() {
            return new CharacterBuffsSave(set.Select(b => b.GetSaveObject()).ToList());
        }

        /// <summary>
        /// Check set equality
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if sets have equal buffs.</returns>
        public override bool Equals(object obj) {
            var item = obj as Buffs;

            if (item == null) {
                return false;
            }

            return new HashSet<Buff>(set).SetEquals(new HashSet<Buff>(item.set));
        }

        /// <summary>
        /// No hash possible
        /// </summary>
        /// <returns>0</returns>
        public override int GetHashCode() {
            return 0;
        }

        /// <summary>
        /// Initialize by loading buffs and readding them.
        /// </summary>
        /// <param name="saveObject">Serializable buffs object.</param>
        public void InitFromSaveObject(CharacterBuffsSave saveObject) {
            foreach (BuffSave bs in saveObject.BuffSaves) {
                set.Add(CharacterBuffsSave.SetupBuffCasterFromSave(bs, partyMembers));
            }
            partyMembers = null;
        }

        /// <summary>
        /// Setup party members for re-referencing after loading
        /// </summary>
        /// <param name="partyMembers">Party members in this particular game.</param>
        public void SetupTemporarySaveFields(List<Character> partyMembers) {
            Util.Assert(!isSetupTempFieldsBefore, "This function is only callable once when the object is being loaded.");
            this.partyMembers = partyMembers;
            isSetupTempFieldsBefore = true;
        }
    }
}