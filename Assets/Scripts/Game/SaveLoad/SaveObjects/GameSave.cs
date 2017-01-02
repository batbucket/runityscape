using System;

public struct GameSave : IRestorable<Camp> {

    public PartySave Party;
    public EventFlagsSave Flags;

    public GameSave(Camp camp) {
        Party = new PartySave(camp.Party);
        Flags = new EventFlagsSave(camp.Flags);
    }

    public Camp Restore() {
        Party p = Party.Restore();
        EventFlags f = Flags.Restore();
        return new Camp(p, f);
    }
}