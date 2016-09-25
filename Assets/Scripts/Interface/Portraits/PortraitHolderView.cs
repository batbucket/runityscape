using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PortraitHolderView : MonoBehaviour {

    public struct PortraitBundle {
        public PortraitView portraitView;
        public bool isSet;
    }

    protected void OnAwake() {
        this.CharacterViews = new Dictionary<Character, PortraitBundle>();
    }

    public IDictionary<Character, PortraitBundle> CharacterViews { get; protected set; }

    // TODO make input a struct of only what we need
    protected void AddPortraits(IList<Character> characters, GameObject portraitPrefab) {

        //Set all existing isSets to false.
        List<Character> keys = new List<Character>(CharacterViews.Keys); //Can't modify Dictionary in foreach loop
        foreach (Character key in keys) {
            CharacterViews[key] = new PortraitBundle { portraitView = key.Presenter.PortraitView, isSet = false };
        }

        //Add or possibly replace new PortraitViews.
        foreach (Character c in characters) {
            PortraitView pv;
            if (!CharacterViews.ContainsKey(c)) {
                GameObject g = (GameObject)GameObject.Instantiate(portraitPrefab);
                Util.Parent(g, gameObject);
                pv = g.GetComponent<PortraitView>();
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
                if (CharacterViews[key].portraitView != null) {
                    GameObject.Destroy(CharacterViews[key].portraitView.gameObject);
                }
                CharacterViews.Remove(key);
            }
        }
    }

    abstract public void AddPortraits(IList<Character> characters);
}
