﻿using Cosmos.System;
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
        public static bool isDragging = false;

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
                window.draw();
            }

        activeWin:
            activeWindow.draw();
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
                window.Init();
                windowsList.Add(window);
                activeWindow = window;
            }
        }

        public static void NewWindow(string TITLE, bool CLOSED, int X, int Y, int WIDTH, int HEIGHT, WindowState WINDOWSTATE, Action<WindowCanvas> WIDGETS)
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

        public static (int, int) GetCurrentDefaultPosition(int width, int height) { return (Kernel.Width / 2 - (width / 2), Kernel.Height / 2 - (height / 2)); }

        public static string SystemFontsToString(SystemFonts font)
        {
            switch (font)
            {
                case SystemFonts.General:
                    return "main";
                case SystemFonts.General_Bold:
                    return "mainBold";
                default:
                    return "";
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
        public Action<WindowCanvas> drawWindowControls;
        public WindowCanvas winCanvas;

        public int dragX, dragY;
        public bool dragging;

        public virtual void Init() {
            winCanvas = new WindowCanvas(this.x, this.y, this.width, this.height);
        }

        public void checkLogic()
        {
            //Kernel.canvas.DrawFilledRectangle(Color.White, dragX, dragY, width, height);
            if (dragging)
            {
                x = (int)MouseManager.X - dragX;
                y = (int)MouseManager.Y - dragY;
                if (x <= 0)
                {
                    x = 0;
                }
                if (y <= 30)
                {
                    y = 30;
                }
                if (x > Kernel.Width - width)
                {
                    x = Kernel.Width - width;
                }
                if (y > Kernel.Height - height)
                {
                    y = Kernel.Height - height;
                }
                winCanvas.x = x;
                winCanvas.y = y;
            }

            if ((int)MouseManager.X > x && (int)MouseManager.X < x + width
                && (int)MouseManager.Y > y && (int)MouseManager.Y < y + 25 &&
                MouseManager.MouseState == MouseState.Left && MouseManager.LastMouseState == MouseState.None
                && !dragging)
            {
                dragX = (int)MouseManager.X - x;
                dragY = (int)MouseManager.Y - y;
                dragging = true;
                WindowManager.isDragging = true;
            }

            if (dragging && MouseManager.MouseState != MouseState.Left) { dragging = false; WindowManager.isDragging = false; dragX = 0; dragY = 0; }
        }

        public void drawWindow()
        {
            Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Mantle, x, y, width, height, 15);
            Kernel.canvas.DrawStringTTF(Kernel.primaryPalette.Text, title, "mainBold", 14, new(x + 10, y + 20));
            Kernel.canvas.DrawLine(Kernel.primaryPalette.SubText0, x + 10, y + 25, x + width - 10, y + 25);
            //Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Blue, x + 2, y + 2, width - 8, 15, 8);
            Kernel.canvas.DrawFilledCircle(Kernel.primaryPalette.Red, x + width - 15, y + 15, 5);
        }

        public void draw()
        {
            checkLogic();
            drawWindow();
            drawWindowControls(winCanvas);
        }
    }

    public struct WindowCanvas
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public List<Action<MouseMgr.MouseEvent, WindowCanvas>> bBA; // binded Button Actions

        public void TriggerMouseEvent(MouseMgr.MouseEvent evt)
        {
            foreach (var button in bBA)
            {
                button(evt, this);
            }
        }

        public WindowCanvas(int x, int y, int width, int height)
        {
            bBA = new();
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        private bool CheckBounds(int x, int y, int w, int h)
        {
            if (x > this.x && y > this.y && x + w < this.width && y + h < this.height)
                return false;
            return true;
        }
        private bool CheckBounds(int x, int y)
        {
            if (x > this.x && y > this.y && x < this.width && y < this.height)
                return false;
            return true;
        }

        public void DrawPoint(Color color, int x, int y) { if (!CheckBounds(x + this.x + 2, y + this.y + 2)) { return; } Kernel.canvas.DrawPoint(color, x + this.x + 2, y + this.y + 27); }

        public void DrawFilledRectangle(Color color, int x, int y, int width, int height) { if (!CheckBounds(x + this.x + 2, y + this.y + 27, width, height)) { return; } Kernel.canvas.DrawFilledRectangle(color, x + this.x + 2, y + this.y + 27, width, height); }

        public void DrawString(Color color, string text, SystemFonts font, int size, int x, int y) { if (!CheckBounds(x + this.x + 2, y + this.y + 27 + size, TTFManager.GetTTFWidth(text, WindowManager.SystemFontsToString(font), size), size)) { return; } Kernel.canvas.DrawStringTTF(color, text, WindowManager.SystemFontsToString(font), size, new(x + this.x + 2, y + this.y + 27 + size)); }
    
        public void DrawButton(string text, int x, int y, int width, int height, Action<MouseMgr.MouseEvent, WindowCanvas> clicked) { if (!CheckBounds(x + this.x + 2, y + this.y + 27, width, height)) { return; } Kernel.DrawFilledRoundRectangle(Kernel.primaryPalette.Crust, x + this.x + 2, y + this.y + 27, width, height, height / 4); DrawString(Kernel.primaryPalette.Text, text, SystemFonts.General, 12, (this.x + x + 2 + width) / 2 - (TTFManager.GetTTFWidth(text, WindowManager.SystemFontsToString(SystemFonts.General), 12)), (this.y + y + 27) + (height / 2) - 6); bBA.Add(clicked); }
    }

    public struct Widget
    {
        public string name;
        public int x, y, width, height;
    }

    public enum SystemFonts
    {
        General, // Comfortaa
        General_Bold // Comfartaa Bold
    }

    public enum WindowState
    {
        Active,
        Open,
        Minimized,
        Closed
    }
}
