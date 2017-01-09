using Scripts.Model.Characters;
using Scripts.Model.Processes;
using Scripts.Model.Stats.Resources;
using Scripts.Model.TextBoxes;
using Scripts.Model.World.Flags;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public static class PageUtil {
        public static void RestoreParty(Party party) {
            foreach (Character c in party) {
                c.CancelBuffs();
                if (c.State == CharacterState.KILLED) {
                    c.AddToResource(ResourceType.HEALTH, false, 1);
                }
                c.State = CharacterState.NORMAL;
            }
        }

        public static void EnterMessages(EventFlags flags) {
            if (TimeType.Get(flags.Ints[Flag.TIME]) == TimeType.NIGHT) {
                Game.Instance.TextBoxes.AddTextBox(new TextBox("It is too <color=blue>dark</color> outside to travel anywhere from camp."));
            }
            if (flags.Ints[Flag.TEMPLE_STATUS] == Flag.TEMPLE_BOSS_CLEARED) {
                Game.Instance.TextBoxes.AddTextBox(new TextBox("Congratulations, you have completed the demo. Thanks for playing my game."));
            }
        }

        public static Process CreateSleep(EventFlags flags, Party party, float sleepRestorePercent) {
            return new Process("Sleep", "End the day.", () => {
                RestoreResources(party, sleepRestorePercent);
                ResetTime(flags);
                AdvanceDays(flags);
                EnterMessages(flags);
                Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party sleeps.")));
            }, () => IsNight(flags));
        }

        public static Process CreateRest(EventFlags flags, Party party, float restRestorePercent) {
            return new Process("Rest", "Rest until the next part of the day.", () => {
                RestoreResources(party, restRestorePercent);
                flags.Ints[Flag.TIME]++;
                Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("The party rests.")));
                PageUtil.EnterMessages(flags);
            }, () => !IsNight(flags));
        }

        public static void RestoreResources(Party party, float percent) {
            foreach (Character c in party.Members) {
                c.CancelBuffs();
                foreach (ResourceType r in ResourceType.RESTORED_RESOURCES) {
                    float missing = c.GetResourceCount(r, true) - c.GetResourceCount(r, false);
                    c.AddToResource(r, false, missing * percent, true);
                }
            }
        }

        public static bool IsNight(EventFlags flags) {
            return flags.Ints[Flag.TIME] == TimeType.NIGHT.Index;
        }

        public static void AdvanceTime(EventFlags flags) {
            flags.Ints[Flag.TIME]++;
        }

        public static void AdvanceDays(EventFlags flags) {
            // Overflow guard against sleep spammers
            if (flags.Ints[Flag.DAYS] < int.MaxValue) {
                flags.Ints[Flag.DAYS]++;
            }
        }

        public static void ResetTime(EventFlags flags) {
            flags.Ints[Flag.TIME] = 0;
        }
    }
}