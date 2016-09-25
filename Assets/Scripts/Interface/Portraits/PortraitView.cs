using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class controls the details one sees on a Portrait prefab
 */
public abstract class PortraitView : MonoBehaviour {
    public CharacterPresenter Presenter;

    public struct ResourceBundle {
        public ResourceView resourceView;
        public bool isSet;
    }

    public struct AttributeBundle {
        public int trueValue;
        public int falseValue;
    }

    public struct BuffParams {
        public int id;
        public string abbreviation;
        public Color color;
        public string duration;
    }
    public struct BuffBundle {
        public BuffView buffView;
        public bool isSet;
    }
    public IDictionary<int, BuffBundle> BuffViews { get; private set; }
    public IDictionary<string, CharacterEffect> Effects { get; private set; }

    [SerializeField]
    Text _portraitName; //Name of the character
    public string PortraitName { get { return _portraitName.text; } set { _portraitName.text = value; } }
    public Text PortraitText { get { return _portraitName; } }

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
    [SerializeField]
    GameObject _buffsHolder;
    public GameObject BuffsHolder { get { return _buffsHolder; } }
    [SerializeField]
    GameObject _buffPrefab;

    // Use this for initialization
    void Awake() {
        ResourceViews = new SortedDictionary<ResourceType, ResourceBundle>();
        AttributeViews = new SortedDictionary<AttributeType, AttributeBundle>();
        BuffViews = new SortedDictionary<int, BuffBundle>();
        Effects = new Dictionary<string, CharacterEffect>();
    }

    public void AddEffect(CharacterEffect ce) {
        CharacterEffect current;
        Effects.TryGetValue(ce.ID, out current);
        if (current != null && !current.IsDone) {
            current.StopCoroutine();
            current.CancelEffect();
        }
        ce.StartCoroutine();
        Effects[ce.ID] = ce;
    }

    public void ClearEffects() {
        foreach (KeyValuePair<string, CharacterEffect> pair in Effects) {
            pair.Value.StopCoroutine();
            pair.Value.CancelEffect();
        }
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
                // Instantiate new resource prefab
                GameObject g = (GameObject)GameObject.Instantiate(resourcePrefab);
                Util.Parent(g, ResourcesHolder); // Placed in back.

                // Set resource prefab's details
                rv = g.GetComponent<ResourceView>();
                rv.Type = resourceType;
                rv.ResourceName = resourceType.ShortName;
                rv.ResourceColor = resourceType.FillColor;
                rv.OverColor = resourceType.FillColor;
                rv.UnderColor = resourceType.EmptyColor;

                // Move resource prefab to appropraite location, resources are ordered.
                ResourceView[] rvs = ResourcesHolder.GetComponentsInChildren<ResourceView>();
                int index = rvs.Length - 1;

                // Possibly working bubble sort
                while (index - 1 >= 0 && rv.Type.CompareTo(rvs[index - 1].Type) < 0) {
                    Util.Swap(rvs[index].gameObject, rvs[index - 1].gameObject);
                    index--;
                }

            } else {
                rv = ResourceViews[resourceType].resourceView;
            }

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

    public void SetBuffs(BuffParams[] buffParams) {
        //Set all existing isSets to false.
        List<int> keys = new List<int>(BuffViews.Keys); //Can't modify Dictionary in foreach loop
        foreach (int key in keys) {
            BuffViews[key] = new BuffBundle { buffView = BuffViews[key].buffView, isSet = false };
        }

        //Add or possibly replace new BuffViews.
        foreach (BuffParams b in buffParams) {
            BuffView bv;
            if (!BuffViews.ContainsKey(b.id)) {
                GameObject g = Instantiate(_buffPrefab);
                Util.Parent(g, BuffsHolder);
                bv = g.GetComponent<BuffView>();
            } else {
                bv = BuffViews[b.id].buffView;
            }
            bv.Text = b.abbreviation;
            bv.Color = b.color;
            bv.Duration = b.duration;
            BuffViews[b.id] = new BuffBundle { buffView = bv, isSet = true };
        }

        //Check if any isSets are false, if so, remove them and Destroy their gameObjects.
        //We can use same keys list as before since newly added keys cannot be false
        foreach (int key in keys) {
            if (!BuffViews[key].isSet) {
                Destroy(BuffViews[key].buffView.gameObject);
                BuffViews.Remove(key);
            }
        }
    }
}
