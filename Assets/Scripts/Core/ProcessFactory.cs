using UnityEngine;
using System;

public static class ProcessFactory {
    public static Process createProcess(string description, Action play) {
        Process process = new Process();
        process.setDescription(description);
        process.setPlay(play);
        return process;
    }

    public static UndoableProcess createUndoableProcess(string description, Action play, Action undo) {
        UndoableProcess process = new UndoableProcess();
        process.setDescription(description);
        process.setPlay(play);
        process.setUndo(undo);
        return process;
    }
}
