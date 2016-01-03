using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class ActionGridFactory {

    public static List<Process> createProcesses(    string d0  = "", Action a0  = null,
                                                    string d1  = "", Action a1  = null,
                                                    string d2  = "", Action a2  = null,
                                                    string d3  = "", Action a3  = null,
                                                    string d4  = "", Action a4  = null,
                                                    string d5  = "", Action a5  = null,
                                                    string d6  = "", Action a6  = null,
                                                    string d7  = "", Action a7  = null,
                                                    string d8  = "", Action a8  = null,
                                                    string d9  = "", Action a9  = null,
                                                    string d10 = "", Action a10 = null,
                                                    string d11 = "", Action a11 = null) {
        List<Process> list = new List<Process>();
        list.Add(ProcessFactory.createProcess(d0,  a0 ));
        list.Add(ProcessFactory.createProcess(d1,  a1 ));
        list.Add(ProcessFactory.createProcess(d2,  a2 ));
        list.Add(ProcessFactory.createProcess(d3,  a3 ));
        list.Add(ProcessFactory.createProcess(d4,  a4 ));
        list.Add(ProcessFactory.createProcess(d5,  a5 ));
        list.Add(ProcessFactory.createProcess(d6,  a6 ));
        list.Add(ProcessFactory.createProcess(d7,  a7 ));
        list.Add(ProcessFactory.createProcess(d8,  a8 ));
        list.Add(ProcessFactory.createProcess(d9,  a9 ));
        list.Add(ProcessFactory.createProcess(d10, a10));
        list.Add(ProcessFactory.createProcess(d11, a11));
        return list;
    }

    public static List<Process> createUndoableProcesses(string d0  = "", Action a0  = null, Action u0  = null,
                                                        string d1  = "", Action a1  = null, Action u1  = null,
                                                        string d2  = "", Action a2  = null, Action u2  = null,
                                                        string d3  = "", Action a3  = null, Action u3  = null,
                                                        string d4  = "", Action a4  = null, Action u4  = null,
                                                        string d5  = "", Action a5  = null, Action u5  = null,
                                                        string d6  = "", Action a6  = null, Action u6  = null,
                                                        string d7  = "", Action a7  = null, Action u7  = null,
                                                        string d8  = "", Action a8  = null, Action u8  = null,
                                                        string d9  = "", Action a9  = null, Action u9  = null,
                                                        string d10 = "", Action a10 = null, Action u10 = null,
                                                        string d11 = "", Action a11 = null, Action u11 = null) {
        List<Process> list = new List<Process>();
        list.Add(ProcessFactory.createUndoableProcess(d0,  a0,  u0 ));
        list.Add(ProcessFactory.createUndoableProcess(d1,  a1,  u1 ));
        list.Add(ProcessFactory.createUndoableProcess(d2,  a2,  u2 ));
        list.Add(ProcessFactory.createUndoableProcess(d3,  a3,  u3 ));
        list.Add(ProcessFactory.createUndoableProcess(d4,  a4,  u4 ));
        list.Add(ProcessFactory.createUndoableProcess(d5,  a5,  u5 ));
        list.Add(ProcessFactory.createUndoableProcess(d6,  a6,  u6 ));
        list.Add(ProcessFactory.createUndoableProcess(d7,  a7,  u7 ));
        list.Add(ProcessFactory.createUndoableProcess(d8,  a8,  u8 ));
        list.Add(ProcessFactory.createUndoableProcess(d9,  a9,  u9 ));
        list.Add(ProcessFactory.createUndoableProcess(d10, a10, u10));
        list.Add(ProcessFactory.createUndoableProcess(d11, a11, u11));
        return list;
    }
}
