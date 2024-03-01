using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using ReturnOS.SystemCore;
using Sys = Cosmos.System;

namespace ReturnOS
{
    public class Kernel: Sys.Kernel
    {
        public static CosmosVFS fs;
        public static LVFS fallbackFS;
        public static bool isUsingLVFS = false;

        protected override void BeforeRun()
        {
            Console.WriteLine("Loading ReturnOS returnos");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[0.0001] checking audio");
            Console.Beep();
            Console.WriteLine("[0.0002] checking filesystem availability");
            var testfs = new CosmosVFS();
            testfs = null;
            Console.WriteLine("[0.0003] booting started");
            Console.ForegroundColor = ConsoleColor.White;
            try {
                fs = new();
                VFSManager.RegisterVFS(fs, true, false);
                ConsoleLib.WriteSystemInfo(Result.PASS, "Filesystem initialized");
                isUsingLVFS = false;
            } catch (Exception) {
                ConsoleLib.WriteSystemInfo(Result.FAIL, "Filesystem failed to initialize");
                ConsoleLib.WriteSystemInfo(Result.WARN, "Please note: the system is on fallback mode and will use a LVFS to load filesystem actions!");
                isUsingLVFS = true;
            }
            ConsoleLib.WriteSystemInfo(Result.OK, "[1/2] LVFS: Starting Fallback LVFS..");
            fallbackFS = new() {
                isRunningAsFallback = true,
                isVirtualizing = false,
                isHavingOSData = true,
                isRunningForProgram = false,
                
                LVFSName = "FallbackKernelLVFS",
                LVFSID = 0,
                Data = new Dictionary<string, string>() { { "/etc/lvfs", "true" } }
            };
            LVFSManager.
            ConsoleLib.WriteSystemInfo(Result.OK, "Starting system service manager.");
        }
        
        protected override void Run()
        {
        }
    }
}
