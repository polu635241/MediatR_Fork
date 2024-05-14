using MemberSystemSimulator.Tool;

namespace MemberSystemTest
{
    /// 每個測試會在各自的流程中操作檔案
    /// 為了保證正確性要依序執行確保檔案獨立性
    [Collection (nameof(DisableParallelizationCollection))]
    public class UnitTestFileRouter
    {
        [System.Serializable]
        class UnitTestData 
        {
            public string name;

            public UnitTestData (string name)
            {
                this.name = name;
            }
        }

        [Fact]
        public void TestAdd ()
        {
            try 
            {
                FileRouter<UnitTestData>.DeleteAll ();

                var newOne = new UnitTestData ("testAdd");

                FileRouter<UnitTestData>.Add (newOne);
            }
            catch (Exception e)
            {
                Assert.Fail (e.Message + e.StackTrace);
            }

            //如何長度為0就是新增失敗
            Assert.False (FileRouter<UnitTestData>.caches.Count == 0, $"新增失敗");
        }

        [Fact]
        public void TestDelete ()
        {
            int oldCount = 0;
            int newCount = 0;

            try
            {
                var newOne = new UnitTestData ("testDelete");

                FileRouter<UnitTestData>.Add (newOne);

                //比對刪除前後數字確定真的刪除了
                oldCount = FileRouter<UnitTestData>.caches.Count;

                FileRouter<UnitTestData>.Delete (newOne);

                newCount = FileRouter<UnitTestData>.caches.Count;
            }
            catch (Exception e)
            {
                Assert.Fail (e.Message + e.StackTrace);
            }

            //刪除完要剛好少1
            Assert.False (newCount != oldCount - 1, "刪除測試失敗");
        }

        [Fact]
        public void TestClear()
        {
            try
            {
                var newOne = new UnitTestData ("testClear");

                FileRouter<UnitTestData>.Add (newOne);

                Assert.False (FileRouter<UnitTestData>.caches.Count == 0, $"新增失敗 {FileRouter<UnitTestData>.caches.Count}");

                FileRouter<UnitTestData>.DeleteAll ();
            }
            catch (Exception e)
            {
                Assert.Fail (e.Message + e.StackTrace);
            }

            //如果長度為0就是新增失敗
            Assert.False (FileRouter<UnitTestData>.caches.Count != 0, $"刪除失敗 {FileRouter<UnitTestData>.caches.Count}");
        }
    }
}