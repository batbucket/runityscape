using System;

/**
 * This class represents an Action
 * That can do something,
 * that is described by a description
 */
public interface IProcess {
    void setDescription(string description);
    string getDescription();
    void setPlay(Action action);
    void play();
}
