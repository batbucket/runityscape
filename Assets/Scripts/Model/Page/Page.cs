using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Page {

    public string Text { get; protected set; }
    public string Tooltip { get; set; }
    public string Location { get; set; }
    public bool HasInputField { get; set; }
    public Character MainCharacter { get; protected set; }
    public IList<Character> LeftCharacters { get; private set; }
    public IList<Character> RightCharacters { get; private set; }
    public Process[] ActionGrid { get; set; }

    public Action OnFirstEnterAction { get; protected set; }
    public Action OnEnterAction { get; protected set; }
    public Action OnFirstExitAction { get; protected set; }
    public Action OnExitAction { get; protected set; }
    public Action OnTick { get; protected set; }

    public bool HasEnteredBefore { get; protected set; }
    public bool HasExitedBefore { get; protected set; }

    public static int idCount = 0;
    public int Id { get; private set; }

    string _inputtedString;
    public string InputtedString { get { return _inputtedString; } set { _inputtedString = value; } }

    public Page(
        string text = "",
        string tooltip = "",
        string location = "",
        bool hasInputField = false,
        Character mainCharacter = null,
        Character[] left = null,
        Character[] right = null,
        Action onFirstEnter = null,
        Action onEnter = null,
        Action onFirstExit = null,
        Action onExit = null,
        Action onTick = null,
        Process[] processes = null
        ) {

        this.Text = text;
        this.Tooltip = tooltip;
        this.MainCharacter = mainCharacter;
        this.HasInputField = hasInputField;

        this.Location = location;


        //Left Characters initialization
        if (left == null || left.Length == 0) {
            this.LeftCharacters = new List<Character>();
        } else {
            SetSide(left, false);
        }

        //Right Characters initialization
        if (right == null || right.Length == 0) {
            this.RightCharacters = new List<Character>();
        } else {
            SetSide(right, true);
        }

        this.RepeatedCharacterCheck(GetAll());
        this.OnFirstEnterAction = onFirstEnter ?? (() => { });
        this.OnEnterAction = onEnter ?? (() => { });
        this.OnFirstExitAction = onFirstExit ?? (() => { });
        this.OnExitAction = onExit ?? (() => { });

        this.ActionGrid = processes ?? new Process[ActionGridView.ROWS * ActionGridView.COLS];

        this.OnTick = onTick ?? (() => { });
        this.Id = idCount++;

        this._inputtedString = "";
    }


    public void OnEnter() {
        if (!HasEnteredBefore) {
            HasEnteredBefore = true;
            OnFirstEnter();
        }
        OnAnyEnter();
    }

    public virtual void OnFirstEnter() {
        OnFirstEnterAction.Invoke();
    }

    public virtual void OnAnyEnter() {
        OnEnterAction.Invoke();
        GetAll().Where(c => c.HasResource(ResourceType.CHARGE)).ToList().ForEach(c => c.Resources[ResourceType.CHARGE].IsVisible = false);
    }

    public void OnExit() {
        if (!HasExitedBefore) {
            HasExitedBefore = true;
            OnFirstExit();
        }
        OnExitAction.Invoke();
    }

    public virtual void OnFirstExit() {
        OnFirstExitAction.Invoke();
    }

    public virtual void OnAnyExit() {
        OnExitAction.Invoke();
    }

    void SetSide(IList<Character> characters, bool isRightSide) {
        SetCharacterSides(characters, isRightSide);
        List<Character> myList = new List<Character>();
        myList.AddRange(characters);
        if (!isRightSide) {
            LeftCharacters = myList;
        } else {
            RightCharacters = myList;
        }
    }

    void SetCharacterSides(IList<Character> characters, bool isRightSide) {
        foreach (Character c in characters) {
            c.Side = isRightSide;
        }
    }

    /**
     * Check if repeated characters exist on a side, if so, append A->Z for each repeated character
     * For example: Steve A, Steve B
     */
    void RepeatedCharacterCheck(IList<Character> characters) {
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
                        c.Name = string.Format("{0} {1}", c.Name, Util.IntToLetter(index++));
                    }
                }
            }
        }
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

    public override int GetHashCode() {
        return Id;
    }

    public IList<Character> GetCharacters(bool isRightSide) {
        return !isRightSide ? LeftCharacters : RightCharacters;
    }

    public IList<Character> GetAllies(bool side) {
        return GetCharacters(side);
    }

    public IList<Character> GetEnemies(bool side) {
        return GetCharacters(!side);
    }

    public Character GetRandomAlly(bool side) {
        return GetRandomCharacter(GetCharacters(side));
    }

    public Character GetRandomEnemy(bool side) {
        return GetRandomCharacter(GetCharacters(!side));
    }

    public Character GetRandomCharacter() {
        return GetRandomCharacter(GetAll());
    }

    public List<Character> GetAll() {
        List<Character> allChars = new List<Character>();
        allChars.AddRange(GetCharacters(false));
        allChars.AddRange(GetCharacters(true));
        return allChars;
    }

    static Character GetRandomCharacter(IList<Character> characters) {
        return characters.Any() ? characters[UnityEngine.Random.Range(0, characters.Count)] : null;
    }

    protected void ClearActionGrid() {
        ActionGrid = new Process[ActionGridView.ROWS * ActionGridView.COLS];
    }

    public virtual void Tick() {
        OnTick.Invoke();
        foreach (Character c in GetAll()) {
            c.Tick(false);
        }
    }
}
