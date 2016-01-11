using UnityEngine;
using System.Collections;
using System;

public interface IUndoableProcess : IProcess {
    void setUndo(System.Action action);
    void undo();
}
