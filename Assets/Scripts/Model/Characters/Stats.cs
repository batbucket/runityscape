using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Stats;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Characters {
    public class Stats : IEnumerable<KeyValuePair<StatType, Stat>>, ISaveable<CharacterStatsSave> {

        public Func<StatType, int> GetEquipmentBonus;
        public Action<SplatDetails> AddSplat;

        private readonly IDictionary<StatType, Stat> dict;

        public Stats() {
            this.dict = new Dictionary<StatType, Stat>();
            this.AddSplat = (a => { });
            SetDefaultStats();
            SetDefaultResources();
        }

        public StatType[] Resources {
            get {
                return dict.Keys.Where(k => StatType.RESOURCES.Contains(k)).ToArray();
            }
        }

        public string AttributeDistribution {
            get {
                List<string> s = new List<string>();
                foreach (KeyValuePair<StatType, Stat> pair in dict) {
                    s.Add(string.Format("{0} {1}/{2}", pair.Key.Name, pair.Value.Mod, pair.Value.Max));
                }
                return string.Format("{0}.", string.Join(", ", s.ToArray()));
            }
        }

        public State State {
            get {
                if (GetStatCount(Value.MOD, StatType.HEALTH) <= 0) {
                    return State.DEAD;
                } else {
                    return State.ALIVE;
                }
            }
        }

        public void AddStat(Stat stat) {
            this.dict.Add(stat.Type, stat);
            AddSplat(new SplatDetails(stat.Type.Color, "+", stat.Type.Sprite));
        }

        public void SetToStat(StatType statType, Value value, int amount) {
            if (HasStat(statType) && amount != 0) {
                Stat stat = dict[statType];
                if (value == Value.MOD) {
                    stat.Mod = amount;
                } else {
                    stat.Max = (int)amount;
                }
            }
            AddSplat(new SplatDetails(statType.DetermineColor(amount), string.Format("={0}", amount), statType.Sprite));
        }

        public void SetToStat(StatType statType, int amount) {
            SetToStat(statType, Value.MAX, amount);
            SetToStat(statType, Value.MOD, amount);
        }

        public void AddToStat(StatType statType, Value value, int amount) {
            if (HasStat(statType) && amount != 0) {
                Stat stat = dict[statType];
                if (value == Value.MOD) {
                    stat.Mod += amount;
                } else {
                    stat.Max += (int)amount;
                }
            }
            AddSplat(new SplatDetails(statType.DetermineColor(amount), StatUtil.ShowSigns((int)amount), statType.Sprite));
        }

        public void AddToStat(StatType statType, int amount) {
            AddToStat(statType, Value.MAX, amount);
            AddToStat(statType, Value.MOD, amount);
        }

        public bool HasStat(StatType statType) {
            Stat stat;
            dict.TryGetValue(statType, out stat);
            return stat != null;
        }

        public int GetStatCount(params StatType[] statTypes) {
            return GetStatCount(Value.MOD_AND_EQUIP, statTypes);
        }

        public int GetStatCount(Value value, params StatType[] statTypes) {
            int sum = 0;
            foreach (StatType st in statTypes) {
                if (HasStat(st)) {
                    Stat stat;
                    dict.TryGetValue(st, out stat);
                    if (value == Value.MOD) {
                        sum += (int)stat.Mod;
                    } else if (value == Value.MOD_AND_EQUIP) {
                        sum += ((int)stat.Mod + GetEquipmentBonus(st));
                    } else {
                        sum += stat.Max;
                    }
                }
            }
            return sum;
        }

        public void Update(Character c) {
            ICollection<Stat> stats = dict.Values;
            foreach (Stat stat in stats) {
                stat.Update(c);
            }
        }

        protected void InitializeStats(int level, int str, int agi, int intel, int vit) {
            SetToStat(StatType.LEVEL, Value.MOD, level);
            SetToStat(StatType.STRENGTH, str);
            SetToStat(StatType.AGILITY, agi);
            SetToStat(StatType.INTELLECT, intel);
            SetToStat(StatType.VITALITY, vit);
        }

        public void InitializeResources() {
            ICollection<Stat> stats = dict.Values;
            foreach (Stat stat in stats) {
                if (StatType.RESTORED.Contains(stat.Type)) {
                    stat.Mod = stat.Max;
                }
            }
        }

        private void SetDefaultStats() {
            this.AddStat(new Level(0));
            this.AddStat(new Strength(0, 0));
            this.AddStat(new Agility(0, 0));
            this.AddStat(new Intellect(0, 0));
            this.AddStat(new Vitality(0, 0));
        }

        private void SetDefaultResources() {
            this.AddStat(new Health(0, 0));
        }

        IEnumerator<KeyValuePair<StatType, Stat>> IEnumerable<KeyValuePair<StatType, Stat>>.GetEnumerator() {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return dict.GetEnumerator();
        }

        public CharacterStatsSave GetSaveObject() {
            List<StatSave> list = new List<StatSave>();
            foreach (KeyValuePair<StatType, Stat> pair in dict) {
                Stat stat = pair.Value;
                list.Add(stat.GetSaveObject());
            }
            return new CharacterStatsSave(list);
        }

        public void InitFromSaveObject(CharacterStatsSave saveObject) {
            foreach (StatSave save in saveObject.Stats) {
                Stat stat = save.ObjectFromID();
                stat.InitFromSaveObject(save);
                dict.Add(stat.Type, stat);
            }
        }
    }
}
