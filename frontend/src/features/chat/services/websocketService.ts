import { Chat } from "../pages/chat.js";

export class WebsocketService {
    private static readonly WEBSOCKET_URL = "wss://localhost:7230/ws";
    private static connection: WebSocket | null = null;

    public static startConnection(): void {
        if (WebsocketService.connection?.readyState === WebSocket.OPEN) {
            console.log('WebSocket is connected');
            return;
        }

        WebsocketService.connection = new WebSocket(WebsocketService.WEBSOCKET_URL);

        WebsocketService.connection.addEventListener('open', () => {
            console.log('WebSocket connection established');
        });

        WebsocketService.connection.addEventListener('message', (event) => {
            Chat.receiveMessage(event.data);
        });

        WebsocketService.connection.addEventListener('close', () => {
            console.log('WebSocket connection closed');
        });

        WebsocketService.connection.addEventListener('error', (error) => {
            console.error('WebSocket error:', error);
        });
    }

    public static sendMessage(message: string): void {
        if (!WebsocketService.connection || WebsocketService.connection.readyState !== WebSocket.OPEN) {
            console.error('WebSocket is not connected');
            return;
        }
        WebsocketService.connection.send(message);
    }

    public static closeConnection(): void {
        if (WebsocketService.connection) {
            WebsocketService.connection.close();
            WebsocketService.connection = null;
        }
    }
}

