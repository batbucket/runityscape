using Scripts.Model.Buffs;
using Scripts.Model.Spells;
using Scripts.Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using System.Collections;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.SaveLoad;

namespace Scripts.Model.Characters {
    public enum RemovalType {
        TIMED_OUT,
        DISPEL,
    }

    /// <summary>
    /// Represents Buffs on a certain character.
    /// </summary>
    public class Buffs : IEnumerable<Buff>, ISaveable<CharacterBuffsSave> {

        public Stats Stats;
        public Action<SplatDetails> AddSplat;

        private HashSet<Buff> set;

        // Temporary fields used in serializable, will be null otherwise
        private List<Character> partyMembers;
        private bool isSetupTempFieldsBefore;

        public Buffs() {
            set = new HashSet<Buff>(new IdNumberEqualityComparer<Buff>());
            this.AddSplat = (a => { });
        }

        public void AddBuff(Buff buff, Character caster) {
            buff.Caster = new BuffParams(caster.Stats, caster.Id);
            AddBuff(buff);
        }

        public void AddBuff(Buff buff) {
            Util.Assert(buff.BuffCaster != null, "Buff's caster is null.");
            buff.OnApply(Stats);
            set.Add(buff);
            AddSplat(new SplatDetails(Color.green, string.Format("+"), buff.Sprite));
        }

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
            if (buff.IsDispellable) {
                set.Remove(buff);
                AddSplat(new SplatDetails(Color.red, string.Format("-"), buff.Sprite));
            }
        }

        public bool HasBuff<T>() where T : Buff {
            return set.Any(b => b is T);
        }

        IEnumerator<Buff> IEnumerable<Buff>.GetEnumerator() {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return set.GetEnumerator();
        }

        public CharacterBuffsSave GetSaveObject() {
            return new CharacterBuffsSave(set.Select(b => b.GetSaveObject()).ToList());
        }

        public override bool Equals(object obj) {
            var item = obj as Buffs;

            if (item == null) {
                return false;
            }

            return new HashSet<Buff>(set).SetEquals(new HashSet<Buff>(item.set));
        }

        public override int GetHashCode() {
            return 0;
        }

        public void InitFromSaveObject(CharacterBuffsSave saveObject) {
            foreach (BuffSave bs in saveObject.BuffSaves) {
                set.Add(CharacterBuffsSave.SetupBuffCasterFromSave(bs, partyMembers));
            }
            partyMembers = null;
        }

        public void SetupTemporarySaveFields(List<Character> partyMembers) {
            Util.Assert(!isSetupTempFieldsBefore, "This function is only callable once when the object is being loaded.");
            this.partyMembers = partyMembers;
            isSetupTempFieldsBefore = true;
        }
    }
}
