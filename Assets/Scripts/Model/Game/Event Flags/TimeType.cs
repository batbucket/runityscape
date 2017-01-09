using UnityEngine;

namespace Scripts.Model.World.Utility {

    /// <summary>
    /// Type-safe enum used to represent the time of day.
    /// </summary>
    public sealed class TimeType {
        public static readonly TimeType AFTERNOON = new TimeType(2, "Afternoon", Color.red, "The sun is at its highest point.");
        public static readonly TimeType DAWN = new TimeType(0, "Dawn", Color.red, "The sun is just at the horizon.");
        public static readonly TimeType DUSK = new TimeType(4, "Dusk", Color.magenta, "The sun is just at the horizon.");
        public static readonly TimeType EVENING = new TimeType(3, "Evening", Color.yellow, "The sun begins its descent.");
        public static readonly TimeType MORNING = new TimeType(1, "Morning", Color.green, "The sun begins its ascent.");
        public static readonly TimeType NIGHT = new TimeType(5, "Night", Color.blue, "The area is completely dark.");
        public readonly Color Color;
        public readonly string Description;
        public readonly int Index;
        public readonly string Name;

        private TimeType(int index, string name, Color color, string description) {
            this.Index = index;
            this.Name = name;
            this.Color = color;
            this.Description = description;
        }

        public TimeType Next {
            get {
                TimeType ret = null;
                switch (Index) {
                    case 0:
                        ret = MORNING;
                        break;

                    case 1:
                        ret = AFTERNOON;
                        break;

                    case 2:
                        ret = EVENING;
                        break;

                    case 3:
                        ret = DUSK;
                        break;

                    case 4:
                        ret = NIGHT;
                        break;

                    case 5:
                        ret = NIGHT;
                        break;
                }
                return ret;
            }
        }

        public static TimeType Get(int index) {
            TimeType ret = null;
            switch (index) {
                case 0:
                    ret = DAWN;
                    break;

                case 1:
                    ret = MORNING;
                    break;

                case 2:
                    ret = AFTERNOON;
                    break;

                case 3:
                    ret = EVENING;
                    break;

                case 4:
                    ret = DUSK;
                    break;

                case 5:
                    ret = NIGHT;
                    break;

                default:
                    Util.Assert(false, "Bad index: " + index);
                    break;
            }
            return ret;
        }
    }
}