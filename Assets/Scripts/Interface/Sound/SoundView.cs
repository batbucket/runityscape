using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundView : MonoBehaviour {
    public IDictionary<string, AudioClip> Sound { get; private set; }
    public IList<AudioSource> Loops { get; private set; }

    private const string SOUND_PREFIX = "Sounds/";
    private const string MUSIC_PREFIX = "Music/";

    void Awake() {
        this.Sound = new Dictionary<string, AudioClip>();
        this.Loops = new List<AudioSource>();
    }

    public void Play(string resourceLocation) {
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

    public void Loop(string resourceLocation) {
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

    public void StopAll() {
        StopAllCoroutines();
        for (int i = 0; i < Loops.Count; i++) {
            Destroy(Loops[i]);
        }
    }

    IEnumerator PlayThenDestroy(string resourceLocation) {
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
