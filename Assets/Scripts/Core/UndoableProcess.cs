using UnityEngine;
using System.Collections;
using System;

public class UndoableProcess : Process, IUndoableProcess {
    System.Action undoAction;

    public UndoableProcess(string name, string description, System.Action playAction, System.Action undoAction) : base(name, description, playAction) {
        this.undoAction = undoAction;
    }

    public void setUndo(System.Action action) {
        undoAction = action;
    }

    public void undo() {
        if (undoAction != null) {
            undoAction.Invoke();
        }
    }
}
