using System;
using System.Collections.Generic;

namespace ReturnOS.SystemCore;

public static class ProcMgr {
    public List<Process> procList;
}

public class Process {
    public Action<int> action; // runs every frame, if exit code not 0 then error is sent and proc is removed
    public Action initialize; // runs when added to proc list once

    public
}