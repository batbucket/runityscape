using UnityEngine;
using System.Collections;
using System;

public interface IUndoableProcess : IProcess {
    Action UndoAction { get; set; }
    void Undo();
}
