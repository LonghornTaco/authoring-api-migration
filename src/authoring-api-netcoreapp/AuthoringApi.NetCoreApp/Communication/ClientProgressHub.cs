using Microsoft.AspNetCore.SignalR;

namespace AuthoringApi.NetCoreApp.Communication
{
    public class ClientProgressHub : Hub
    {
        public async Task SendUpdate(string message)
        {
            await Clients.All.SendAsync("ClientUpdate", message);
        }
    }
}
