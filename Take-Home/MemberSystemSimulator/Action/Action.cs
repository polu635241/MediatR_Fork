using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberSystemSimulator.Action
{
    /// <summary>
    /// 創建帳號請求
    /// </summary>
    internal class CreateAccountReq : IRequest<ProcessMemberRes>
    {
        public CreateAccountReq (string account, string password = null)
        {
            this.account = account;
            this.password = password;
        }

        public string account;
        public string password;
    }

    /// <summary>
    /// 創建帳號Handle
    /// </summary>
    internal class CreateAccountReqHandle : IRequestHandler<CreateAccountReq, ProcessMemberRes>
    {
        public CreateAccountReqHandle (IMemberManager memberManager)
        {
            this.memberManager = memberManager;
        }

        IMemberManager memberManager;

        Task<ProcessMemberRes> IRequestHandler<CreateAccountReq, ProcessMemberRes>.Handle (CreateAccountReq request, CancellationToken cancellationToken)
        {
            var res = memberManager.CheckAddAccount (request.account, request.password);

            return Task.FromResult (res);
        }
    }

    /// <summary>
    /// 登入帳號請求
    /// </summary>
    internal class LoginReq  : IRequest<ProcessMemberRes>
    {
        public LoginReq (string account, string password = null)
        {
            this.account = account;
            this.password = password;
        }

        public string account;
        public string password;
    }

    /// <summary>
    /// 登入帳號Handle
    /// </summary>
    internal class LoginReqHandle : IRequestHandler<LoginReq, ProcessMemberRes>
    {
        public LoginReqHandle (IMemberManager memberManager)
        {
            this.memberManager = memberManager;
        }

        IMemberManager memberManager;

        Task<ProcessMemberRes> IRequestHandler<LoginReq, ProcessMemberRes>.Handle (LoginReq request, CancellationToken cancellationToken)
        {
            var res = memberManager.CheckAccountLogin (request.account, request.password);

            return Task.FromResult (res);
        }
    }

    /// <summary>
    /// 清空帳號請求
    /// </summary>
    internal class ClearReq : IRequest<bool>
    {

    }


    /// <summary>
    /// 清空帳號Handle
    /// </summary>
    internal class ClearReqHandle : IRequestHandler<ClearReq, bool>
    {
        public ClearReqHandle (IMemberManager memberManager)
        {
            this.memberManager = memberManager;
        }

        IMemberManager memberManager;

        Task<bool> IRequestHandler<ClearReq, bool>.Handle (ClearReq request, CancellationToken cancellationToken)
        {
            var res = memberManager.ClearAccount ();

            return Task.FromResult (res);
        }
    }
}
