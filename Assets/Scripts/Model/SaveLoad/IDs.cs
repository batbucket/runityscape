using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Characters;
using Scripts.Game.Defined.Serialized.Items.Consumables;
using Scripts.Game.Defined.Serialized.Items.Equipment;
using Scripts.Game.Defined.Serialized.Items.Misc;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Initable;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.SaveLoad;
using Scripts.Model.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Scripts.Model.SaveLoad {

    public static class IDs {
        public static TypeMap Types = new TypeMap();
        public static EnumMap<StatType> Stats = new EnumMap<StatType>(StatType.AllTypes);
        public static EnumMap<EquipType> Equips = new EnumMap<EquipType>(EquipType.AllTypes);

        private static HashSet<IInitable> initables;

        public static void Init() {
            foreach (IInitable init in initables) {
                init.Init();
            }
        }

        public static void AddInit(IInitable init) {
            if (initables == null) {
                initables = new HashSet<IInitable>();
            }
            initables.Add(init);
        }
    }

    public class TypeMap : IdMap<Type> {
        protected override void InitHelper() {
            Stats();
            Spells();
            Items();
            Buffs();
            Brains();
        }

        protected override bool IsAllIncluded() {
            return IsEntireNamespaceIdentified() && IsDictTypesContainDefaultConstructor();
        }

        private void Stats() {
            Add<Strength>("str");
            Add<Intellect>("int");
            Add<Agility>("agi");
            Add<Vitality>("vit");

            Add<Health>("hp");
            Add<Skill>("sk");
            Add<Experience>("exp");
            Add<Level>("lvl");
        }

        private void Spells() {
            Add<Attack>("attack");
            Add<Wait>("wait");
            Add<InflictPoison>("inflictpoison");
        }

        private void Items() {
            Add<Apple>("apple");
            Add<PoisonArmor>("poisonarmor");
            Add<Money>("money");
        }

        private void Buffs() {
            Add<Poison>("poison");
        }

        private void Brains() {
            Add<Player>("player");
            Add<DebugAI>("debugai");
        }

        private void Add<Type>(string id) {
            Add(typeof(Type), id);
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace) {
            return assembly.GetTypes().Where(t => t.Namespace != null && !Regex.IsMatch(t.ToString(), "\\+|\\<|\\>|_") && t.Namespace.Contains(nameSpace)).ToArray();
        }

        private bool IsEntireNamespaceIdentified() {
            Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "Scripts.Game.Defined.Serialized");
            bool isEverythingIncludedInMap = true;
            foreach (Type t in types) {
                if (!map.Contains(t)) {
                    Util.Log(string.Format("Unable to locate {0} in Map.", t.ToString()));
                    isEverythingIncludedInMap = false;
                }
            }
            return isEverythingIncludedInMap;
        }

        private bool IsDictTypesContainDefaultConstructor() {
            bool isAllDefault = true;
            foreach (KeyValuePair<Type, string> pair in map) {
                Type type = pair.Key;
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor == null) {
                    Util.Log(string.Format("Unable to locate default constructor for {0} in map.", type.ToString()));
                    isAllDefault = false;
                }
            }
            return isAllDefault;
        }
    }

    public class EnumMap<T> : IdMap<T> where T : INameable {
        private ICollection<T> all;

        public EnumMap(ICollection<T> all) {
            this.all = all;
        }

        protected override void InitHelper() {
            foreach (T item in all) {
                Add(item, item.Name);
            }
        }

        protected override bool IsAllIncluded() {
            return IsAllTypesIncluded();
        }

        private bool IsAllTypesIncluded() {
            bool isAllIncluded = true;
            foreach (T item in all) {
                if (!map.Contains(item)) {
                    isAllIncluded = false;
                }
            }
            return isAllIncluded;
        }
    }
}