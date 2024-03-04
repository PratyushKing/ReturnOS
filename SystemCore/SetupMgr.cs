using System;
using System.Drawing;
namespace ReturnOS.SystemCore;

public static class SetupMgr {
    
    public static void Setup(bool fallbackMode) {
        while (true) {
            Kernel.canvas.Clear(Color.DarkViolet);
            Kernel.canvas.Display();
        }
    }

}