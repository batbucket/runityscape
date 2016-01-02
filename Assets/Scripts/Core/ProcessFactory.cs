using UnityEngine;
using System;

public static class ProcessFactory {
    public static Process createProcess(string description, Action play, Action undo) {
        Process process = new Process();
        process.setDescription(description);
        process.setPlay(play);
        process.setUndo(undo);
        return process;
    }
}
