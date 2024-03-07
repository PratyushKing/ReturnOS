using CosmosTTF;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            width = 500;
            height = 250;
            x = WindowManager.GetCurrentDefaultPosition(width, height).Item1;
            y = WindowManager.GetCurrentDefaultPosition(width, height).Item2;
            windowState = WindowState.Active;
            drawWindowControls = dWC;
        }

        public void dWC(WindowCanvas winCanvas)
        {
            winCanvas.DrawFilledRectangle(Kernel.primaryPalette.Crust, 0, 0, winCanvas.width - 2, 50);
            winCanvas.DrawString(Kernel.primaryPalette.Text, "Welcome to ReturnOS!", SystemFonts.General_Bold, 20, winCanvas.width / 2 - TTFManager.GetTTFWidth("Welcome to ReturnOS!", WindowManager.SystemFontsToString(SystemFonts.General_Bold), 20), 10);
            
            // OS Name ReturnOS
            winCanvas.DrawString(Kernel.primaryPalette.SubText0, "OS Name", SystemFonts.General, 12, 5, 55);
            winCanvas.DrawString(Kernel.primaryPalette.Sapphire, "ReturnOS", SystemFonts.General_Bold, 12, TTFManager.GetTTFWidth("OS Name__", WindowManager.SystemFontsToString(SystemFonts.General), 12), 55);

            // Memory <memory_available>
            winCanvas.DrawString(Kernel.primaryPalette.SubText0, "Memory", SystemFonts.General, 12, 6, 70);
            winCanvas.DrawString(Kernel.primaryPalette.Sapphire, Math.Ceiling((Cosmos.Core.CPU.GetAmountOfRAM() / 8.0) * 8.0).ToString() + " MB", SystemFonts.General_Bold, 12, TTFManager.GetTTFWidth("Memory__", WindowManager.SystemFontsToString(SystemFonts.General), 12), 70);

            // Version <current_version>
            winCanvas.DrawString(Kernel.primaryPalette.SubText0, "Version", SystemFonts.General, 12, 5, 85);
            winCanvas.DrawString(Kernel.primaryPalette.Sapphire, Kernel.Version, SystemFonts.General_Bold, 12, TTFManager.GetTTFWidth("Version__", WindowManager.SystemFontsToString(SystemFonts.General), 12), 85);

            winCanvas.DrawButton("Test", 80, 80, 40, 20, buttonTest);
        }

        public void register() => WindowManager.NewWindow(this);

        public void buttonTest(MouseMgr.MouseEvent mouseEvent, WindowCanvas winCanvas)
        {
        }
    }
}
