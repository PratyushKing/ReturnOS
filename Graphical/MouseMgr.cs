using Cosmos.System;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnOS.Graphical
{
    internal class MouseMgr
    {
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
        }
    }
}
