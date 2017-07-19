using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.View.ObjectPool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Pages {

    public class Page : IButtonable {
        public static Action<Page> ChangePageFunc;
        public static Action<Page> UpdateGridUI;
        public static Func<TextBox, PooledBehaviour> TypeText;

        public HashList<Character> left;
        public readonly string Location;
        public string Music;
        public HashList<Character> right;
        public string Body;
        public Sprite Icon;
        public Action OnEnter;
        public Func<bool> Condition;
        public bool HasInputField;
        public string Input;

        private IButtonable[] grid;

        public Page(string location) {
            this.Location = location;
            this.grid = new IButtonable[Grid.DEFAULT_BUTTON_COUNT];
            this.left = new HashList<Character>(new IdNumberEqualityComparer<Character>());
            this.right = new HashList<Character>(new IdNumberEqualityComparer<Character>());
            this.OnEnter = () => { };
            this.Input = string.Empty;
            this.Condition = () => true;
        }

        public IList<IButtonable> Actions {
            set {
                IButtonable[] set = null;
                if (value.Count < Grid.DEFAULT_BUTTON_COUNT) {
                    set = new IButtonable[Grid.DEFAULT_BUTTON_COUNT];
                    Array.Copy(value.ToArray(), set, value.Count);
                } else {
                    set = value.ToArray();
                }
                this.grid = set;
                UpdateGridUI.Invoke(this);
            }

            get {
                return grid;
            }
        }

        public ICollection<Character> Left {
            get {
                return left;
            }
        }

        public ICollection<Character> Right {
            get {
                return right;
            }
        }

        public string ButtonText {
            get {
                return Util.ColorString(Location, IsInvokable);
            }
        }

        public bool IsInvokable {
            get {
                return Condition();
            }
        }

        public bool IsVisibleOnDisable {
            get {
                return true;
            }
        }

        public string TooltipText {
            get {
                return string.Format("Go to {0}.", Location);
            }
        }

        public Sprite Sprite {
            get {
                return Icon;
            }
        }

        public void AddCharacters(Side side, params Character[] chars) {
            ICollection<Character> mySet = (side == Side.LEFT) ? Left : Right;
            foreach (Character c in chars) {
                mySet.Add(c);
            }
        }

        public void AddCharacters(Side side, IEnumerable<Character> chars) {
            ICollection<Character> mySet = (side == Side.LEFT) ? Left : Right;
            foreach (Character c in chars) {
                mySet.Add(c);
            }
        }

        public void AddText(params string[] texts) {
            foreach (string s in texts) {
                AddText(s);
            }
        }

        public void ClearActionGrid() {
            Actions = new IButtonable[Grid.DEFAULT_BUTTON_COUNT];
        }

        public List<Character> GetAll(Character c = null) {
            List<Character> allChars = new List<Character>();
            allChars.AddRange(GetCharacters(Side.LEFT));
            allChars.AddRange(GetCharacters(Side.RIGHT));
            return c == null ? allChars : allChars.Where(chara => chara != c).ToList();
        }

        public void Shuffle(Side side) {
            if (side == Side.LEFT) {
                left.Shuffle();
            } else {
                right.Shuffle();
            }
        }

        public ICollection<Character> GetAllies(Character c) {
            return GetCharacters(GetSide(c));
        }

        public ICollection<Character> GetFoes(Character c) {
            ICollection<Character> coll = null;
            if (GetSide(c) == Side.LEFT) {
                coll = GetCharacters(Side.RIGHT);
            } else {
                coll = GetCharacters(Side.LEFT);
            }
            return coll;
        }

        public ICollection<Character> GetCharacters(Side side) {
            return side == Side.LEFT ? Left : Right;
        }

        public void Tick() {
            IList<Character> all = GetAll();
            RepeatedCharacterCheck(all);
            UpdateCharacters(Left);
            UpdateCharacters(Right);
        }

        private void UpdateCharacters(IEnumerable<Character> characters) {
            foreach (Character c in characters) {
                c.Update();
            }
        }

        public void Invoke() {
            ChangePageFunc.Invoke(this);
        }

        public Side GetSide(Character c) {
            if (Left.Contains(c)) {
                return Side.LEFT;
            } else if (Right.Contains(c)) {
                return Side.RIGHT;
            } else {
                throw new UnityException(string.Format("Character {0} not found in either side.", c.Look.DisplayName));
            }
        }

        public PooledBehaviour AddText(Character c, string s) {
            return TypeText(new AvatarBox(GetSide(c), c.Look.Sprite, c.Look.TextColor, s));
        }

        public PooledBehaviour AddText(string s) {
            return TypeText(new TextBox(s));
        }

        public override int GetHashCode() {
            return Location.GetHashCode();
        }

        public override bool Equals(object obj) {
            Page item = obj as Page;
            if (item == null) {
                return false;
            }
            return item.Location.Equals(this.Location);
        }

        /// <summary>
        /// Check if repeated characters exist on a side, if so, append A->Z for each repeated character
        /// For example: Steve A, Steve B
        /// </summary>
        /// <param name="characters">To check</param>
        private void RepeatedCharacterCheck(IList<Character> characters) {
            Dictionary<string, IList<Character>> repeatedCharacters = new Dictionary<string, IList<Character>>();
            foreach (Character c in characters) {
                if (!repeatedCharacters.ContainsKey(c.Look.Name)) {
                    List<Character> characterList = new List<Character>();
                    characterList.Add(c);
                    repeatedCharacters.Add(c.Look.Name, characterList);
                } else {
                    repeatedCharacters[c.Look.Name].Add(c);
                }
            }

            foreach (KeyValuePair<string, IList<Character>> cPair in repeatedCharacters) {
                if (cPair.Value.Count > 1) {
                    int index = 0;
                    foreach (Character c in cPair.Value) {
                        if (characters.Contains(c)) {
                            c.Look.Suffix = string.Format("{0}", index++);
                        }
                    }
                } else {
                    cPair.Value[0].Look.Suffix = "";
                }
            }
        }
    }
}