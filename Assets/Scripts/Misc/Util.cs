using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WindowsInput;
using System.Text;
using System;
using System.Collections.Generic;

/**
 * This class holds various helper methods that don't fit anywhere else
 */
public static class Util {
    public static void setTextAlpha(Text target, float alpha) {
        Color c = target.color;
        c.a = alpha;
        target.color = c;
    }

    public static void setImageAlpha(Image target, float alpha) {
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

    static char[] splitChars = new char[] { ' ', '-', '\t' };

    public static string wordWrap(string str, int width) {
        string[] words = explode(str, splitChars);

        int curLineLength = 0;
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < words.Length; i += 1) {
            string word = words[i];
            // If adding the new word to the current line would be too long,
            // then put it on a new line (and split it up if it's too long).
            if (curLineLength + word.Length > width) {
                // Only move down to a new line if we have text on the current line.
                // Avoids situation where wrapped whitespace causes emptylines in text.
                if (curLineLength > 0) {
                    strBuilder.Append(Environment.NewLine);
                    curLineLength = 0;
                }

                // If the current word is too long to fit on a line even on it's own then
                // split the word up.
                while (word.Length > width) {
                    strBuilder.Append(word.Substring(0, width - 1) + "-");
                    word = word.Substring(width - 1);

                    strBuilder.Append(Environment.NewLine);
                }

                // Remove leading whitespace from the word so the new line starts flush to the left.
                word = word.TrimStart();
            }
            strBuilder.Append(word);
            curLineLength += word.Length;
        }

        return strBuilder.ToString();
    }

    private static string[] explode(string str, char[] splitChars) {
        List<string> parts = new List<string>();
        int startIndex = 0;
        while (true) {
            int index = str.IndexOfAny(splitChars, startIndex);

            if (index == -1) {
                parts.Add(str.Substring(startIndex));
                return parts.ToArray();
            }

            string word = str.Substring(startIndex, index - startIndex);
            char nextChar = str.Substring(index, 1)[0];
            // Dashes and the likes should stick to the word occuring before it. Whitespace doesn't have to.
            if (char.IsWhiteSpace(nextChar)) {
                parts.Add(word);
                parts.Add(nextChar.ToString());
            } else {
                parts.Add(word + nextChar);
            }

            startIndex = index + 1;
        }
    }

    public static Color hexToColor(string hex) {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8) {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color(r, g, b, a);
    }
}
