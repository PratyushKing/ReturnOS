using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using ReturnOS.SystemCore;
using Sys = Cosmos.System;

namespace ReturnOS
{
    public class Kernel: Sys.Kernel
    {
        public static CosmosVFS fs = new();
        public static LVFSHandler lvfsHandler;
        public static LVFS fallbackFS;
        public static bool isUsingLVFS = false;
        public static string kernelCurrentDir;
        public static bool bootIntoSetup;
        public static SVGAIICanvas canvas;

        public static Dictionary<string, string> variables = new Dictionary<string, string>() {
            { "SHELL", "[$USER@$HOSTNAME] ($CDIR) $USERSIGN" },
            { "LOGNAME", "" },
            { "HOSTNAME", "ReturnOS" },
            { "$CDIR", kernelCurrentDir }
        };

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("Loading ReturnOS returnos");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[0.0001] checking audio");
            Console.Beep();
            // Console.WriteLine("[0.0002] checking filesystem availability");
            // var testfs = new CosmosVFS();
            // testfs = null;
            Console.WriteLine("[0.0003] booting started");
            Console.ForegroundColor = ConsoleColor.White;
            try {
                VFSManager.RegisterVFS(fs, true, false);
                ConsoleLib.WriteSystemInfo(Result.PASS, "Filesystem initialized");
                isUsingLVFS = false;
            } catch (Exception) {
                ConsoleLib.WriteSystemInfo(Result.FAIL, "Filesystem failed to initialize");
                ConsoleLib.WriteSystemInfo(Result.WARN, "Please note: the system is on fallback mode and will use a LVFS to load filesystem actions!");
                isUsingLVFS = true;
            }
            if (isUsingLVFS) {
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
                ConsoleLib.WriteSystemInfo(Result.OK, "[1/2] LVFS: Registering Fallback FS and binding to Kernel");
                lvfsHandler = LVFSManager.RegisterLVFS(fallbackFS);
            }
            ConsoleLib.WriteSystemInfo(Result.OK, "Starting system service manager.");
            ProcMgr.StartProcManager();
            ConsoleLib.WriteSystemInfo(Result.OK, "Checking for current installation");
            if (!Directory.Exists("0:\\etc")) {
                ConsoleLib.WriteSystemInfo(Result.OK, "Launching ReturnOS Setup GUI");
                bootIntoSetup = true;
            } else {
                bootIntoSetup = false;
            }
            ConsoleLib.WriteSystemInfo(Result.OK, "Starting GUI Canvas");
            canvas = new(new(1024, 768, ColorDepth.ColorDepth32));
            Sys.MouseManager.ScreenWidth = 1024;
            Sys.MouseManager.ScreenHeight = 768;
            Sys.MouseManager.X = 1024 / 2;
            Sys.MouseManager.Y = 768 / 2;
        }
        
        protected override void Run()
        {
            canvas.Clear(Color.DarkViolet);
            if (bootIntoSetup)
                SetupMgr.Setup(isUsingLVFS);

            canvas.Display();
        }

    }
}
