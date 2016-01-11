using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class ActionGridFactory {

    public static List<Process> createProcesses(    string n0 = "", string d0  = "", System.Action a0  = null,
                                                    string n1 = "", string d1  = "", System.Action a1  = null,
                                                    string n2 = "", string d2  = "", System.Action a2  = null,
                                                    string n3 = "", string d3  = "", System.Action a3  = null,
                                                    string n4 = "", string d4  = "", System.Action a4  = null,
                                                    string n5 = "", string d5  = "", System.Action a5  = null,
                                                    string n6 = "", string d6  = "", System.Action a6  = null,
                                                    string n7 = "", string d7  = "", System.Action a7  = null) {
        List<Process> list = new List<Process>();
        list.Add(new Process(n0,  d0,  a0 ));
        list.Add(new Process(n1,  d1,  a1 ));
        list.Add(new Process(n2,  d2,  a2 ));
        list.Add(new Process(n3,  d3,  a3 ));
        list.Add(new Process(n4,  d4,  a4 ));
        list.Add(new Process(n5,  d5,  a5 ));
        list.Add(new Process(n6,  d6,  a6 ));
        list.Add(new Process(n7,  d7,  a7 ));
        return list;
    }

    public static List<Process> createUndoableProcesses(string n0 = "", string d0  = "", System.Action a0  = null, System.Action u0  = null,
                                                        string n1 = "", string d1  = "", System.Action a1  = null, System.Action u1  = null,
                                                        string n2 = "", string d2  = "", System.Action a2  = null, System.Action u2  = null,
                                                        string n3 = "", string d3  = "", System.Action a3  = null, System.Action u3  = null,
                                                        string n4 = "", string d4  = "", System.Action a4  = null, System.Action u4  = null,
                                                        string n5 = "", string d5  = "", System.Action a5  = null, System.Action u5  = null,
                                                        string n6 = "", string d6  = "", System.Action a6  = null, System.Action u6  = null,
                                                        string n7 = "", string d7  = "", System.Action a7  = null, System.Action u7  = null) {
        List<Process> list = new List<Process>();
        list.Add(new UndoableProcess(n0,  d0,  a0,  u0 ));
        list.Add(new UndoableProcess(n1,  d1,  a1,  u1 ));
        list.Add(new UndoableProcess(n2,  d2,  a2,  u2 ));
        list.Add(new UndoableProcess(n3,  d3,  a3,  u3 ));
        list.Add(new UndoableProcess(n4,  d4,  a4,  u4 ));
        list.Add(new UndoableProcess(n5,  d5,  a5,  u5 ));
        list.Add(new UndoableProcess(n6,  d6,  a6,  u6 ));
        list.Add(new UndoableProcess(n7,  d7,  a7,  u7 ));
        return list;
    }
}
