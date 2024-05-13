using MediatR;
using MemberSystemSimulator.Action;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MemberSystemSimulator
{
    public class MemberSystem
    {
        /// <summary>
        /// 初始化系統流程
        /// </summary>
        public void Init () 
        {
            IMemberManager memberManager = CreateMemberManager ();

            mediator = InitMediatorSystem (memberManager);
        }

        IMediator mediator;

        /// <summary>
        /// 初始化Mediator系統以及注入所需管理器
        /// </summary>
        /// <returns></returns>
        IMediator InitMediatorSystem (IMemberManager memberManager)
        {
            //初始化.net DI系統
            IServiceCollection services = new ServiceCollection ();

            //將簡易的會員管理器註冊進DI
            //讓handle可以存取
            services.AddSingleton (memberManager);

            //初始化MediatR DI系統
            MediatRServiceConfiguration mediatRServiceConfiguration = new MediatRServiceConfiguration ();
            //限定該project內的都掃描
            mediatRServiceConfiguration.RegisterServicesFromAssemblies (Assembly.GetExecutingAssembly ());
            services.AddMediatR (mediatRServiceConfiguration);
            IServiceProvider serviceProvider = services.BuildServiceProvider ();

            return serviceProvider.GetRequiredService<IMediator> ();
        }

        IMemberManager CreateMemberManager ()
        {
            return new MemberManager ();
        }

        public async Task<ProcessMemberRes> ProcessLogin (string account, string password)
        {
            LoginReq req = new LoginReq (account, password);

            //透過Request取得結果
            var res = await mediator.Send (req);

            return res;
        }

        public async Task<ProcessMemberRes> ProcessCreate (string account, string password)
        {
            CreateAccountReq req = new CreateAccountReq (account, password);

            //透過Request取得結果
            var res = await mediator.Send (req);

            return res;
        }

        public async Task<bool> ProcessClear ()
        {
            ClearReq req = new ClearReq ();

            //透過Request取得結果
            var res = await mediator.Send (req);

            return res;
        }
    }
}
