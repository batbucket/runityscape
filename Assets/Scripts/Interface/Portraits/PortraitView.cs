using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class controls the details one sees on a Portrait prefab
 */
public abstract class PortraitView : MonoBehaviour {
    public struct ResourceBundle {
        public ResourceView resourceView;
        public bool isSet;
    }

    public struct AttributeBundle {
        public int trueValue;
        public int falseValue;
    }

    [SerializeField]
    Text _portraitName; //Name of the character
    public string PortraitName { get { return _portraitName.text; } set { _portraitName.text = value; } }

    [SerializeField]
    Image _iconImage; //Image of the character
    public Image Image { get { return _iconImage; } set { _iconImage = value; } }
    public Sprite Sprite { get { return _iconImage.sprite; } set { _iconImage.sprite = value; } }
    public IDictionary<ResourceType, ResourceBundle> ResourceViews { get; private set; }
    public IDictionary<AttributeType, AttributeBundle> AttributeViews { get; private set; }

    [SerializeField]
    GameObject ResourcesHolder;
    [SerializeField]
    GameObject _hitsplatsHolder;
    public GameObject Hitsplats { get { return _hitsplatsHolder; } }

    // Use this for initialization
    void Awake() {
        ResourceViews = new SortedDictionary<ResourceType, ResourceBundle>();
        AttributeViews = new SortedDictionary<AttributeType, AttributeBundle>();
    }

    protected void SetResources(ResourceType[] resourceTypes, GameObject resourcePrefab) {

        //Set all existing isSets to false.
        List<ResourceType> keys = new List<ResourceType>(ResourceViews.Keys); //Can't modify Dictionary in foreach loop
        foreach (ResourceType key in keys) {
            ResourceViews[key] = new ResourceBundle { resourceView = ResourceViews[key].resourceView, isSet = false };
        }

        //Add or possibly replace new ResourceViews.
        foreach (ResourceType resourceType in resourceTypes) {
            ResourceView rv;
            if (!ResourceViews.ContainsKey(resourceType)) {
                GameObject g = (GameObject)GameObject.Instantiate(resourcePrefab);
                Util.Parent(g, ResourcesHolder);
                rv = g.GetComponent<ResourceView>();
            } else {
                rv = ResourceViews[resourceType].resourceView;
            }
            rv.ResourceName = resourceType.ShortName;
            rv.ResourceColor = resourceType.FillColor;
            rv.OverColor = resourceType.FillColor;
            rv.UnderColor = resourceType.EmptyColor;
            ResourceViews[resourceType] = new ResourceBundle { resourceView = rv, isSet = true };
        }

        //Check if any isSets are false, if so, remove them and Destroy their gameObjects.
        //We can use same keys list as before since newly added keys cannot be false
        foreach (ResourceType key in keys) {
            if (!ResourceViews[key].isSet) {
                GameObject.Destroy(ResourceViews[key].resourceView.gameObject);
                ResourceViews.Remove(key);
            }
        }
    }

    public void SetAttributes(AttributeType attribute, AttributeBundle bundle) {
        AttributeViews[attribute] = bundle;
    }

    //Show stats OnMouseOver
    void OnMouseOver() {
        List<string> s = new List<string>();
        foreach (KeyValuePair<AttributeType, AttributeBundle> pair in AttributeViews) {
            s.Add(string.Format("{0}: {1}/{2}", pair.Key.ShortName, pair.Value.falseValue, pair.Value.trueValue));
        }
        Game.Instance.Tooltip.Text = string.Join(" ", s.ToArray());
    }

    /**
     * Has to be abstract because the resource prefab is different
     * depending on whether it's left or right on the screen
     */
    abstract public void SetResources(ResourceType[] resourceTypes);
}
