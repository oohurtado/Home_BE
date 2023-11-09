using Home.Source.Data.Infrastructure;
using Home.Source.Models.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace Home.Source.Hubs
{
    // https://dev.to/isaacojeda/aspnet-core-creando-un-chat-con-signalr-y-angular-23ig
    // https://www.youtube.com/watch?v=mrCxfifTepU&t=1037s
    // https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/signalr/authn-and-authz.md
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SignalR_Test1_Hub : Hub
    {
        private readonly IAspNetRepository aspNetRepository;

        public SignalR_Test1_Hub(IAspNetRepository aspNetRepository)
        {
            this.aspNetRepository = aspNetRepository;
        }

        public async Task MessageReceived(Message message)
        {
            var val = Context.UserIdentifier;
            if (message.Who == "everyone")
            {
                await Clients.All.SendAsync("MessageReceived", message);
            }
            else if (message.Who == "user" || message.Who == "admin")
            {
                var users = await aspNetRepository.GetUsersAsync(message.Who);
                var userIds = users.Select(p => p.Id).ToList();
                await Clients.Users(userIds).SendAsync("MessageReceived", message);
            }
        }
    }
}
