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
        resources = Util.FindChild(gameObject, "Resources");
    }

    public void setPortraitName(string name) {
        portraitName.text = name;
    }

    public void setPortraitColor(Color color) {
        portraitName.color = color;
    }

    public virtual void setSprite(Sprite image) {
        iconImage.sprite = image;
    }

    public void setIconColor(Color color) {
        iconImage.color = color;
    }

    protected ResourceManager addResource(string resourceName, Color overBarColor, Color underBarColor, int numerator, int denominator, string resourceLocation) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load(resourceLocation));
        Util.Parent(g, Util.FindChild(gameObject, "Resources"));
        ResourceManager rm = g.GetComponent<ResourceManager>();
        rm.setResourceName(resourceName);
        rm.setOverBarColor(overBarColor);
        rm.setUnderBarColor(underBarColor);
        rm.setFraction(numerator, denominator);
        rm.setBarScale((numerator + 0.0f / denominator));
        return rm;
    }

    /**
     * Has to be abstract because the resource prefab is different
     * depending on whether it's left or right on the screen
     */
    abstract public ResourceManager addResource(string resourceName, Color overBarColor, Color underBarColor, int numerator, int denominator);
}
