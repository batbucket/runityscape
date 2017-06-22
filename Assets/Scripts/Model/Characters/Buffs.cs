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
    public class Buffs : IEnumerable<Buff>, ISaveable<PartialCharacterBuffsSave, FullCharacterBuffsSave> {

        public Stats Stats;
        public Action<SplatDetails> AddSplat;

        private HashSet<Buff> set;

        // Temporary fields used in serializable, will be null otherwise
        private List<Character> partyMembers;
        private bool isSetupTempFieldsBefore;

        public Buffs() {
            set = new HashSet<Buff>();
            this.AddSplat = (a => { });
        }

        public void AddBuff(Buff buff) {
            buff.OnApply(Stats);
            set.Add(buff);
            AddSplat(new SplatDetails(Color.white, string.Format("+{0}", buff.Name), buff.Sprite));
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
            set.Remove(buff);
            AddSplat(new SplatDetails(Color.grey, string.Format("-{0}", buff.Name), buff.Sprite));
        }

        IEnumerator<Buff> IEnumerable<Buff>.GetEnumerator() {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return set.GetEnumerator();
        }

        public PartialCharacterBuffsSave GetSaveObject() {
            return new PartialCharacterBuffsSave(set.Select(b => b.GetSaveObject()).ToList());
        }

        public void InitFromSaveObject(FullCharacterBuffsSave saveObject) {
            foreach (FullBuffSave bs in saveObject.BuffSaves) {
                Buff b = bs.CreateObjectFromID();
                b.InitFromSaveObject(bs);

                Stats caster = null;
                int id = 0;
                if (bs.IsCasterInParty) {
                    Character character = partyMembers[bs.PartyIndex];
                    caster = character.Stats;
                    id = character.Id;
                } else {
                    caster = new Stats();
                    caster.InitFromSaveObject(bs.Stats);
                    id = Character.UNKNOWN_ID;
                }
                b.Caster = new BuffParams() { Caster = caster, CasterId = id };

                set.Add(b);
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
