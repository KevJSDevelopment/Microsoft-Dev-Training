using Microsoft.AspNetCore.SignalR;
using Shared.Models;

namespace client.Services
{
    public class ChatService
    {
        private readonly HubConnection _hubconnection;
        public event Action<ChatMessage> OnMessageReceived;
        public ChatService()
        {
            _hubconnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chatHub")
                .WithAutomaticReconnect()
                .Build();

            _hubconnection.On<ChatMessage>("ReceiveMessage", (message) =>
            {
                OnMessageReceived?.Invoke(message);
            });

            _hubconnection.Closed += async (error) =>
            {
                Console.WriteLine("Connection closed. Attempting to reconnect...");
                await Task.Delay(5000);
                await _hubconnection.StartAsync();
            };
        }

        public async Task StartAsync()
        {
            await _hubconnection.StartAsync();
        }
        
    }
}