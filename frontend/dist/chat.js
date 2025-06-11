import { WebsocketManager } from "./websocket.js";
export class Chat {
    static SendMessage() {
        const messageForm = document.querySelector('#input-area');
        const messageInput = messageForm.querySelector('input[name="message-to-send"]');
        messageForm.addEventListener('submit', function (event) {
            event.preventDefault();
            if (!messageInput || !messageInput.value.trim())
                return;
            Chat.RenderMessage(messageInput.value, 'sent');
            WebsocketManager.SendMessage(messageInput.value);
            messageInput.value = '';
        });
    }
    static ReceiveMessage(message) {
        if (!message || !message.trim())
            return;
        console.log("message received:", message);
        Chat.RenderMessage(message, 'received');
    }
    static RenderMessage(message, messageType) {
        if (!Chat.messageBox) {
            console.error('Message box not found');
            return;
        }
        const messageElement = document.createElement('div');
        messageElement.classList.add(messageType === 'sent' ? 'message-sent' : 'message-received');
        messageElement.textContent = message;
        Chat.messageBox.appendChild(messageElement);
    }
    ;
}
Chat.messageBox = document.querySelector('.messages');
document.addEventListener('DOMContentLoaded', () => {
    Chat.SendMessage();
});
