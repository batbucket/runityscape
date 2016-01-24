using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WindowsInput;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

/**
 * This class holds various helper methods that don't fit anywhere else
 */
public static class Util {

    public static bool Chance(double probability) {
        return UnityEngine.Random.Range(0f, 1f) < probability;
    }

    public static int Range(double min, double max) {
        return (int) UnityEngine.Random.Range((float) min, (float) max);
    }

    public static void SetTextAlpha(Text target, float alpha) {
        Color c = target.color;
        c.a = alpha;
        target.color = c;
    }

    public static void SetImageAlpha(Image target, float alpha) {
        Color c = target.color;
        c.a = alpha;
        target.color = c;
    }

    public static void Parent(GameObject child, GameObject parent) {
        child.transform.SetParent(parent.transform);
    }

    public static GameObject FindChild(GameObject parent, string childName) {
        return parent.transform.FindChild(childName).gameObject;
    }

    /**
     * Converts an AudioClip to an AudioSource
     */
    public static AudioSource ClipToSource(AudioClip clip) {
        AudioSource source = new AudioSource();
        source.clip = clip;
        return source;
    }

    public static void Assert(bool statement) {
        if (!statement) {
            throw new UnityException("Expected value: true");
        }
    }

    /**
     * Horrible horrible hack for converting KeyCode (unity) to VirtualKeyCode (windows)
     *
     * Parse searches for an enum with a certain string.
     * KeyCode's toString (For the keys we deem important has [KEY] uppercase
     * VirtualKeyCode's toString (For the keys we deem important has VK_[KEY]
     * So we search for VK_ + [KEYCODE]
     */
    public static VirtualKeyCode KeyCodeToVirtualKeyCode(KeyCode keyToConvert) {
        return (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), "VK_" + keyToConvert.ToString());
    }

    public static Color InvertColor(Color color) {
        return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
    }

    static char[] splitChars = new char[] { ' ', '-', '\t', '\n' };

    public static string WordWrap(string str, int width) {
        string[] words = Explode(str, splitChars);

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

    private static string[] Explode(string str, char[] splitChars) {
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

    public static Color HexToColor(string hex) {
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

    public static string Color(string s, Color c) {
        return string.Format("<color={1}>{0}</color>", s, RGBToHex(c));
    }

    public static string RGBToHex(Color color) {
        float redValue = color.r;
        float greenValue = color.g;
        float blueValue = color.b;
        float alpha = color.a;

        //1 checking is for strange [0-1] interval formatting of numbers. Otherwise 256 format
        return (string.Format("#{0}{1}{2}{3}",
            ((int)(redValue <= 1 ? redValue * 255 : redValue)).ToString("X2"),
            ((int)(greenValue <= 1 ? greenValue * 255 : greenValue)).ToString("X2"),
            ((int)(blueValue <= 1 ? blueValue * 255 : greenValue)).ToString("X2"),
            ((int)(alpha <= 1 ? alpha * 255 : alpha)).ToString("X2")));
    }

    public static T PickRandom<T>(this IEnumerable<T> source) {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static void KillAllChildren(GameObject parent) {
        foreach (Transform child in parent.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static Sprite GetSprite(string name) {
        return Resources.Load<Sprite>(name);
    }

    /**
     * Converts 1 to A, 2 to B, and so on
     */
    public static string IntToLetter(int column) {
        string columnString = "";
        decimal columnNumber = column;
        while (columnNumber > 0) {
            decimal currentLetterNumber = (columnNumber - 1) % 26;
            char currentLetter = (char)(currentLetterNumber + 65);
            columnString = currentLetter + columnString;
            columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
        }
        return columnString;
    }
}
