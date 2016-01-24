using System;

public class UndoableProcess : Process, IUndoableProcess {
    public Action UndoAction { get; private set; }

    public UndoableProcess(string name, string description, System.Action playAction, System.Action undoAction) : base(name, description, playAction) {
        this.UndoAction = undoAction;
    }

    public void Undo() {
        UndoAction.Invoke();
    }
}
