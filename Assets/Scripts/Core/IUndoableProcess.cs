using UnityEngine;
using System.Collections;
using System;

public interface IUndoableProcess : IProcess {
    void setUndo(Action action);
    void undo();
}
