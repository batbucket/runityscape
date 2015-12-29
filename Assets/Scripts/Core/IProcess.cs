using System;

/**
 * This class represents an Action
 * That can do something, and undo itself
 */
public interface IProcess {
    void setPlay(Action action);
    void setUndo(Action action);
    void play();
    void undo();
}
