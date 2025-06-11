import { WebsocketManager } from "./websocket.js";

export class Chat {
  private static messageBox = document.querySelector('.messages') as HTMLDivElement | null;

  public static SendMessage(): void {
    const messageForm = document.querySelector('#input-area') as HTMLFormElement;
    const messageInput = messageForm.querySelector<HTMLInputElement>('input[name="message-to-send"]');
    
    messageForm.addEventListener('submit', function (event) {
      event.preventDefault();

      if (!messageInput || !messageInput.value.trim())
        return;

      Chat.RenderMessage(messageInput.value, 'sent');
      WebsocketManager.SendMessage(messageInput.value);
      messageInput.value = '';
    });
  }

  public static ReceiveMessage(message: string): void {
    if (!message || !message.trim()) 
      return;
  
    console.log("message received:", message);
    Chat.RenderMessage(message, 'received');
  }

  private static RenderMessage(message: string, messageType: 'sent' | 'received'): void {
    if (!Chat.messageBox) {
      console.error('Message box not found');
      return;
    }

    const messageElement = document.createElement('div');
    messageElement.classList.add(messageType === 'sent' ? 'message-sent' : 'message-received');
    messageElement.textContent = message;

    Chat.messageBox.appendChild(messageElement);
  };

}

document.addEventListener('DOMContentLoaded', () => {
  Chat.SendMessage();
});
