using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WindowsInput;

public static class Util {
    public static void setTextAlpha(Text target, float alpha) {
        Color c = target.color;
        c.a = alpha;
        target.color = c;
    }

    public static void parent(GameObject child, GameObject parent) {
        child.transform.SetParent(parent.transform);
    }

    public static GameObject findChild(GameObject parent, string childName) {
        return parent.transform.FindChild(childName).gameObject;
    }

    /**
     * Converts an AudioClip to an AudioSource
     */
    public static AudioSource clipToSource(AudioClip clip) {
        AudioSource source = new AudioSource();
        source.clip = clip;
        return source;
    }

    /**
     * Horrible horrible hack for converting KeyCode (unity) to VirtualKeyCode (windows)
     *
     * Parse searches for an enum with a certain string.
     * KeyCode's toString (For the keys we deem important has [KEY] uppercase
     * VirtualKeyCode's toString (For the keys we deem important has VK_[KEY]
     * So we search for VK_ + [KEYCODE]
     */
    public static VirtualKeyCode keyCodeToVirtualKeyCode(KeyCode keyToConvert) {
        return (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), "VK_" + keyToConvert.ToString());
    }

    public static Color invertColor(Color color) {
        return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
    }
}
