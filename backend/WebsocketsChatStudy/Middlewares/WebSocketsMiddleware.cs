
using System.Net.WebSockets;
using System.Text;
using WebSocketsChatStudy.Services.Interfaces;

namespace WebSocketsChatStudy.Middlewares;

public class WebSocketsMiddleware
{
    private readonly IWebSocketConnectionManager _webSocketConnectionManager;
    private readonly RequestDelegate _next;

    public WebSocketsMiddleware(RequestDelegate next, IWebSocketConnectionManager webSocketConnectionManager)
    {
        _next = next;
        _webSocketConnectionManager = webSocketConnectionManager;
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
            _webSocketConnectionManager.AddSocket(webSocket, 1);

            await HandleWebSocketConnection(webSocket, 1);
            return;
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

                await _webSocketConnectionManager.SendMessageAsync(userId, message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket error: {ex.Message}");
            await _webSocketConnectionManager.RemoveSocketAsync(userId, ex.Message, WebSocketCloseStatus.InternalServerError);
        }
        finally
        {
            await _webSocketConnectionManager.RemoveSocketAsync(userId, $"Connection closed with user id: {userId}", WebSocketCloseStatus.NormalClosure);
        }
    }
}
