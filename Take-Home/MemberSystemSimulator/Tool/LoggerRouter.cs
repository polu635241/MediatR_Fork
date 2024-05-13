using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace Kun.Tool
{
    /// <summary>
    /// 統一的log輸出導向
    /// 可透過事件廣播出去
    /// </summary>
    public static class LoggerRouter
    {
        static LoggerRouter ()
        {
            LogFolderPath = Path.Combine (FileUtility.CurrentDriectory, LogFolderName);

            if (Directory.Exists (LogFolderPath) == false)
            {
                Directory.CreateDirectory (LogFolderPath);
            }

            logLocker = new ReaderWriterLockSlim ();
            logBuilder = new StringBuilder ();
            OnWriteLine = (msg) =>
            {
                logBuilder.AppendLine (msg);
            };
        }

        /// <summary>
        /// Log資料夾名稱
        /// </summary>
        const string LogFolderName = "Logs";

        /// <summary>
        /// 所有專案統一存放log的資料夾
        /// </summary>
        public static string LogFolderPath { get; private set; }

        /// <summary>
        /// 截到分就好, 顯示當地時間比較好當地使用者閱讀檢視
        /// </summary>
        public const string TimeFormatMinute = "yyyy-MM-dd-HH-mm";

        /// <summary>
        /// 截到秒就好
        /// </summary>
        public const string TimeFormatSecond = "yyyy-MM-dd-HH-mm-ss";

        /// <summary>
        /// 截到天
        /// </summary>
		public const string DayFormat = "yyyy-MM-dd";

        static ReaderWriterLockSlim logLocker;

        /// <summary>
        /// 註冊log事件,
        /// 可自行將log導向頁面或是Console上
        /// </summary>
        /// <param name="callback"></param>
        public static void BindLogEvent (Action<string> callback)
        {
            logLocker.EnterWriteLock ();

            var history = logBuilder.ToString ();
            if (string.IsNullOrEmpty (history) == false)
            {
                //每次AppednLine都會移到前面, 所以要把最後的\r\n消掉
                var processHistory = new char[history.Length - 2];
                history.CopyTo (0, processHistory, 0, processHistory.Length);

                callback.Invoke (history);
            }

            OnWriteLine += callback;
            logLocker.ExitWriteLock ();
        }

        static StringBuilder logBuilder;

        static event Action<string> OnWriteLine;

        /// <summary>
        /// 統一的log輸出導向
        /// 可透過事件廣播出去
        /// </summary>
        /// <param name="lineObj"></param>
        public static void WriteLine (object lineObj)
        {
            if (lineObj == null)
            {
                WriteLine ("不該寫入空");
                return;
            }

            string line = lineObj != null ? lineObj.ToString () : "";

            logLocker.EnterWriteLock ();
            logBuilder.AppendLine (line);
            OnWriteLine.Invoke (line);
            logLocker.ExitWriteLock ();
        }
    }
}
