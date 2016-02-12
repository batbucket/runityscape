using System.Collections.Generic;
using UnityEngine;

public abstract class PortraitHolderView : MonoBehaviour {
    public struct PortraitBundle {
        public PortraitView portraitView;
        public bool isSet;
    }

    protected void OnAwake() {
        this.CharacterViews = new Dictionary<string, PortraitBundle>();
    }

    public IDictionary<string, PortraitBundle> CharacterViews { get; protected set; }

    protected void AddPortraits(string[] portraitNames, string portraitLocation) {

        //Set all existing isSets to false.
        List<string> keys = new List<string>(CharacterViews.Keys); //Can't modify Dictionary in foreach loop
        foreach (string key in keys) {
            CharacterViews[key] = new PortraitBundle { portraitView = CharacterViews[key].portraitView, isSet = false };
        }

        //Add or possibly replace new PortraitViews.
        foreach (string name in portraitNames) {
            PortraitView pv;
            if (!CharacterViews.ContainsKey(name)) {
                GameObject g = (GameObject)GameObject.Instantiate(Resources.Load(portraitLocation));
                Util.Parent(g, gameObject);
                pv = g.GetComponent<PortraitView>();
            } else {
                pv = CharacterViews[name].portraitView;
            }
            pv.PortraitName = name;
            CharacterViews[name] = new PortraitBundle { portraitView = pv, isSet = true };
        }

        //Check if any isSets are false, if so, remove them and Destroy their gameObjects.
        //We can use same keys list as before since newly added keys cannot be false
        foreach (string key in keys) {
            if (!CharacterViews[key].isSet) {
                GameObject.Destroy(CharacterViews[key].portraitView.gameObject);
                CharacterViews.Remove(key);
            }
        }
    }

    abstract public void AddPortraits(string[] portraitNames);
}
