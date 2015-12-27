using System;

public interface IProcess {
    void setPlay(Action a);
    void setRewind(Action a);
    void play();
    void rewind();
}