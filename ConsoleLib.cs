using System;
using System.Collections.Generic;

namespace ReturnOS;

public class ConsoleLib {
    public static string lastCommand = "";
    public static readonly ConsoleColor[] ResultColor = { ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.DarkRed };
    public static readonly string[] ResultString = { "[  OK  ]", "[ FAIL ]", "[ PASS ]", "[ WARN ]", "[KPANIC]" };

    public static void WriteSystemInfo(Result result, string proc) {
        var currentConsoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ResultColor[(int)result];
        Console.Write(ResultString[(int)result] + " ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(proc);
        Console.ForegroundColor = currentConsoleColor;
        return;
    }

}

public enum Result {
    OK = 0,
    FAIL = 1,
    PASS = 2,
    WARN = 3,
    PANIC = 4
}