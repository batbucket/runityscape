using Scripts.Model.TextBoxes;
using Scripts.View.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.TextBoxes {

    public class TextBoxHolderView : MonoBehaviour {
        private const int MAX_NUMBER_OF_TEXTBOXES = 100;

        [SerializeField]
        private InputBoxView inputBoxPrefab;

        [SerializeField]
        private AvatarBoxView leftBoxPrefab;

        [SerializeField]
        private AvatarBoxView rightBoxPrefab;

        /// <summary>
        /// Dictionary of all textbox types and their
        /// prefab
        /// </summary>
        private IDictionary<TextBoxType, PooledBehaviour> textBoxes;

        [SerializeField]
        private TextBoxView textBoxPrefab;

        public InputBoxView AddInputBox() {
            InputBoxView ibv = ObjectPoolManager.Instance.Get(inputBoxPrefab);
            Util.Parent(ibv.gameObject, gameObject);

            return ibv;
        }

        public void ReturnChildren() {
            foreach (PooledBehaviour pb in transform.GetComponentsInChildren<PooledBehaviour>()) {
                ObjectPoolManager.Instance.Return(pb);
            }
        }

        public PooledBehaviour AddTextBox(TextBox textBox) {
            while (transform.childCount > MAX_NUMBER_OF_TEXTBOXES) {
                ObjectPoolManager.Instance.Return(transform.GetComponentsInChildren<PooledBehaviour>()[0]);
            }
            if (string.IsNullOrEmpty(textBox.RawText)) {
                return null;
            }
            PooledBehaviour pb = ObjectPoolManager.Instance.Get(textBoxes[textBox.Type]);

            Util.Parent(pb.gameObject, gameObject);
            pb.transform.SetAsLastSibling();
            textBox.SetupPrefab(pb.gameObject);
            textBox.Write();
            return pb;
        }

        public void AddTextBoxes(IList<TextBox> textBoxes) {
            StartCoroutine(MultiWrite(textBoxes));
        }

        /// <summary>
        /// Types out textboxes, one after the other finishes writing.
        /// </summary>
        /// <param name="textBoxes">Textboxes to string together</param>
        /// <returns></returns>
        private IEnumerator MultiWrite(IList<TextBox> textBoxes) {
            for (int i = 0; i < textBoxes.Count; i++) {
                TextBox t = textBoxes[i];
                AddTextBox(t);
                while (!t.IsDone) {
                    yield return null;
                }
            }
        }

        private void Start() {
            textBoxes = new Dictionary<TextBoxType, PooledBehaviour>() {
            { TextBoxType.TEXT, textBoxPrefab },
            { TextBoxType.LEFT, leftBoxPrefab },
            { TextBoxType.RIGHT, rightBoxPrefab }
        };

            foreach (PooledBehaviour pb in textBoxes.Values) {
                ObjectPoolManager.Instance.Register(pb);
            }
            ObjectPoolManager.Instance.Register(inputBoxPrefab, 1);
        }
    }
}