using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundView : MonoBehaviour {
    public IDictionary<string, AudioSource> Sound { get; private set; }

    void Awake() {
        this.Sound = new Dictionary<string, AudioSource>();
    }

    public void Play(string resourceLocation) {
        Util.Assert(Resources.Load(resourceLocation) != null, "Sound location does not exist!");
        if (!Sound.ContainsKey(resourceLocation)) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>(resourceLocation);
            Sound.Add(resourceLocation, audioSource);
        }
        Sound[resourceLocation].Play();
    }

    public void Play(AudioSource sound) {
        Util.Assert(sound != null, "Sound does not exist!");
        if (!Sound.ContainsKey(sound.name)) {
            Sound.Add(sound.name, sound);
        }
        Sound[sound.name].Play();
    }
}
