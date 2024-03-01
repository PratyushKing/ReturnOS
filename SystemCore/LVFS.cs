using System;
using System.Collections.Generic;
using Cosmos.System.FileSystem.VFS;

namespace ReturnOS.SystemCore {

    public static class LVFSManager {
        public static List<LVFS> RegisteredLVFS = new();

        public static readonly Dictionary<string, string> OSData = new Dictionary<string, string>() {
            {"test", "test"}
        };

        public static void RegisterLVFS(LVFS lvfs) {

        }
    }

    public class LVFSHandler {
        public LVFS lvfs;

        /// <summary>
        /// Writes all text of a file in a path with data. Path is path+file+extension.
        /// </summary>
        public static void WriteAllText(string path, string data) {
            string[] cPath = path.Split('/');
            foreach (var cp in cPath) {
                
            }
        }
    }

    public class LVFS {
        public bool isRunningAsFallback;
        public bool isVirtualizing;
        public bool isRunningForProgram;
        
        public Dictionary<string, string> Data; // fileName, fileData. Will combine on top of OSData
        
        public string LVFSName;
        public int LVFSID;
        public bool isHavingOSData;

        public bool Registered;
    }

}