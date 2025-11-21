using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using WebSocketsChatStudy.Services.Interfaces;

namespace WebSocketsChatStudy.Services;

public class WebSocketConnectionManager : IWebSocketConnectionManager
{
    private static readonly ConcurrentDictionary<long, WebSocket> _connections = new();

    public void AddSocket(WebSocket socket, long userId)
    {
        _connections[userId] = socket;
    }

    public WebSocket? GetByUserId(long userId)
    {
        _connections.TryGetValue(userId, out var socket);
        return socket;
    }

    public async Task RemoveSocketAsync(long userId, string reason, WebSocketCloseStatus closeStatus)
    {
        _connections.TryRemove(userId, out WebSocket? socket);

        if (socket != null)
        {
            await socket.CloseAsync(closeStatus, reason, CancellationToken.None);
            socket.Dispose();
        }
    }

    public async Task SendMessageAsync(long userId, string message)
    {
        var socket = GetByUserId(userId);

        if (socket != null && socket.State == WebSocketState.Open)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(messageBytes);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
