import { Chat } from './chat.js';

export class WebsocketManager {
    private static WEBSOCKET_URL = "wss://localhost:7230/ws"
    private static connection: WebSocket;

    public static StartWebsocketConnection(): void {
        WebsocketManager.connection = new WebSocket(WebsocketManager.WEBSOCKET_URL);

        WebsocketManager.connection.addEventListener('open', () => {
            console.log('WebSocket connection established');
        });

        WebsocketManager.connection.addEventListener('message', (event) => {
            Chat.ReceiveMessage(event.data);
        });

        WebsocketManager.connection.addEventListener('close', () => {
            console.log('WebSocket connection closed');
        });

        WebsocketManager.connection.addEventListener('error', (error) => {
            console.error('WebSocket error:', error);
        });
    }

    public static SendMessage(message: string): void {
        WebsocketManager.connection.send(message);
    }
}

document.addEventListener('DOMContentLoaded', () => {
    WebsocketManager.StartWebsocketConnection();
});