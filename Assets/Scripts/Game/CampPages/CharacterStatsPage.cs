using System.Collections.Generic;

public class CharacterStatsPage : ReadPage {
    public CharacterStatsPage(Page back, Character c) :
        base(
        "",
        "",
        c.DisplayName,
        false,
        c,
        new Character[] { c }) {

        OnEnterAction = () => {
            Game.Instance.TextBoxes.AddTextBox(new TextBox(StatText(c)));
            if (c.HasResource(ResourceType.EXPERIENCE)) {
                ActionGrid[0] = LevelUp(c);
            }
            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;

            if (IsLevelUp(c)) {
                Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} has enough experience to level up.", c.DisplayName)));
            }
        };
    }

    public static string StatText(Character target) {
        List<string> a = new List<string>();
        foreach (KeyValuePair<AttributeType, Attribute> pair in target.Attributes) {
            if (pair.Key.IsAssignable) {
                a.Add((string.Format("{1}/{2} {0}: {3}", Util.Color(pair.Key.Name, pair.Key.Color), pair.Value.False, pair.Value.True, pair.Key.ShortDescription)));
            }
        }

        List<string> r = new List<string>();
        foreach (KeyValuePair<ResourceType, Resource> pair in target.Resources) {
            if (pair.Value.IsVisible) {
                r.Add(string.Format("{1}/{2} {0}: {3}", Util.Color(pair.Key.Name, pair.Key.FillColor), pair.Value.False, pair.Value.True, pair.Key.Description));
            }
        }
        return string.Format(
            "Level {1} {0}\n{2}{3}{4}",
            target.DisplayName,
            target.Level.False,
            string.IsNullOrEmpty(target.CheckText) ? "" : string.Format("\n{0}", target.CheckText),
            "\nAttributes:\n" + string.Join("\n", a.ToArray()),
            "\nResources:\n" + string.Join("\n", r.ToArray()),
            target.Equipment.Count <= 0 ? "" : string.Format("\n{0}", target.Equipment.ToString())
            );
    }

    private Process LevelUp(Character c) {
        return new Process(
            c.GetResourceCount(ResourceType.EXPERIENCE, false) < c.GetResourceCount(ResourceType.EXPERIENCE, true) ?
            string.Format("{0}/{1} XP", c.GetResourceCount(ResourceType.EXPERIENCE, false), c.GetResourceCount(ResourceType.EXPERIENCE, true))
            : "Level up!",
            string.Format("{0} will be able to level up if their experience reaches its cap.", c.DisplayName),
            () => {
                Game.Instance.CurrentPage = new LevelUpPage(this, c);
            },
            () => c.GetResourceCount(ResourceType.EXPERIENCE, false) >= c.GetResourceCount(ResourceType.EXPERIENCE, true)
            );
    }

    private static bool IsLevelUp(Character c) {
        return c.HasResource(ResourceType.EXPERIENCE) && c.GetResourceCount(ResourceType.EXPERIENCE, false) >= c.GetResourceCount(ResourceType.EXPERIENCE, true);
    }

}
