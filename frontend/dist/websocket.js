import { Chat } from './chat.js';
export class WebsocketManager {
    static StartWebsocketConnection() {
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
    static SendMessage(message) {
        WebsocketManager.connection.send(message);
    }
}
WebsocketManager.WEBSOCKET_URL = "wss://localhost:7230/ws";
document.addEventListener('DOMContentLoaded', () => {
    WebsocketManager.StartWebsocketConnection();
});
