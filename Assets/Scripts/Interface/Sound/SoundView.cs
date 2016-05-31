using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundView : MonoBehaviour {
    public IDictionary<string, AudioClip> Sound { get; private set; }
    public IList<AudioSource> Loops { get; private set; }

    void Awake() {
        this.Sound = new Dictionary<string, AudioClip>();
        this.Loops = new List<AudioSource>();
    }

    public void Play(string resourceLocation) {
        Util.Assert(Resources.Load<AudioClip>(resourceLocation) != null, "Sound location does not exist!");
        if (!Sound.ContainsKey(resourceLocation)) {
            Sound[resourceLocation] = Resources.Load<AudioClip>(resourceLocation);
        }
        StartCoroutine(PlayThenDestroy(resourceLocation));
    }

    public void Loop(string resourceLocation) {
        Util.Assert(Resources.Load<AudioClip>(resourceLocation) != null, "Sound location does not exist!");
        if (!Sound.ContainsKey(resourceLocation)) {
            Sound[resourceLocation] = Resources.Load<AudioClip>(resourceLocation);
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
