using EventEase.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace EventEase.Services
{
    public interface IUserSessionService
    {
        Task SaveUserSessionAsync(User user);
        Task<User> GetCurrentUserAsync();
        Task ClearUserSessionAsync();
        Task<bool> IsUserLoggedInAsync();
    }

    public class UserSessionService : IUserSessionService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string UserSessionKey = "currentUser";

        public UserSessionService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveUserSessionAsync(User user)
        {
            var serializedUser = JsonSerializer.Serialize(user);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserSessionKey, serializedUser);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserSessionKey);
            if (string.IsNullOrEmpty(json))
                return null;
                
            return JsonSerializer.Deserialize<User>(json);
        }

        public async Task ClearUserSessionAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserSessionKey);
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            var user = await GetCurrentUserAsync();
            return user != null;
        }
    }
}