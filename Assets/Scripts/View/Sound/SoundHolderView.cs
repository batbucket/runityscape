using UnityEngine;
using System.Linq;
using Scripts.View.Sound;
using System.Collections.Generic;
using Scripts.View.ObjectPool;

namespace Scripts.View.Sounds {

    public abstract class SoundHolderView : MonoBehaviour {
        private IDictionary<int, ClipView> clips;

        private void Start() {
            clips = new Dictionary<int, ClipView>();
        }

        public void StopAllSounds() {
            IList<int> clipIDs = clips.Keys.ToList();
            for (int i = 0; i < clipIDs.Count; i++) {
                Stop(clipIDs[i]);
            }
        }

        public void Add(ClipView clip) {
            Util.Parent(clip.gameObject, this.gameObject);
            ModifyClip(clip);
            clip.Play();
            clips.Add(clip.gameObject.GetInstanceID(), clip);
        }

        public void Stop(int instanceID) {
            if (clips.ContainsKey(instanceID)) {
                ObjectPoolManager.Instance.Return(clips[instanceID]);
                clips.Remove(instanceID);
            }
        }

        protected abstract void ModifyClip(ClipView clip);
    }
}