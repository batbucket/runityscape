using System.Collections.Generic;

public class LevelUpPage : ReadPage {

    public LevelUpPage(Page back, Character c) :
        base(
        "",
        "Select an additional attribute to increase.",
        "",
        false,
        new Character[] { c }) {

        OnEnterAction = () => {
            c.AddToResource(ResourceType.EXPERIENCE, false, -c.GetResourceCount(ResourceType.EXPERIENCE, true));
            c.AddToAttribute(AttributeType.LEVEL, false, 1);
            IncreaseAttributes(c);
            this.Location = string.Format("{0}: Level {1} → {2}", c.DisplayName, c.Level.False - 1, c.Level.False);
            Game.Instance.TextBoxes.AddTextBox(new TextBox(IncreaseText(c)));
            ActionGrid = AssignAttribute(c, back);
        };
    }

    public const int ALL_ATT_INCREASE_AMOUNT = 1;
    public const int CHOSEN_ATT_INCREASE_AMOUNT = 1;

    private static string IncreaseText(Character c) {
        List<string> s = new List<string>();
        foreach (AttributeType at in AttributeType.COMBAT_ATTRIBUTES) {
            s.Add(string.Format("{0}: {1} → {2}", at.Name, c.GetAttributeCount(at, true) - ALL_ATT_INCREASE_AMOUNT, c.GetAttributeCount(at, true)));
        }
        return string.Join("\n", s.ToArray());
    }

    private static void IncreaseAttributes(Character c) {
        foreach (AttributeType a in AttributeType.COMBAT_ATTRIBUTES) {
            c.AddToAttribute(a, true, ALL_ATT_INCREASE_AMOUNT, true);
            c.AddToAttribute(a, false, ALL_ATT_INCREASE_AMOUNT, false);
        }
    }

    private IButtonable[] AssignAttribute(Character c, Page back) {
        List<IButtonable> attributes = new List<IButtonable>();
        foreach (AttributeType _at in AttributeType.COMBAT_ATTRIBUTES) {
            AttributeType at = _at;
            if (at.IsAssignable) {
                attributes.Add(new Process(
                    string.Format(Util.Color(at.Name, at.Color)),
                    string.Format("Increase {0} by {1}.\n{0}: {2}", at.Name, CHOSEN_ATT_INCREASE_AMOUNT, at.PrimaryDescription),
                    () => {
                        c.AddToAttribute(at, true, CHOSEN_ATT_INCREASE_AMOUNT, false);
                        c.AddToAttribute(at, false, CHOSEN_ATT_INCREASE_AMOUNT, false);
                        Game.Instance.Sound.Play("Blip_0");
                        Game.Instance.CurrentPage = back;
                    }
                    ));
            }
        }
        return attributes.ToArray();
    }
}
