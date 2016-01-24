using System;

/**
 * This class represents an Action
 * That can do something,
 * that is described by a description
 */
public interface IProcess {
    string Name { get; }
    string Description { get; }
    Action Action { get; }
    void Play();
}
