using Cosmos.System;
using Cosmos.System.Graphics;
using ReturnOS.Graphical.Deskgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnOS.Graphical
{
    public class MouseMgr
    {
        public struct MouseEvent
        {
            public MouseState clicked;
            public int x, y;
        }

        public static void ShowAndManage()
        {            
            if ((int)MouseManager.X + Kernel.cursor.Width >= Kernel.Width)
            {
                MouseManager.X = Kernel.Width - Kernel.cursor.Width;
            } else if ((int)MouseManager.Y + Kernel.cursor.Height >= Kernel.Height)
            {
                MouseManager.Y = Kernel.Height - Kernel.cursor.Height;
            }
            Kernel.canvas.DrawImageAlpha(Kernel.cursor, (int)Cosmos.System.MouseManager.X, (int)Cosmos.System.MouseManager.Y);
            if (MouseManager.MouseState != MouseState.None && !WindowManager.isDragging)
            {
                MouseEvent heldMouseEvent = new() { clicked = MouseManager.MouseState, x = (int)MouseManager.X, y = (int)MouseManager.Y };
                TopMenu.HandleMouseEvent(heldMouseEvent);
            }
        }
    }
}
