using System;

public interface IProcess {
    void setPlay(Action action);
    void setUndo(Action action);
    void play();
    void undo();
}
