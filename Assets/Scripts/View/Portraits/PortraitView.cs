using Scripts.Model.Stats;
using Scripts.Presenter;
using Scripts.View.Effects;
using Scripts.View.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Portraits {

    /// <summary>
    /// This class represents a Portrait viewable
    /// on either the left or right side of the screen.
    /// </summary>
    public class PortraitView : PooledBehaviour {
        public CharacterPresenter Presenter;

        [SerializeField]
        private BuffView buffPrefab;

        [SerializeField]
        private GameObject buffsHolder;

        [SerializeField]
        private GameObject effectsHolder;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text portraitName;

        //Name of the character
        [SerializeField]
        private ResourceView resourcePrefab;

        //Image of the character
        [SerializeField]
        private GameObject resourcesHolder;

        public GameObject BuffsHolder { get { return buffsHolder; } }

        /// <summary>
        /// Dictionary of all the buff boxes.
        /// </summary>
        public IDictionary<int, BuffBundle> BuffViews { get; private set; }

        public GameObject EffectsHolder { get { return effectsHolder; } }

        public Image Image {
            get {
                return iconImage;
            }
            set {
                iconImage = value;
            }
        }

        public string PortraitName { get { return portraitName.text; } set { portraitName.text = value; } }
        public Text PortraitText { get { return portraitName; } }
        public IDictionary<StatType, ResourceBundle> ResourceViews { get; private set; }
        public Sprite Sprite { get { return iconImage.sprite; } set { iconImage.sprite = value; } }

        public override void Reset() {
            PortraitName = "";
            PortraitText.color = Color.white;
            Image.color = Color.white;
            Image.enabled = true;

            ResourceView[] rvs = resourcesHolder.GetComponentsInChildren<ResourceView>();
            BuffView[] bvs = buffsHolder.GetComponentsInChildren<BuffView>();
            PooledBehaviour[] ces = effectsHolder.GetComponentsInChildren<PooledBehaviour>();

            for (int i = 0; i < rvs.Length; i++) {
                ObjectPoolManager.Instance.Return(rvs[i]);
            }

            for (int i = 0; i < bvs.Length; i++) {
                ObjectPoolManager.Instance.Return(bvs[i]);
            }

            for (int i = 0; i < ces.Length; i++) {
                ObjectPoolManager.Instance.Return(ces[i]);
            }

            ResourceViews.Clear();
            BuffViews.Clear();
        }

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
                    bv = ObjectPoolManager.Instance.Get(buffPrefab);
                    Util.Parent(bv.gameObject, BuffsHolder);
                } else {
                    bv = BuffViews[b.id].buffView;
                }

                // Tooltip details
                bv.Tooltip.Sprite = b.sprite;
                bv.Tooltip.Title = b.name;
                bv.Tooltip.Body = b.description;

                bv.Text = b.name;
                bv.Icon = b.sprite;
                bv.Color = b.color;
                bv.Duration = b.duration;
                BuffViews[b.id] = new BuffBundle { buffView = bv, isSet = true };
            }

            //Check if any isSets are false, if so, remove them and Destroy their gameObjects.
            //We can use same keys list as before since newly added keys cannot be false
            foreach (int key in keys) {
                if (!BuffViews[key].isSet) {
                    ObjectPoolManager.Instance.Return(BuffViews[key].buffView);
                    BuffViews.Remove(key);
                }
            }
        }

        public void SetResources(StatType[] resourceTypes) {
            //Set all existing isSets to false.
            List<StatType> keys = new List<StatType>(ResourceViews.Keys); //Can't modify Dictionary in foreach loop
            foreach (StatType key in keys) {
                ResourceViews[key] = new ResourceBundle { resourceView = ResourceViews[key].resourceView, isSet = false };
            }

            //Add or possibly replace new ResourceViews.
            foreach (StatType resourceType in resourceTypes) {
                ResourceView rv;
                if (!ResourceViews.ContainsKey(resourceType)) {
                    // Instantiate new resource prefab
                    rv = ObjectPoolManager.Instance.Get(resourcePrefab);
                    Util.Parent(rv.gameObject, resourcesHolder); // Placed in back.

                    // Set tooltip details
                    rv.Tip.Sprite = resourceType.Sprite;
                    rv.Tip.Title = resourceType.Name;

                    // Set resource prefab's details
                    rv.Type = resourceType;
                    rv.Sprite = resourceType.Sprite;
                    rv.FillColor = resourceType.Color;
                    rv.EmptyColor = resourceType.NegativeColor;

                    // Move resource prefab to appropraite location, resources are ordered.
                    ResourceView[] rvs = resourcesHolder.GetComponentsInChildren<ResourceView>();

                    // Sort everything so HP bar doesn't show up under mana, and etc.
                    Array.Sort(rvs);
                } else {
                    rv = ResourceViews[resourceType].resourceView;
                }

                ResourceViews[resourceType] = new ResourceBundle { resourceView = rv, isSet = true };
            }

            //Check if any isSets are false, if so, remove them and Destroy their gameObjects.
            //We can use same keys list as before since newly added keys cannot be false
            foreach (StatType key in keys) {
                if (!ResourceViews[key].isSet) {
                    if (ResourceViews[key].resourceView != null) {
                        ObjectPoolManager.Instance.Return(ResourceViews[key].resourceView);
                    }
                    ResourceViews.Remove(key);
                }
            }
        }

        // Use this for initialization
        private void Awake() {
            ObjectPoolManager.Instance.Register(buffPrefab, 10);
            ObjectPoolManager.Instance.Register(resourcePrefab, 4);

            ResourceViews = new SortedDictionary<StatType, ResourceBundle>();
            BuffViews = new SortedDictionary<int, BuffBundle>();
        }

        public struct BuffBundle {
            public BuffView buffView;
            public bool isSet;
        }

        public struct BuffParams {
            public int id;
            public Color color;
            public Sprite sprite;
            public string name;
            public string duration;
            public string description;
        }

        public struct ResourceBundle {
            public bool isSet;
            public ResourceView resourceView;
        }
    }
}