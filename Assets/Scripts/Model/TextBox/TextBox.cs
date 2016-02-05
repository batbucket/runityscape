using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

public class TextBox {
    public string[] TextArray { get; private set; }
    public string RawText { get; private set; }
    public Color Color { get; private set; } //Base color
    public float TimePerLetter { get; private set; }
    public string SoundLocation { get; private set; }
    public TextEffect Effect { get; private set; }

    public TextBox(string text, Color color, TextEffect effect = TextEffect.NONE, string soundLocation = "Blip_0", float timePerLetter = 0) {

        //Splits by rich text, then letters
        this.TextArray = Regex.Matches(text, "(<.*?>)|.").Cast<Match>().Select(m => m.Value).ToArray();
        this.RawText = text;
        this.Color = color;
        this.TimePerLetter = timePerLetter;
        this.Effect = effect;
        this.SoundLocation = soundLocation;
    }
}
