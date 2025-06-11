
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketsChatStudy.Middlewares;

public class WebSocketsMiddleware
{
    private static readonly ConcurrentDictionary<long, WebSocket> _connections = new();

    private readonly RequestDelegate _next;

    public WebSocketsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws")
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            /*long userId = Convert.ToInt64(context.Response.Headers["UserId"]);
            if (userId == 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("UserId is required");
                return;
            }*/

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _connections[1] = webSocket;

            await HandleWebSocketConnection(webSocket, 1);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task HandleWebSocketConnection(WebSocket webSocket, long userId)
    {
        var buffer = new byte[1024 * 4];

        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message from user {userId}: {message}");
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket error: {ex.Message}");
        }
        finally
        {
            _connections.TryRemove(userId, out _);

            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, $"Connection closed with user id: {userId}", CancellationToken.None);

            webSocket.Dispose();
        }
    }
}
