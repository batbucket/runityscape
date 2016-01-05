using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class controls the details one sees on a Portrait prefab
 */
public abstract class PortraitManager : MonoBehaviour {

    Text portraitName; //Name of the character
    protected Image iconImage; //Image of the character
    GameObject resources; //Health points, mana points, those things

    // Use this for initialization
    void Awake() {
        portraitName = gameObject.GetComponentInChildren<Text>();
        iconImage = gameObject.GetComponentInChildren<Image>();
        resources = Util.findChild(gameObject, "Resources");
    }

    public void setPortraitName(string name) {
        portraitName.text = name;
    }

    public void setPortraitColor(Color color) {
        portraitName.color = color;
    }

    public virtual void setIconImage(Sprite image) {
        iconImage.sprite = image;
    }

    public void setIconColor(Color color) {
        iconImage.color = color;
    }

    /**
     * Has to be abstract because the resource prefab is different
     * depending on whether it's left or right on the screen
     */
    abstract public ResourceManager addResource();
}
