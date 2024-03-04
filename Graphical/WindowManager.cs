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

        public static List<Window> openedWindows;
        public static List<Window> minimizedWindows;
        public static Window activeWindow;

        public static void DrawAll()
        {
            if (openedWindows.Count == 0)
                return;
            else if (openedWindows.Count == 1)
                goto activeWin;

            foreach (var window in openedWindows)
            {
                Kernel.canvas.DrawFilledRectangle(Color.FromArgb(23, 23, 23), window.x, window.y, window.width, window.height);
                Kernel.canvas.DrawFilledRectangle(Color.DarkGray, window.x, window.y, window.width, 20);
                Kernel.canvas.DrawFilledCircle(Color.Red, window.x + window.width - 10, window.y + 10, 10);
            }

        activeWin:
            Kernel.canvas.DrawFilledRectangle(Color.FromArgb(23, 23, 23), activeWindow.x, activeWindow.y, activeWindow.width, activeWindow.height);
            Kernel.canvas.DrawFilledRectangle(Color.FromArgb(50, 50, 50), activeWindow.x, activeWindow.y, activeWindow.width, 20);
        }

        public static void Update()
        {
            // CheckAllState();
            DrawAll();
        }

        public static void CheckAllState()
        {
            foreach (var window in openedWindows)
            {
                if (window.windowState == WindowState.Active)
                {
                    activeWindow = window;
                }
                else if (window.windowState == WindowState.Minimized)
                {
                    openedWindows.Remove(window);
                    minimizedWindows.Add(window);
                }
                else if (window.windowState == WindowState.Closed)
                {
                    openedWindows.Remove(window);
                }
            }

            foreach (var window in minimizedWindows)
            {
                if (window.windowState == WindowState.Active)
                {
                    activeWindow = window;
                }
                else if (window.windowState == WindowState.Open)
                {
                    openedWindows.Add(window);
                    minimizedWindows.Remove(window);
                }
                else if (window.windowState == WindowState.Closed)
                {
                    minimizedWindows.Remove(window);
                }
            }
        }

        public static void NewWindow(Window window)
        {
            if (window.windowState == WindowState.Open)
            {
                openedWindows.Add(window);
            }
            else if (window.windowState == WindowState.Active)
            {
                activeWindow = window;
            }
            else if (window.windowState == WindowState.Minimized)
            {
                minimizedWindows.Add(window);
            }
            else if (window.windowState == WindowState.Closed)
            {
                return;
            }
        }

        public static void NewWindow(int ID, string TITLE, bool CLOSED, int X, int Y, int WIDTH, int HEIGHT, WindowState WINDOWSTATE, List<Widget> WIDGETS)
        {
            if (WINDOWSTATE == WindowState.Open)
            {
                openedWindows.Add(new()
                {
                    id = ID,
                    title = TITLE,
                    closed = CLOSED,
                    x = X,
                    y = Y,
                    width = WIDTH,
                    height = HEIGHT,
                    windowState = WINDOWSTATE,
                    widgets = WIDGETS
                });
            }
            else if (WINDOWSTATE == WindowState.Active)
            {
                activeWindow = new()
                {
                    id = ID,
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
            else if (WINDOWSTATE == WindowState.Minimized)
            {
                minimizedWindows.Add(new()
                {
                    id = ID,
                    title = TITLE,
                    closed = CLOSED,
                    x = X,
                    y = Y,
                    width = WIDTH,
                    height = HEIGHT,
                    windowState = WINDOWSTATE,
                    widgets = WIDGETS
                });
            }
            else if (WINDOWSTATE == WindowState.Closed)
            {
                return;
            }

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
