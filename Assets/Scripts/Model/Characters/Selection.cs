/**
 * Type-Safe Enum Pattern
 */
public sealed class Selection {
    public string Name { get; } //Name of selection: Example: Spell
    public string Declare { get; } //"X will do Y"
    public string Question { get; } //"What will X do?"
    public string Extra { get; } //Extra tooltip
    public Type SelectionType { get; }

    private Selection(Type type, string name, string declare, string question, string extra = "") {
        this.SelectionType = type;
        this.Name = name;
        this.Declare = declare;
        this.Question = question;
        this.Extra = extra;
    }

    public static readonly Selection FAIM = new Selection(Type.FAIM, "FAIM", "null", "What will {0} do?");
    public static readonly Selection SPELL = new Selection(Type.SPELL, "Spell", "{0} will cast a SPELL.", "What SPELL will {0} cast?");
    public static readonly Selection ACT = new Selection(Type.ACT, "Act", "{0} will perform an ACTION.", "What ACTION will {0} perform?");
    public static readonly Selection ITEM = new Selection(Type.ITEM, "Item", "{0} will use an ITEM.", "What ITEM will {0} use?");
    public static readonly Selection MERCY = new Selection(Type.MERCY, "Mercy", "{0} will show MERCY.", "How will {0} show MERCY?");
    public static readonly Selection TARGET = new Selection(Type.TARGET, "Target", "{0} will TARGET {1} with {2}.", "Who will {0} TARGET with {1}?");

    public enum Type {
        FAIM, SPELL, ACT, ITEM, MERCY, TARGET
    }
}