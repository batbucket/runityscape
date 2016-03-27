using System;

public class UndoableProcess : Process, IUndoableProcess {
    public Action UndoAction { get; set; }

    public UndoableProcess(string name = "",
                           string description = "",
                           Action playAction = null,
                           Action undoAction = null)
                            : base(name, description, playAction) {

        this.UndoAction = undoAction ?? (() => { });
    }

    public void Undo() {
        UndoAction.Invoke();
    }
}
