using System;
using Newtonsoft;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kun.Tool;
using Newtonsoft.Json;

namespace MemberSystemSimulator.Tool
{
    /// <summary>
    /// 集合資料的緩存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class FileRouter<T>
    {
        /// <summary>
        /// 靜態建構式確保會被觸發
        /// </summary>
        static FileRouter ()
        {
            FolderPath = Path.Combine (FileUtility.CurrentDriectory, "Caches");

            //用typeName當作不同泛型緩存
            FilePath = Path.Combine (FolderPath, $"{typeof (T).Name}.dat");

            EnsureFilePath ();

            caches = new List<T> ();

            //初始化的時候聽緩存的
            //程序啟動後如果有人手動刪除檔案會被運行時的緩存覆蓋設定檔
            caches = File.ReadAllLines (FilePath).ToList ().ConvertAll (line => JsonConvert.DeserializeObject<T> (line));
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Delete (T data)
        {
            bool result = caches.Remove (data);

            if (result) 
            {
                FlushToFile ();
            }

            return result;
        }

        /// <summary>
        /// 清空所有資料
        /// 目前是本地資料一定會成功
        /// </summary>
        public static bool DeleteAll () 
        {
            caches.Clear ();

            FlushToFile ();

            return true;
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="data"></param>
        public static void Add (T data)
        {
            caches.Add (data);
            FlushToFile ();
        }

        /// <summary>
        /// 把記憶體的資料保存成實體的文本資料
        /// </summary>
        static void FlushToFile () 
        {
            EnsureFilePath ();

            using (StreamWriter writer = new StreamWriter (FilePath, false))
            {
                foreach (var cache in caches)
                {
                    var lien = JsonConvert.SerializeObject (cache);
                    writer.WriteLine (lien);
                }
            }
        }

        /// <summary>
        /// 所在的資料夾位置
        /// </summary>
        readonly static string FolderPath;

        /// <summary>
        /// 檔案所在的路徑
        /// </summary>
        readonly static string FilePath;

        /// <summary>
        /// 可能會有人主動去刪除檔案以及資料夾
        /// 每次讀寫前都要確保一次路徑
        /// </summary>
        static void EnsureFilePath () 
        {
            if (Directory.Exists (FolderPath) == false) 
            {
                Directory.CreateDirectory (FolderPath);
            }

            if (File.Exists (FilePath) == false) 
            {
                //需要Dispose
                File.Create (FilePath).Dispose ();
            }
        }

        /// <summary>
        /// 所有資料的緩存
        /// </summary>
        public static List<T> caches { get; private set; } = new List<T> ();
    }
}
