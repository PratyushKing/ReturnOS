using Cosmos.System;
using Cosmos.System.Graphics;
using CosmosTTF;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReturnOS.Graphical
{

    public static class WindowManager
    {

        public static List<Window> windowsList = new();
        public static Window activeWindow = new();
        public static int currentID = -1;

        public static void DrawAll()
        {
            if (windowsList.Count == 0)
                return;
            else if (windowsList.Count == 1)
                goto activeWin;

            foreach (var window in windowsList)
            {
                if (activeWindow.id == window.id)
                {
                    continue;
                }
                Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Base, window.x, window.y, window.width, window.height, 15);
                Kernel.canvas.DrawStringTTF(Kernel.primaryPalette.Text, window.title, "mainBold", 12, new(window.x + 6, window.y + 15));
                Kernel.canvas.DrawLine(Kernel.primaryPalette.SubText0, window.x + 10, window.y + 25, window.x + window.width - 10, window.y + 25);
                //Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Inactive, window.x, window.y, window.width, 20, 15);
                Kernel.canvas.DrawFilledCircle(Kernel.primaryPalette.Red, window.x + window.width - 15, window.y + 10, 5);
            }

        activeWin:
            Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Mantle, activeWindow.x, activeWindow.y, activeWindow.width, activeWindow.height, 15);
            Kernel.canvas.DrawStringTTF(Kernel.primaryPalette.Text, activeWindow.title, "mainBold", 14, new(activeWindow.x + 10, activeWindow.y + 20));
            Kernel.canvas.DrawLine(Kernel.primaryPalette.SubText0, activeWindow.x + 10, activeWindow.y + 25, activeWindow.x + activeWindow.width - 10, activeWindow.y + 25);
            //Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Blue, activeWindow.x + 2, activeWindow.y + 2, activeWindow.width - 8, 15, 8);
            Kernel.canvas.DrawFilledCircle(Kernel.primaryPalette.Red, activeWindow.x + activeWindow.width - 15, activeWindow.y + 15, 5);
        }

        public static void Update()
        {
            DrawAll();
        }

        public static void SendMouseEventToActiveWindow(MouseMgr.MouseEvent mouseEvent) => activeWindow.TriggerMouseEvent(mouseEvent);

        public static void NewWindow(Window window)
        {
            if (window.windowState == WindowState.Closed)
            {
                return;
            } else
            {
                window.id = GetCurrentID();
                window.Init();
                windowsList.Add(window);
                activeWindow = window;
            }
        }

        public static void NewWindow(string TITLE, bool CLOSED, int X, int Y, int WIDTH, int HEIGHT, WindowState WINDOWSTATE, Action WIDGETS)
        {
            if (WINDOWSTATE == WindowState.Closed)
            {
                return;
            } else
            {
                var window = new Window()
                {
                    id = GetCurrentID(),
                    title = TITLE,
                    closed = CLOSED,
                    x = X,
                    y = Y,
                    width = WIDTH,
                    height = HEIGHT,
                    windowState = WINDOWSTATE,
                    drawWindowControls = WIDGETS
                };
                window.Init();
                windowsList.Add(window);
                activeWindow = window;
            }
        }

        public static int GetCurrentID()
        {
            currentID++;
            return currentID;
        }
    }

    public class Window
    {
        public int id;
        public string title;
        public bool closed;
        public int x, y, width, height;
        public WindowState windowState = WindowState.Open;
        public Action drawWindowControls;

        private int dragX, dragY;

        public void Init()
        {
            dragX = (int)MouseManager.X - x; dragY = (int)MouseManager.Y - y;
        }

        public void TriggerMouseEvent(MouseMgr.MouseEvent mouseEvent)
        {
            Kernel.canvas.DrawFilledRectangle(Color.White, dragX, dragY, width, height);
            if (mouseEvent.x > dragX && mouseEvent.x < width &&
                mouseEvent.y > dragY && mouseEvent.y < 25 &&
                mouseEvent.clicked == Cosmos.System.MouseState.Left)
            {
                x = mouseEvent.x - dragX;
                y = mouseEvent.y - dragY;
            }
        }
    }

    public struct Widget
    {
        public string name;
        public int x, y, width, height;
    }

    public enum WindowState
    {
        Active,
        Open,
        Minimized,
        Closed
    }
}
