using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Page : IButtonable {

    public string Text { get; protected set; }
    public string Tooltip { get; set; }
    public string Location { get; set; }
    public bool HasInputField { get; set; }
    public Character MainCharacter { get; protected set; }
    public IList<Character> LeftCharacters { get; private set; }
    public IList<Character> RightCharacters { get; private set; }

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
    private IButtonable[] actionGrid;

    public Action OnFirstEnterAction { get; protected set; }
    public Action OnEnterAction { get; protected set; }
    public Action OnFirstExitAction { get; protected set; }
    public Action OnExitAction { get; protected set; }
    public Action OnTick { get; protected set; }

    public bool HasEnteredBefore { get; protected set; }
    public bool HasExitedBefore { get; protected set; }
    public string Music { get; protected set; }

    public static int idCount = 0;
    public int Id { get; private set; }

    private string _inputtedString;
    public string InputtedString { get { return _inputtedString; } set { _inputtedString = value; } }

    string IButtonable.ButtonText {
        get {
            return Location;
        }
    }

    string IButtonable.TooltipText {
        get {
            return string.Format("Go to {0}.", Location);
        }
    }

    bool IButtonable.IsInvokable {
        get {
            return true;
        }
    }

    bool IButtonable.IsVisibleOnDisable {
        get {
            return true;
        }
    }

    public Page(
        string text = "",
        string tooltip = "",
        string location = "",
        bool hasInputField = false,
        Character mainCharacter = null,
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
        this.MainCharacter = mainCharacter;
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

        this.OnTick = onTick ?? (() => { });
        this.Music = musicLoc;

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

    protected virtual void OnAddCharacter(Character c) {

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
        RepeatedCharacterCheck(GetAll());
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
            c.IsShowingBarCounts = !isRightSide;
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
                        c.Suffix = Util.IntToLetter(index++);
                    }
                }
            } else {
                cPair.Value[0].Suffix = "";
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

    public IList<Character> GetAllies(Character c) {
        return GetCharacters(c.Side);
    }

    public IList<Character> GetEnemies(Character c) {
        return GetCharacters(!c.Side);
    }

    public Character GetRandomAlly(Character c) {
        return GetAllies(c).PickRandom();
    }

    public Character GetRandomEnemy(Character c) {
        return GetEnemies(c).PickRandom();
    }

    public Character GetRandomCharacter(Character c = null) {
        return GetAll(c).PickRandom();
    }

    public List<Character> GetAll(Character c = null) {
        List<Character> allChars = new List<Character>();
        allChars.AddRange(GetCharacters(false));
        allChars.AddRange(GetCharacters(true));
        return c == null ? allChars : allChars.Where(chara => chara != c).ToList();
    }

    protected void ClearActionGrid() {
        for (int i = 0; i < ActionGrid.Length; i++) {
            ActionGrid[i] = new Process();
        }
    }

    public virtual void Tick() {
        OnTick.Invoke();
        foreach (Character c in GetAll()) {
            c.UpdateStats(MainCharacter);
        }
    }

    void IButtonable.Invoke() {
        // TODO
    }
}
