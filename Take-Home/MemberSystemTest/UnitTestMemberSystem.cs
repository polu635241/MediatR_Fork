using MediatR;
using MemberSystemSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberSystemTest
{
    /// 每個測試會在各自的流程中操作檔案
    /// 為了保證正確性要依序執行確保檔案獨立性
    [Collection (nameof(DisableParallelizationCollection))]
    public class UnitTestMemberSystem
    {
        [Fact]
        public async Task TestCreateAndLogin () 
        {
            MemberSystem memberSystem = new MemberSystem ();
            memberSystem.Init ();

            //先清空保證環境乾淨
            var deleteResult = await memberSystem.ProcessClear ();

            if (deleteResult == false)
            {
                Assert.Fail ("清除帳號失敗");
            }

            string mockAccount = "123456";
            string mockPassword = "123456";

            //嘗試新增帳號
            var createRes = await memberSystem.ProcessCreate (mockAccount, mockPassword);

            if (createRes != ProcessMemberRes.processSuccess) 
            {
                Assert.Fail ("新增帳號失敗");
            }

            //如果同一個組帳號可以用來登入就表示創建成功
            var loginRes = await memberSystem.ProcessLogin (mockAccount, mockPassword);

            if (loginRes != ProcessMemberRes.processSuccess) 
            {
                Assert.Fail ("登入失敗");
            }
        }

        [Fact]
        public async Task TestDelete ()
        {
            MemberSystem memberSystem = new MemberSystem ();
            memberSystem.Init ();

            //先清空保證環境乾淨
            var deleteResult = await memberSystem.ProcessClear ();

            if (deleteResult == false)
            {
                Assert.Fail ("清除帳號失敗");
            }

            string mockAccount = "123456";
            string mockPassword = "123456";

            //嘗試新增帳號
            var createRes = await memberSystem.ProcessCreate (mockAccount, mockPassword);

            if (createRes != ProcessMemberRes.processSuccess)
            {
                Assert.Fail ("新增帳號失敗");
            }

            //如果同一個組帳號可以用來登入就表示創建成功
            var clearResult = await memberSystem.ProcessClear ();

            if (clearResult == false)
            {
                Assert.Fail ("刪除失敗");
            }
        }
    }
}
