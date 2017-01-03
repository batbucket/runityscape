using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.Sounds {

    /// <summary>
    /// This class manages the sounds that play during the game.
    /// </summary>
    public class SoundView : MonoBehaviour {
        private const string MUSIC_PREFIX = "Music/";
        private const string SOUND_PREFIX = "Sounds/";
        public IList<AudioSource> Loops { get; private set; }
        public IDictionary<string, AudioClip> Sound { get; private set; }

        public void LoopMusic(string resourceLocation) {
            if (string.IsNullOrEmpty(resourceLocation)) {
                return;
            }
            string loc = MUSIC_PREFIX + resourceLocation;
            Util.Assert(Resources.Load<AudioClip>(loc) != null, "Music location does not exist!");
            if (!Sound.ContainsKey(resourceLocation)) {
                Sound[resourceLocation] = Resources.Load<AudioClip>(loc);
            }
            for (int i = 0; i < Loops.Count; i++) {
                Destroy(Loops[i]);
            }
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = Sound[resourceLocation];
            source.loop = true;
            source.Play();
            Loops.Add(source);
        }

        public void PlaySound(string resourceLocation) {
            if (string.IsNullOrEmpty(resourceLocation)) {
                return;
            }
            string loc = SOUND_PREFIX + resourceLocation;
            Util.Assert(Resources.Load<AudioClip>(loc) != null, string.Format("Sound location \"{0}\" does not exist!", loc));
            if (!Sound.ContainsKey(resourceLocation)) {
                Sound[resourceLocation] = Resources.Load<AudioClip>(loc);
            }
            StartCoroutine(PlayThenDestroy(resourceLocation));
        }

        public void StopAllSounds() {
            StopAllCoroutines();
            for (int i = 0; i < Loops.Count; i++) {
                Destroy(Loops[i]);
            }
        }

        private void Awake() {
            this.Sound = new Dictionary<string, AudioClip>();
            this.Loops = new List<AudioSource>();
        }

        private IEnumerator PlayThenDestroy(string resourceLocation) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = Sound[resourceLocation];
            source.Play();
            while (source.isPlaying) {
                yield return null;
            }
            Destroy(source);
            yield break;
        }
    }
}