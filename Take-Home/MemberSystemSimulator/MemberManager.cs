using MemberSystemSimulator.Model;
using MemberSystemSimulator.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberSystemSimulator
{
    internal class MemberManager : IMemberManager
    {
        //仿資料庫的本地緩存
        List<Member> members => FileRouter<Member>.caches;

        /// <summary>
        /// 檢查此帳號是否存在
        /// 用以創帳號的時候
        /// 檢查帳號是否已經存在
        /// </summary>
        /// <returns></returns>
        ProcessMemberRes IMemberManager.CheckAddAccount (string account, string password)
        {
            //檢查是否存在相同名稱的帳號
            if (members.Exists (m => m.account == account) == false) 
            {
                var newMember = new Member (account, password);
                FileRouter<Member>.Add (newMember);

                //成功並寫入實體資料
                return ProcessMemberRes.processSuccess;
            }
            else
            {
                //帳號重名了回傳失敗碼
                return ProcessMemberRes.accountExist;   
            }
        }

        /// <summary>
        /// 檢查帳號是否通過
        /// 用以登入時確認,
        /// 帳號與密碼都會確認
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ProcessMemberRes IMemberManager.CheckAccountLogin (string account, string password) 
        {
            //檢查是否有完全符合的檔案
            bool exist = members.Exists (m => m.account == account && m.password == password);

            if (exist) 
            {
                return ProcessMemberRes.processSuccess;
            }
            else
            {
                return ProcessMemberRes.accountNoFullSame;
            }
        }

        bool IMemberManager.ClearAccount ()
        {
            return FileRouter<Member>.DeleteAll ();
        }
    }

    internal interface IMemberManager 
    {
        /// <summary>
        /// 檢查此帳號是否存在
        /// 用以創帳號的時候
        /// 檢查帳號是否已經存在
        /// </summary>
        /// <returns></returns>
        ProcessMemberRes CheckAddAccount (string account, string password);

        /// <summary>
        /// 檢查帳號是否通過
        /// 用以登入時確認,
        /// 帳號與密碼都會確認
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ProcessMemberRes CheckAccountLogin (string account, string password);

        /// <summary>
        /// 清空所有註冊的帳號
        /// </summary>
        /// <returns></returns>
        bool ClearAccount ();
    }

    /// <summary>
    /// 處裡完的結果
    /// </summary>
    public enum ProcessMemberRes
    {
        processSuccess,
        /// <summary>
        /// 帳號已經存在
        /// </summary>
        accountExist,
        /// <summary>
        /// 帳號沒有完全相同
        /// 訊息應被包裝成錯誤的帳號或密碼
        /// </summary>
        accountNoFullSame,
    }
}
