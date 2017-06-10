using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Characters {
    public class Stats {
        private const string SHOW_SIGNS = "+#;-#;0";

        public readonly IDictionary<StatType, Stat> Dict;

        public Action<SplatDetails> AddSplat;

        public Stats() {
            this.Dict = new Dictionary<StatType, Stat>();
            this.AddSplat = (a => { });
            SetDefaultStats();
            SetDefaultResources();
        }

        public string AttributeDistribution {
            get {
                List<string> s = new List<string>();
                foreach (KeyValuePair<StatType, Stat> pair in Dict) {
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
            this.Dict.Add(stat.Type, stat);
            AddSplat(new SplatDetails(stat.Type.Color, "+", stat.Type.Sprite));
        }

        public void SetToStat(StatType statType, Value value, float amount) {
            if (HasStat(statType) && amount != 0) {
                Stat stat = Dict[statType];
                if (value == Value.MOD) {
                    stat.Mod = amount;
                } else {
                    stat.Max = (int)amount;
                }
            }
            AddSplat(new SplatDetails(statType.DetermineColor(amount), string.Format("={0}", amount), statType.Sprite));
        }

        public void SetToStat(StatType statType, float amount) {
            SetToStat(statType, Value.MAX, amount);
            SetToStat(statType, Value.MOD, amount);
        }

        public void AddToStat(StatType statType, Value value, float amount) {
            if (HasStat(statType) && amount != 0) {
                Stat stat = Dict[statType];
                if (value == Value.MOD) {
                    stat.Mod += amount;
                } else {
                    stat.Max += (int)amount;
                }
            }
            AddSplat(new SplatDetails(statType.DetermineColor(amount), amount.ToString(SHOW_SIGNS), statType.Sprite));
        }

        public void AddToStat(StatType statType, float amount) {
            AddToStat(statType, Value.MAX, amount);
            AddToStat(statType, Value.MOD, amount);
        }

        public bool HasStat(StatType statType) {
            Stat stat;
            Dict.TryGetValue(statType, out stat);
            return stat != null;
        }

        public int GetStatCount(params StatType[] statTypes) {
            return GetStatCount(Value.MOD, statTypes);
        }

        public int GetStatCount(Value value, params StatType[] statTypes) {
            int sum = 0;
            foreach (StatType st in statTypes) {
                if (HasStat(st)) {
                    Stat stat;
                    Dict.TryGetValue(st, out stat);
                    if (value == Value.MOD) {
                        sum += (int)stat.Mod;
                    } else {
                        sum += stat.Max;
                    }
                }
            }
            return sum;
        }

        public void Update(Character c) {
            ICollection<Stat> stats = Dict.Values;
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
            ICollection<Stat> stats = Dict.Values;
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

    }
}
