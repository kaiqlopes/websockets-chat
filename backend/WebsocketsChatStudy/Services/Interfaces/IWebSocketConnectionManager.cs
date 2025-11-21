using System.Net.WebSockets;

namespace WebSocketsChatStudy.Services.Interfaces;

public interface IWebSocketConnectionManager
{
    void AddSocket(WebSocket socket, long userId);
    Task RemoveSocketAsync(long userId, string reason, WebSocketCloseStatus closeStatus);
    WebSocket? GetByUserId(long userId);
    Task SendMessageAsync(long userId, string message);
}
