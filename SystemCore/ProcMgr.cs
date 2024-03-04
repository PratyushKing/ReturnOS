using System;
using System.Collections.Generic;

namespace ReturnOS.SystemCore;

public static class ProcMgr {
    public static List<Process> procList;

    public static void StartProcManager() {
        
    }
}

public class Process {
    public Action<int> action; // runs every frame, if exit code not 0 then error is sent and proc is removed
    public Action initialize; // runs when added to proc list once

    public Process(Action init, Action<int> eachFrameRun) {
        this.action = eachFrameRun;
        this.initialize = init;
    }
}
