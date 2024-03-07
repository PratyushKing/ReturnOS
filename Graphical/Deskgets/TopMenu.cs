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
            var textWidth = TTFManager.GetTTFWidth(DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "  FPS: " + Kernel.FPS, "main", 15);
            canvas.DrawStringTTF(Kernel.primaryPalette.Text, DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "  FPS: " + Kernel.FPS, "main", 15, new(Kernel.Width - textWidth - 5, 20));

            canvas.DrawFilledRectangle(Kernel.primaryPalette.GeneralSurfaceColor, 0, 0, 30, 30);
            canvas.DrawImageAlpha(Kernel.logo, 0, 0);
            canvas.DrawLine(Color.Gray, 35, 10, 35, 20);
            canvas.DrawStringTTF(Kernel.primaryPalette.Text, WindowManager.windowsList[0].title, "mainBold", 15, new(45, 20));
        }

    }
}
