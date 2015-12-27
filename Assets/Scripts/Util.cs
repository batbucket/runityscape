using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class Util {
    public static void setTextAlpha(Text target, float alpha) {
        Color c = target.color;
        c.a = alpha;
        target.color = c;
    }

    public static void parent(GameObject child, GameObject parent) {
        child.transform.SetParent(parent.transform);
    }

    /**
     * Converts an AudioClip to an AudioSource
     */
    public static AudioSource clipToSource(AudioClip clip) {
        AudioSource source = new AudioSource();
        source.clip = clip;
        return source;
    }
}
