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

        public static void Draw(SVGAIICanvas canvas)
        {
            canvas.DrawFilledRectangle(Kernel.primaryPalette.GeneralSurfaceColor, 0, 0, 1368, 30);
            canvas.DrawStringTTF(Kernel.primaryPalette.Text, "O:" + (int)WindowManager.windowsList.Count, "main", 15, new(Kernel.Width - 100, 20));

            canvas.DrawFilledRectangle(Kernel.primaryPalette.Base, 0, 0, 30, 30);
        }

    }
}
