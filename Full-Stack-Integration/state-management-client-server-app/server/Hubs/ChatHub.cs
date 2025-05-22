using Microsoft.AspNetCore.SignalR;
using Shared.Models;

namespace server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await Clients.All.SendAsync("ReceiveMessage", chatMessage);
        }
    }
}