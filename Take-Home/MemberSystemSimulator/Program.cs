using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kun.Tool;
using MemberSystemSimulator;
using MemberSystemSimulator.Action;
using System.Security.Principal;

namespace MediatRTest
{
    class Program
    {
        static void Main (string[] args)
        {
            //初始化log導向
            LoggerRouter.BindLogEvent (Console.WriteLine);

            //初始化會員系統
            memberSystem = new MemberSystem ();
            memberSystem.Init ();

            //註冊指令對應的流程
            cmdTable.Add (("login", LoginFlow, "登入"));
            cmdTable.Add (("create", CreateFlow, "新增"));
            cmdTable.Add (("clear", ClearFlow, "清空帳號"));

            CmdFlow ();
        }

        static MemberSystem memberSystem;

        /// <summary>
        /// demo透過指令表控制流程
        /// desc為顯示的內容
        /// </summary>
        static List<(string key, Func<Task> flow, string desc)> cmdTable = new List<(string key, Func<Task> flow, string desc)> ();

        /// <summary>
        /// 等待輸入指令切換流程
        /// </summary>
        static async void CmdFlow ()
        {
            //從設定表中提取訊息印出
            var lines = cmdTable.ConvertAll (pair => $"{pair.key} -> {pair.desc}");

            Console.WriteLine ("指令表");
            lines.ForEach (line =>
            {
                Console.WriteLine (line);
            });

            Console.WriteLine ("請輸入指令切換流程");

            var cmdMsg = Console.ReadLine ().ToLower ();

            var pairIndex = cmdTable.FindIndex (pair => pair.key.ToLower () == cmdMsg);

            //指令存在則透過指令切換流程
            if (pairIndex >= 0) 
            {
                var pair = cmdTable[pairIndex];
                Console.WriteLine ($"--------------> 進入 {pair.desc} 流程");
                await pair.flow.Invoke ();
            }
            else
            {
                Console.WriteLine ($"不存在的指令 -> {cmdMsg}, 請重新輸入");

                CmdFlow ();
            }
        }

        static async Task LoginFlow () 
        {
            var account = InputFlow ("帳號").Result;
            var password = InputFlow ("密碼").Result;

            var res = await memberSystem.ProcessLogin (account, password);

            var msg = "";

            switch (res)
            {
                case ProcessMemberRes.processSuccess:
                    {
                        //demo沒有登出流程接著登入
                        msg = $"登入成功, 沒有登出流程 接著回到輸入指令";
                        break;
                    }

                case ProcessMemberRes.accountNoFullSame:
                    {
                        msg = "帳號或者密碼錯誤";
                        break;
                    }

                //每個流程有其對應的res
                //出現預期之外的res要跳警告
                default:
                    {
                        msg = $"{nameof (LoginFlow)} -> {res}, 非預期的流程回傳值";
                        break;
                    }
            }

            Console.WriteLine (msg);

            //分隔流程訊息
            Console.WriteLine ("");
            Console.WriteLine ("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine ("");

            //重覆回到指令流程
            CmdFlow ();
        }

        static async Task ClearFlow ()
        {
            var res = await memberSystem.ProcessClear ();

            var msg = res ? "清空帳號成功" : "清空帳號失敗";

            Console.WriteLine (msg);

            //分隔流程訊息
            Console.WriteLine ("");
            Console.WriteLine ("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine ("");

            //重覆回到指令流程
            CmdFlow ();
        }

        static async Task CreateFlow ()
        {
            var account = InputFlow ("帳號").Result;
            var password = InputFlow ("密碼").Result;

            var res = await memberSystem.ProcessCreate (account, password);

            var msg = "";

            switch (res)
            {
                case ProcessMemberRes.processSuccess: 
                    {
                        msg = "創建成功";
                        break;
                    }

                case ProcessMemberRes.accountExist:
                    {
                        msg = "帳號已經存在, 無法新增";
                        break;
                    }

                    //每個流程有其對應的res
                    //出現預期之外的res要跳警告
                default: 
                    {
                        msg = $"{nameof (CreateFlow)} -> {res}, 非預期的流程回傳值";
                        break;
                    }
            }

            Console.WriteLine (msg);

            //分隔流程訊息
            Console.WriteLine ("");
            Console.WriteLine ("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine ("");

            //重覆回到指令流程
            CmdFlow ();
        }

        /// <summary>
        /// 重覆流程直到輸入沒有不合法的內容
        /// ex:空字串
        /// </summary>
        /// <returns></returns>
        static Task<string> InputFlow (string inputMsg) 
        {
            string input = "";

            while (true) 
            {
                Console.WriteLine ($"請輸入 -> {inputMsg}");
                input = Console.ReadLine ();

                if (string.IsNullOrEmpty (input)) 
                {
                    Console.WriteLine ("請勿輸入空字串");
                    continue;
                }

                break;
            }

            return Task.FromResult (input);
        }

        public class MessageHandle : IMessage 
        {
            public string Message { get; set; }
        }

        public interface IMessage 
        {
            public string Message { get; set; }
        }

        public record SomeEvent (string message) : INotification;

        public class Ping : IRequest<int>
        {

        }

        public class PingHandler : IRequestHandler<Ping, int>
        {
            public PingHandler ()
            {

            }

            Task<int> IRequestHandler<Ping, int>.Handle (Ping request, CancellationToken cancellationToken)
            {
                return Task.FromResult (1);
            }
        }

        public class Handler1 : INotificationHandler<SomeEvent>
        {
            public Handler1 (IMessage message)
            {
                this.message = message;
            }

            IMessage message;

            Task INotificationHandler<SomeEvent>.Handle (SomeEvent notification, CancellationToken cancellationToken)
            {
                LoggerRouter.WriteLine ($"Handled 1 : {notification.message}, {message.Message}");

                return Task.CompletedTask;
            }
        }
        public class Handler2 : INotificationHandler<SomeEvent>
        {
            Task INotificationHandler<SomeEvent>.Handle (SomeEvent notification, CancellationToken cancellationToken)
            {
                LoggerRouter.WriteLine ($"Handled 2: {notification.message}");

                return Task.CompletedTask;
            }
        }
    }
}