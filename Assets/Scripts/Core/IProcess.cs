using System;

/**
 * This class represents an Action
 * That can do something,
 * that is described by a description
 */
public interface IProcess {
    void setName(string name);
    string getName();
    void setDescription(string description);
    string getDescription();
    void setPlay(System.Action action);
    void play();
}
