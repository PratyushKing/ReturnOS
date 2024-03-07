using Cosmos.System;
using Cosmos.System.Graphics;
using CosmosTTF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnOS.Graphical.Deskgets
{
    public static class TopMenu
    {
        public static bool menuOpen = false;
        public static readonly List<Action<int, int>> topMenuLogoButtonContents = new List<Action<int, int>>()
        {
            DrawAboutButton,
            DrawSeparator1,
            DrawShutdownButton,
            DrawRestartButton,
            DrawSeparator2,
            DrawLogOutButton
        };

        public static void Draw(SVGAIICanvas canvas)
        {
            canvas.DrawFilledRectangle(Kernel.primaryPalette.GeneralSurfaceColor, 0, 0, 1368, 30);
            var textWidth = TTFManager.GetTTFWidth(DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "  FPS: " + Kernel.FPS, "main", 15);
            canvas.DrawStringTTF(Kernel.primaryPalette.Text, DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "  FPS: " + Kernel.FPS, "main", 15, new(Kernel.Width - textWidth - 5, 20));

            canvas.DrawFilledRectangle(Kernel.primaryPalette.GeneralSurfaceColor, 0, 0, 30, 30);
            canvas.DrawImageAlpha(Kernel.logo, 0, 0);
            canvas.DrawLine(Color.Gray, 35, 10, 35, 20);
            canvas.DrawStringTTF(Kernel.primaryPalette.Text, WindowManager.windowsList[0].title, "mainBold", 15, new(45, 20));

            if (menuOpen)
            {
                Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.GeneralSurfaceColor, 0, 30, 120, topMenuLogoButtonContents.Count * 20 + 10, 10);
                var x = 15;
                var y = 50;
                foreach (var content in topMenuLogoButtonContents)
                {
                    content(x, y);
                    y += 20;
                }
            }
        }

        public static void HandleMouseEvent(MouseMgr.MouseEvent mouseEvent)
        {
            // Check Click for top menu -> logo button
            if (mouseEvent.clicked == Cosmos.System.MouseState.Left &&
                mouseEvent.x > 0 && mouseEvent.x < 30 &&
                mouseEvent.y > 0 && mouseEvent.y < 30)
            {
                menuOpen = !menuOpen;
                MouseManager.MouseState = MouseState.None;
            }
        }

        public static void DrawAboutButton(int x, int y)
        {
            Kernel.canvas.DrawStringTTF(Kernel.primaryPalette.Text, "About", "main", 12, new(x, y));
        }

        public static void DrawSeparator1(int x, int y)
        {
            Kernel.canvas.DrawLine(Kernel.primaryPalette.SubText0, x, y - 5, x + 95, y - 5);
        }

        public static void DrawShutdownButton(int x, int y)
        {
            Kernel.canvas.DrawStringTTF(Kernel.primaryPalette.Text, "Shutdown", "main", 12, new(x, y));
        }

        public static void DrawRestartButton(int x, int y)
        {
            Kernel.canvas.DrawStringTTF(Kernel.primaryPalette.Text, "Restart", "main", 12, new(x, y));
        }

        public static void DrawSeparator2(int x, int y)
        {
            Kernel.canvas.DrawLine(Kernel.primaryPalette.SubText0, x, y - 5, x + 100, y - 5);
        }

        public static void DrawLogOutButton(int x, int y)
        {
            Kernel.canvas.DrawStringTTF(Kernel.primaryPalette.Text, "Log Out", "main", 12, new(x, y));
        }
    }
}
