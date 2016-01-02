using UnityEngine;
using System;

public static class ProcessFactory {
    public static Process createProcess(Action play, Action undo) {
        Process process = new Process();
        process.setPlay(play);
        process.setUndo(undo);
        return process;
    }
}
