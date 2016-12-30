using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PortraitHolderView : PooledBehaviour {
    [SerializeField]
    private PortraitView portraitPrefab;

    public IDictionary<Character, PortraitBundle> CharacterViews { get; protected set; }

    public struct PortraitBundle {
        public PortraitView portraitView;
        public bool isSet;
    }

    private void Start() {
        this.CharacterViews = new Dictionary<Character, PortraitBundle>();
        ObjectPoolManager.Instance.Register(portraitPrefab, 10);
    }

    // TODO make input a struct of only what we need
    public void AddPortraits(IList<Character> characters) {

        //Set all existing isSets to false.
        List<Character> keys = new List<Character>(CharacterViews.Keys); //Can't modify Dictionary in foreach loop
        foreach (Character key in keys) {
            CharacterViews[key] = new PortraitBundle { portraitView = key.Presenter.PortraitView, isSet = false };
        }

        //Add or possibly replace new PortraitViews.
        foreach (Character c in characters) {
            PortraitView pv;
            if (!CharacterViews.ContainsKey(c)) {
                pv = ObjectPoolManager.Instance.Get(portraitPrefab);
                pv.ClearEffects();
                Util.Parent(pv.gameObject, gameObject);
            } else {
                pv = CharacterViews[c].portraitView;
            }
            pv.PortraitName = c.DisplayName;
            CharacterViews[c] = new PortraitBundle { portraitView = pv, isSet = true };
        }

        //Check if any isSets are false, if so, remove them and Destroy their gameObjects.
        //We can use same keys list as before since newly added keys cannot be false
        foreach (Character key in keys) {
            if (CharacterViews.ContainsKey(key) && !CharacterViews[key].isSet) {
                ObjectPoolManager.Instance.Return(CharacterViews[key].portraitView);
                CharacterViews.Remove(key);
            }
        }
    }

    public override void Reset() {

    }
}
