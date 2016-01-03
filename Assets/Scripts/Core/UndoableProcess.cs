using UnityEngine;
using System.Collections;
using System;

public class UndoableProcess : Process, IUndoableProcess {
    Action undoAction;

    public void setUndo(Action action) {
        undoAction = action;
    }

    public void undo() {
        if (undoAction != null) {
            undoAction.Invoke();
        }
    }
}
