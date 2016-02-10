using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Page {

    public string Text { get; protected set; }
    public string Tooltip { get; protected set; }
    public Character MainCharacter { get; protected set; }
    public IList<Character> LeftCharacters { get; private set; }
    public IList<Character> RightCharacters { get; private set; }
    public Process[] ActionGrid { get; protected set; }

    public Action OnFirstEnterAction { get; protected set; }
    public Action OnEnterAction { get; protected set; }
    public Action OnFirstExitAction { get; protected set; }
    public Action OnExitAction { get; protected set; }
    public Action OnTick { get; protected set; }

    public bool HasEnteredBefore { get; protected set; }
    public bool HasExitedBefore { get; protected set; }

    public static int idCount = 0;
    public int Id { get; private set; }

    public Page(string text = "", string tooltip = "", Character mainCharacter = null, List<Character> left = null, List<Character> right = null,
        Action onFirstEnter = null, Action onEnter = null, Action onFirstExit = null, Action onExit = null, Action onTick = null, params Process[] processes) {
        this.Text = text;
        this.Tooltip = tooltip;
        this.MainCharacter = mainCharacter;
        if (left == null) this.LeftCharacters = new List<Character>(); else LeftCharacters = left;
        if (right == null) this.RightCharacters = new List<Character>(); else RightCharacters = right;
        this.OnFirstEnterAction = (OnFirstEnterAction == null) ? () => { } : onFirstEnter;
        this.OnEnterAction = (onEnter == null) ? () => { } : onEnter;
        this.OnFirstExitAction = (onFirstExit == null) ? () => { } : onFirstExit;
        this.OnExitAction = (onExit == null) ? () => { } : onExit;

        //ActionGrid initialization
        this.ActionGrid = new Process[ActionGridView.ROWS * ActionGridView.COLS];
        if (processes != null) {
            int index = 0;
            foreach (Process p in processes) {
                ActionGrid[index++] = p;
            }
        }

        this.OnTick = (onTick == null) ? () => { } : onTick;
        this.Id = idCount++;
    }


    public void OnEnter() {
        if (!HasEnteredBefore && OnFirstEnterAction != null) {
            HasEnteredBefore = true;
            OnFirstEnter();
        } else if (OnEnterAction != null) {
            OnEnterAction.Invoke();
        }
    }

    public void OnFirstEnter() {
        OnFirstEnterAction.Invoke();
    }

    public void OnExit() {
        if (!HasExitedBefore && OnFirstExitAction != null) {
            HasExitedBefore = true;
            OnFirstExit();
        } else if (OnExitAction != null) {
            OnExitAction.Invoke();
        }
    }

    public void OnFirstExit() {
        OnFirstExitAction.Invoke();
    }

    void SetSide(IList<Character> characters, bool isRightSide) {
        RepeatedCharacterCheck(characters);
        SetCharacterSides(characters, isRightSide);
        if (!isRightSide) {
            LeftCharacters = characters;
        } else {
            RightCharacters = characters;
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
                    c.Name = string.Format("{0} {1}", c.Name, Util.IntToLetter(index++));
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
        allChars.AddRange(GetCharacters(true));
        allChars.AddRange(GetCharacters(false));
        return allChars;
    }

    static Character GetRandomCharacter(IList<Character> characters) {
        return characters[UnityEngine.Random.Range(0, characters.Count)];
    }

    public virtual void Tick() {
        OnTick.Invoke();
    }
}
