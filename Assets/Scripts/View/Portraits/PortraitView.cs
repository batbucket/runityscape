using Scripts.Model.Stats;
using Scripts.Presenter;
using Scripts.View.Effects;
using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
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
    public class PortraitView : PooledBehaviour, ITippable {

        [SerializeField]
        private GameObject effectsHolder;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text portraitName;

        [SerializeField]
        private BuffHolderView buffsHolder;

        [SerializeField]
        private ResourceHolderView resourcesHolder;

        [SerializeField]
        private Tooltip.Tip tip;

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
        public Sprite Sprite { get { return iconImage.sprite; } set { iconImage.sprite = value; } }

        Tip ITippable.Tip {
            get {
                return tip;
            }
        }

        public void Setup(Sprite sprite, string title, string body, IEnumerable<ResourceHolderView.ResourceContent> resources, IEnumerable<BuffHolderView.BuffContent> buffs) {
            tip.Setup(sprite, title, body);
            PortraitName = title;
            Sprite = sprite;
            resourcesHolder.AddContents(resources);
            buffsHolder.AddContents(buffs);
        }

        public override void Reset() {
            PortraitName = "";
            PortraitText.color = Color.white;
            Image.color = Color.white;
            Image.enabled = true;
            tip.Reset();

            // Not a class because its just a gameobject things get parented to
            PooledBehaviour[] ces = effectsHolder.GetComponentsInChildren<PooledBehaviour>();
            for (int i = 0; i < ces.Length; i++) {
                ObjectPoolManager.Instance.Return(ces[i]);
            }
        }
    }
}