using System.Collections.Generic;

public interface IPage {
    void onEnter();
    void setOnEnter(Process p);
    void onFirstEnter();
    void setOnFirstEnter(Process p);
    void onExit();
    void setOnExit(Process p);
    void onFirstExit();
    void setOnFirstExit(Process p);
    void setText(string text);
    string getText();
    void setAction(Process p, int r, int c);
    void setActions(List<Process> processes);
    void setLeft(List<Character> characters);
    void setRight(List<Character> characters);
    Process[,] getActions();
}
