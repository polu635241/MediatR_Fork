using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace Kun.Tool
{
    public class FileUtility
    {
        static FileUtility () 
        {
            var curProcess = System.Diagnostics.Process.GetCurrentProcess ();
            var currentAppPath = curProcess.MainModule.FileName;
            CurrentDriectory = Path.GetDirectoryName (currentAppPath);
        }

        /// <summary>
        /// 避免程序是被別人開啟而不是第一個進入點
        /// 不要使用Directory.GetCurrentDirectory
        /// </summary>
        public static string CurrentDriectory { get; private set; }
    }
}
