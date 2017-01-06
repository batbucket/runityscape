using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Processes;
using Scripts.Presenter;
using Scripts.View.ActionGrid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Pages {

    public abstract class Page : IButtonable {
        public static int idCount = 0;
        protected Stack<IButtonable[]> gridStack;
        private string _inputtedString;
        private IButtonable[] actionGrid;

        public Page(
            string text = "",
            string tooltip = "",
            string location = "",
            bool hasInputField = false,
            IList<Character> left = null,
            IList<Character> right = null,
            Action onFirstEnter = null,
            Action onEnter = null,
            Action onFirstExit = null,
            Action onExit = null,
            Action onTick = null,
            IList<IButtonable> buttonables = null,
            string musicLoc = null
            ) {
            this.Text = text;
            this.Tooltip = tooltip;
            this.HasInputField = hasInputField;

            this.Location = location;

            //Left Characters initialization
            if (left == null || left.Count == 0) {
                this.LeftCharacters = new List<Character>();
            } else {
                SetSide(left, false);
            }

            //Right Characters initialization
            if (right == null || right.Count == 0) {
                this.RightCharacters = new List<Character>();
            } else {
                SetSide(right, true);
            }

            this.RepeatedCharacterCheck(GetAll());
            this.OnFirstEnterAction = onFirstEnter ?? (() => { });
            this.OnEnterAction = onEnter ?? (() => { });
            this.OnFirstExitAction = onFirstExit ?? (() => { });
            this.OnExitAction = onExit ?? (() => { });

            this.ActionGrid = buttonables == null ? new IButtonable[0] : buttonables.ToArray();

            this.OnTickAction = onTick ?? (() => { });
            this.Music = musicLoc;

            this.Id = idCount++;

            this._inputtedString = "";
        }

        public IButtonable[] ActionGrid {
            get {
                return actionGrid;
            }
            set {
                this.actionGrid = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
                for (int i = 0; i < actionGrid.Length; i++) {
                    if (value != null && i < value.Length) {
                        actionGrid[i] = value[i];
                    } else {
                        actionGrid[i] = new Process();
                    }
                }
            }
        }

        public bool HasEnteredBefore { get; protected set; }
        public bool HasExitedBefore { get; protected set; }
        public bool HasInputField { get; set; }
        public int Id { get; private set; }
        public string InputtedString { get { return _inputtedString; } set { _inputtedString = value; } }
        public IList<Character> LeftCharacters { get; protected set; }
        public string Location { get; set; }
        public string Music { get; protected set; }
        public Action OnEnterAction { get; protected set; }
        public Action OnExitAction { get; protected set; }
        public Action OnFirstEnterAction { get; protected set; }
        public Action OnFirstExitAction { get; protected set; }
        public Action OnTickAction { get; protected set; }
        public IList<Character> RightCharacters { get; protected set; }
        public string Text { get; protected set; }
        public string Tooltip { get; set; }

        public string ButtonText {
            get {
                return Location;
            }
        }

        public bool IsInvokable {
            get {
                return true;
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

        public void AddCharacters(Character sameSide, params Character[] chars) {
            AddCharacters(sameSide.Side, chars);
        }

        public void AddCharacters(bool isRightSide, params Character[] chars) {
            SetCharacterSides(chars, isRightSide);
            IList<Character> myList = !isRightSide ? LeftCharacters : RightCharacters;
            foreach (Character c in chars) {
                myList.Add(c);
                OnAddCharacter(c);
            }
        }

        public void ClearActionGrid() {
            for (int i = 0; i < ActionGrid.Length; i++) {
                ActionGrid[i] = new Process();
            }
        }

        public void Enter() {
            if (!HasEnteredBefore) {
                HasEnteredBefore = true;
                OnFirstEnter();
                OnFirstEnterAction.Invoke();
            }
            OnAnyEnter();
            OnEnterAction.Invoke();
        }

        public override bool Equals(object obj) {
            // If parameter is null return false.
            if (obj == null) {
                return false;
            }

            // If parameter cannot be cast to Page return false.
            Page p = obj as Page;
            if ((object)p == null) {
                return false;
            }

            // Return true if the fields match:
            return p.Id == this.Id;
        }

        public void Exit() {
            if (!HasExitedBefore) {
                HasExitedBefore = true;
                OnFirstExit();
                OnFirstExitAction.Invoke();
            }
            OnAnyExit();
            OnExitAction.Invoke();
        }

        public List<Character> GetAll(Character c = null) {
            List<Character> allChars = new List<Character>();
            allChars.AddRange(GetCharacters(false));
            allChars.AddRange(GetCharacters(true));
            return c == null ? allChars : allChars.Where(chara => chara != c).ToList();
        }

        public IList<Character> GetAllies(Character c) {
            return GetCharacters(c.Side);
        }

        public IList<Character> GetCharacters(bool isRightSide) {
            return !isRightSide ? LeftCharacters : RightCharacters;
        }

        public IList<Character> GetEnemies(Character c) {
            return GetCharacters(!c.Side);
        }

        public override int GetHashCode() {
            return Id;
        }

        public Character GetRandomAlly(Character c) {
            return GetAllies(c).PickRandom();
        }

        public Character GetRandomCharacter(Character c = null) {
            return GetAll(c).PickRandom();
        }

        public Character GetRandomEnemy(Character c) {
            return GetEnemies(c).PickRandom();
        }

        public void Tick() {
            OnTickAction.Invoke();
            OnTick();
            IList<Character> all = GetAll();
            RepeatedCharacterCheck(all);
            foreach (Character c in all) {
                c.Tick();
            }
        }

        public void Invoke() {
            Game.Instance.CurrentPage = this;
        }

        protected virtual void OnAddCharacter(Character c) {
        }

        protected virtual void OnAnyEnter() {
        }

        protected virtual void OnAnyExit() {
        }

        protected virtual void OnFirstEnter() {
        }

        protected virtual void OnFirstExit() {
        }

        protected virtual void OnTick() {
        }

        /// <summary>
        /// Check if repeated characters exist on a side, if so, append A->Z for each repeated character
        /// For example: Steve A, Steve B
        /// </summary>
        /// <param name="characters">To check</param>
        private void RepeatedCharacterCheck(IList<Character> characters) {
            Dictionary<string, IList<Character>> repeatedCharacters = new Dictionary<string, IList<Character>>();
            foreach (Character c in characters) {
                if (!repeatedCharacters.ContainsKey(c.Name)) {
                    List<Character> characterList = new List<Character>();
                    characterList.Add(c);
                    repeatedCharacters.Add(c.Name, characterList);
                } else {
                    repeatedCharacters[c.Name].Add(c);
                }
            }

            foreach (KeyValuePair<string, IList<Character>> cPair in repeatedCharacters) {
                if (cPair.Value.Count > 1) {
                    int index = 0;
                    foreach (Character c in cPair.Value) {
                        if (characters.Contains(c)) {
                            c.Suffix = Util.IntToLetter(index++);
                        }
                    }
                } else {
                    cPair.Value[0].Suffix = "";
                }
            }
        }

        private void SetCharacterSides(IList<Character> characters, bool isRightSide) {
            foreach (Character c in characters) {
                c.Side = isRightSide;
            }
        }

        private void SetSide(IList<Character> characters, bool isRightSide) {
            SetCharacterSides(characters, isRightSide);
            List<Character> myList = new List<Character>();
            myList.AddRange(characters);
            if (!isRightSide) {
                LeftCharacters = myList;
            } else {
                RightCharacters = myList;
            }
        }
    }
}