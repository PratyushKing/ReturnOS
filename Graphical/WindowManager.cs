using Cosmos.System.Graphics;
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
                Kernel.canvas.DrawFilledRectangle(Kernel.primaryPalette.Base, window.x, window.y, window.width, window.height);
                Kernel.canvas.DrawFilledRectangle(Kernel.primaryPalette.Inactive, window.x, window.y, window.width, 20);
                Kernel.canvas.DrawFilledCircle(Kernel.primaryPalette.Red, window.x + window.width, window.y + 10, 5);
            }

        activeWin:
            Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Mantle, activeWindow.x, activeWindow.y, activeWindow.width, activeWindow.height, 15, Kernel.primaryPalette.Blue);
            Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Crust, activeWindow.x, activeWindow.y, activeWindow.width, 20, 15, Kernel.primaryPalette.Blue);
            Kernel.canvas.DrawFilledCircle(Kernel.primaryPalette.Red, activeWindow.x + activeWindow.width - 10, activeWindow.y + 10, 5);
        }

        public static void Update()
        {
            DrawAll();
        }

        public static void NewWindow(Window window)
        {
            if (window.windowState == WindowState.Closed)
            {
                return;
            } else
            {
                window.id = GetCurrentID();
                windowsList.Add(window);
                activeWindow = window;
            }
        }

        public static void NewWindow(string TITLE, bool CLOSED, int X, int Y, int WIDTH, int HEIGHT, WindowState WINDOWSTATE, List<Widget> WIDGETS)
        {
            if (WINDOWSTATE == WindowState.Closed)
            {
                return;
            } else
            {
                var RecievedID = GetCurrentID();
                windowsList.Add(new()
                {
                    id = RecievedID,
                    title = TITLE,
                    closed = CLOSED,
                    x = X,
                    y = Y,
                    width = WIDTH,
                    height = HEIGHT,
                    windowState = WINDOWSTATE,
                    widgets = WIDGETS
                });
                activeWindow = new()
                {
                    id = RecievedID,
                    title = TITLE,
                    closed = CLOSED,
                    x = X,
                    y = Y,
                    width = WIDTH,
                    height = HEIGHT,
                    windowState = WINDOWSTATE,
                    widgets = WIDGETS
                };
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
        public List<Widget> widgets;
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
