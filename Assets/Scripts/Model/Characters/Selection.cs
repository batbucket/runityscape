
using System;
/**
* Type-Safe Enum Pattern
*/
public sealed class Selection : IComparable {
    public string Name { get; private set; } //Name of selection: Example: Spell
    public string Declare { get; private set; } //"X will do Y"
    public string Question { get; private set; } //"What will X do?"
    public string Extra { get; private set; } //Extra tooltip
    public Type SelectionType { get; private set; }
    public int Index { get; private set; } //Index of this Selection

    private Selection(Type type, string name, string declare, string question, int index, string extra = "") {
        this.SelectionType = type;
        this.Name = name;
        this.Declare = declare;
        this.Question = question;
        this.Extra = extra;
        this.Index = index;
    }

    public static readonly Selection FAIM = new Selection(Type.FAIM, "FAIM", "null", "What will {0} do?", -1);
    public static readonly Selection SPELL = new Selection(Type.SPELL, "Spell", "{0} will cast a SPELL.", "What SPELL will {0} cast?", 4);
    public static readonly Selection ACT = new Selection(Type.ACTION, "Action", "{0} will perform an ACTION.", "What ACTION will {0} perform?", 5);
    public static readonly Selection ITEM = new Selection(Type.ITEM, "Item", "{0} will use an ITEM.", "What ITEM will {0} use?", 6);
    public static readonly Selection MERCY = new Selection(Type.MERCY, "Mercy", "{0} will show MERCY.", "How will {0} show MERCY?", 7);
    public static readonly Selection EQUIP = new Selection(Type.EQUIP, "Equip", "{0} will manage EQUIPMENT.", "What EQUIPMENT will {0} use?", 10);
    public static readonly Selection TARGET = new Selection(Type.TARGET, "Target", "{0} will TARGET {1} with {2}.", "Who will {0} TARGET with {1}?", -1);
    public static readonly Selection SWITCH = new Selection(Type.SWITCH, "Switch", "{0} will GIVE control to another character.", "Who will {0} SWITCH with?", 11);

    public enum Type {
        FAIM, SPELL, ACTION, ITEM, MERCY, TARGET, SWITCH, EQUIP
    }

    public int CompareTo(object obj) {
        return this.SelectionType.CompareTo(((Selection)obj).SelectionType);
    }

    public override bool Equals(object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to Selection return false.
        Selection p = obj as Selection;
        if ((object)p == null) {
            return false;
        }

        // Return true if the fields match:
        return p.SelectionType == this.SelectionType;
    }

    public override int GetHashCode() {
        return SelectionType.GetHashCode();
    }
}