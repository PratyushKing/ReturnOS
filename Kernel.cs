using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using ReturnOS.Graphical;
using ReturnOS.Graphical.Deskgets;
using ReturnOS.SystemCore;
using s = System;
using Sys = Cosmos.System;
using CosmosTTF;
using Cosmos.Core.Memory;

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

        public const int Width = 1360;
        public const int Height = 768;

        [ManifestResourceStream(ResourceName = "ReturnOS.Resources.back.bmp")] public static byte[] background_raw;
        public static Bitmap background = new Bitmap(background_raw);

        [ManifestResourceStream(ResourceName = "ReturnOS.Resources.cursor.bmp")] public static byte[] cursor_raw;
        public static Bitmap cursor = new Bitmap(cursor_raw);
        [ManifestResourceStream(ResourceName = "ReturnOS.Resources.Comfortaa-Regular.ttf")] public static byte[] mainFont;
        [ManifestResourceStream(ResourceName = "ReturnOS.Resources.Comfortaa-Bold.ttf")] public static byte[] mainFont_Bold;

        [ManifestResourceStream(ResourceName = "ReturnOS.Resources.ReturnOSLogo.bmp")] public static byte[] logo_raw;
        public static Bitmap logo = new Bitmap(logo_raw);

        public static Dictionary<string, string> variables = new Dictionary<string, string>() {
            { "SHELL", "[$USER@$HOSTNAME] ($CDIR) $USERSIGN" },
            { "LOGNAME", "" },
            { "HOSTNAME", "ReturnOS" },
            { "$CDIR", kernelCurrentDir }
        };

        // Main catppuccin-based palette
        public static readonly ColorPalette primaryPalette = new()
        {
            RoseWater = Color.FromArgb(245, 224, 220),
            Flamingo = Color.FromArgb(242, 205, 205),
            Pink = Color.FromArgb(245, 194, 231),
            Mauve = Color.FromArgb(203, 166, 247),
            Red = Color.FromArgb(243, 139, 168),
            Maroon = Color.FromArgb(235, 160, 172),
            Peach = Color.FromArgb(250, 179, 135),
            Yellow = Color.FromArgb(249, 226, 175),
            Green = Color.FromArgb(166, 227, 161),
            Teal = Color.FromArgb(148, 226, 213),
            Sky = Color.FromArgb(137, 220, 235),
            Sapphire = Color.FromArgb(116, 199, 236),
            Blue = Color.FromArgb(137, 180, 250),
            Lavender = Color.FromArgb(180, 190, 254),
            Text = Color.FromArgb(205, 214, 244),
            Base = Color.FromArgb(30, 30, 46),
            GeneralSurfaceColor = Color.FromArgb(49, 50, 68),
            Mantle = Color.FromArgb(24, 24, 37),
            Inactive = Color.FromArgb(127, 132, 156),
            Crust = Color.FromArgb(17, 17, 27),
            SubText0 = Color.FromArgb(166, 173, 200)
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
            if (!File.Exists("0:\\etc\\installed")) {
                ConsoleLib.WriteSystemInfo(Result.OK, "Launching ReturnOS Setup GUI");
                bootIntoSetup = true;
            } else {
                bootIntoSetup = false;
            }
            ConsoleLib.WriteSystemInfo(Result.OK, "Starting GUI Canvas");
            canvas = new(new(1360, 768, ColorDepth.ColorDepth32));
            Sys.MouseManager.ScreenWidth = 1360;
            Sys.MouseManager.ScreenHeight = 768;
            Sys.MouseManager.X = 1360 / 2;
            Sys.MouseManager.Y = 768 / 2;
            ConsoleLib.WriteSystemInfo(Result.OK, "Loading fonts");
            TTFManager.RegisterFont("main", mainFont);
            TTFManager.RegisterFont("mainBold", mainFont_Bold);
        }

        public static int FPS = 0;

        public static int LastS = -1;
        public static int Ticken = 0;

        public static int useMode = 0;

        public static void Update()
        {
            if (LastS == -1)
            {
                LastS = s.DateTime.UtcNow.Second;
            }
            if (s.DateTime.UtcNow.Second - LastS != 0)
            {
                if (s.DateTime.UtcNow.Second > LastS)
                {
                    FPS = Ticken / (s.DateTime.UtcNow.Second - LastS);
                }
                LastS = s.DateTime.UtcNow.Second;
                Ticken = 0;
            }
            Ticken++;
        }

        public static int framesBeforeHeapCollect = 2;
        public static int framesNeededToHeapCollect = 0;

        protected override void Run()
        {
            if (framesNeededToHeapCollect == framesBeforeHeapCollect)
            {
                Heap.Collect();
                framesNeededToHeapCollect = 0;
            }
            framesNeededToHeapCollect++;
            Update();
            canvas.DrawImage(background, 0, 0);
            if (!bootIntoSetup)
                SetupMgr.Setup(isUsingLVFS);

            WindowManager.Update();
            TopMenu.Draw(canvas);
            MouseMgr.ShowAndManage();
            canvas.Display();
        }

        public struct ColorPalette
        {
            public Color RoseWater, Flamingo, Pink, Mauve, Red, Maroon, Peach, Yellow, Green, Teal, Sky, Sapphire, Blue, Lavender;
            public Color Text, GeneralSurfaceColor, Base, Mantle, Inactive, Crust, SubText0;
        }

        public static void DrawFilledRoundRectangle(Color backgroundColor, int x, int y, int width, int height, int cornerRadius)
        {
            canvas.DrawFilledRectangle(backgroundColor, x + cornerRadius, y, width - 2 * cornerRadius, height);
            canvas.DrawFilledRectangle(backgroundColor, x, y + cornerRadius, cornerRadius, height - 2 * cornerRadius);
            canvas.DrawFilledRectangle(backgroundColor, x + width - cornerRadius, y + cornerRadius, cornerRadius, height - 2 * cornerRadius);
            canvas.DrawFilledCircle(backgroundColor, x + cornerRadius, y + cornerRadius, cornerRadius);
            canvas.DrawFilledCircle(backgroundColor, x + width - cornerRadius - 1, y + cornerRadius, cornerRadius);
            canvas.DrawFilledCircle(backgroundColor, x + cornerRadius, y + height - cornerRadius - 1, cornerRadius);
            canvas.DrawFilledCircle(backgroundColor, x + width - cornerRadius - 1, y + height - cornerRadius - 1, cornerRadius);
        }
    }
}
