using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class PortraitManager : MonoBehaviour {

    Text portraitName;
    Image iconImage;
    GameObject resources;

    // Use this for initialization
    void Start() {
        portraitName = gameObject.GetComponentInChildren<Text>();
        iconImage = gameObject.GetComponentInChildren<Image>();
        resources = Util.findChild(gameObject, "Resources");
    }

    // Update is called once per frame
    void Update() {

    }

    public void setPortraitName(string name) {
        portraitName.text = name;
    }

    public void setIconImage(Sprite image) {
        iconImage.sprite = image;
    }

    /**
     * Has to be abstract because the resource prefab is different
     * depending on whether it's left or right on the screen
     */
    abstract public ResourceManager addResource();
}
