using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnOS.Graphical.Apps
{
    public class Welcome : Window
    {
        public Welcome()
        {
            id = WindowManager.GetCurrentID();
            title = "Welcome to ReturnOS";
            closed = false;
            x = WindowManager.GetCurrentDefaultPosition(width, height).Item1;
            y = WindowManager.GetCurrentDefaultPosition(width, height).Item2;
            width = 500;
            height = 250;
            windowState = WindowState.Active;
            drawWindowControls = dWC;
        }

        public void dWC(WindowCanvas winCanvas)
        {
            winCanvas.DrawFilledRectangle(Kernel.primaryPalette.Blue, 10, 10, 100, 50);
        }

        public void register() => WindowManager.NewWindow(this);
    }
}
