using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberSystemSimulator.Model
{
    [Serializable]
    /// <summary>
    /// 會員登入資訊
    /// demo用途所以資料是明碼
    /// </summary>
    public class Member
    {
        /// <summary>
        /// 留著無參數的版本讓序列化器可以透過反射生成
        /// </summary>
        public Member () 
        {

        }

        public Member (string account, string password)
        {
            this.account = account;
            this.password = password;
        }

        /// <summary>
        /// 帳號
        /// </summary>
        public string account;

        /// <summary>
        /// 密碼
        /// </summary>
        public string password;
    }
}
