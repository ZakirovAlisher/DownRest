using Microsoft.Owin;
using Owin;
using DownRest.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Telegram.Bot;
using Telegram.Bot.Args;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

[assembly: OwinStartup(typeof(DownRest.Startup))]

namespace DownRest
{
    public class Startup
    {
        private static ITelegramBotClient botClient;
        
                    private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var text = e?.Message?.Text;


            UserContext db = new UserContext();
            var blog = db.Projects.Where(b => b.Cat.Name.Equals(text));
            string res = "";
            foreach (Project p in blog) {
                if(p.AcceptedBy == 0)
                res = res + p.Name + " " + p.Reward + " " +  "https://localhost:44361/Home/DetailsProject/"+p.Id +  "\n";

            }



            if (!res.Equals(""))
            await botClient.SendTextMessageAsync(

           chatId: e.Message.Chat,
           text: res
           
           ).ConfigureAwait(false);
            else
                await botClient.SendTextMessageAsync(

           chatId: e.Message.Chat,
           text: "Nothing found"

           ).ConfigureAwait(false);

            botClient.SendStickerAsync(
                e.Message.Chat, "https://raw.githubusercontent.com/ZakirovAlisher/MidtermPHP/main/1-51125-128b.webp"
                );

        }

        public void Configuration(IAppBuilder app)
        {
            botClient = new TelegramBotClient("1440761266:AAH6sYq_5KUG_hpmdSoMoQFhDd_al0CnAXk") { Timeout = TimeSpan.FromSeconds(10) };
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            // настраиваем контекст и менеджер
            app.CreatePerOwinContext<UserContext>(UserContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}