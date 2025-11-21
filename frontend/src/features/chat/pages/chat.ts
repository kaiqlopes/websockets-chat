import { WebsocketService } from "../services/websocketService.js";
import { AuthService } from "../../auth/services/authService.js";

export class Chat {
  private static messageBox = document.querySelector('.messages') as HTMLDivElement | null;

  public static initialize(): void {
    Chat.setupMessageForm();
    Chat.setupLogoutButton();
  }

  private static setupMessageForm(): void {
    const messageForm = document.querySelector('#input-area') as HTMLFormElement;
    const messageInput = messageForm?.querySelector<HTMLInputElement>('input[name="message-to-send"]');
    
    if (!messageForm || !messageInput) {
      console.error('Message form not found');
      return;
    }

    messageForm.addEventListener('submit', (event) => {
      event.preventDefault();

      const message = messageInput.value.trim();
      if (!message) {
        return;
      }

      Chat.renderMessage(message, 'sent');
      WebsocketService.sendMessage(message);
      messageInput.value = '';
    });
  }

  public static receiveMessage(message: string): void {
    if (!message || !message.trim()) {
      return;
    }
  
    console.log("message received:", message);
    Chat.renderMessage(message, 'received');
  }

  private static renderMessage(message: string, messageType: 'sent' | 'received'): void {
    if (!Chat.messageBox) {
      console.error('Message box not found');
      return;
    }

    const messageElement = document.createElement('div');
    messageElement.classList.add(messageType === 'sent' ? 'message-sent' : 'message-received');
    messageElement.textContent = message;

    Chat.messageBox.appendChild(messageElement);
    Chat.messageBox.scrollTop = Chat.messageBox.scrollHeight;
  }

  private static setupLogoutButton(): void {
    const logoutButton = document.getElementById('logout-btn') as HTMLButtonElement | null;
    const logoutModal = document.getElementById('logout-modal') as HTMLDivElement | null;
    const logoutConfirmBtn = document.getElementById('logout-confirm-btn') as HTMLButtonElement | null;
    const logoutCancelBtn = document.getElementById('logout-cancel-btn') as HTMLButtonElement | null;

    if (!logoutButton || !logoutModal || !logoutConfirmBtn || !logoutCancelBtn) {
      console.error('Logout elements not found');
      return;
    }

    const showModal = (): void => {
      logoutModal.style.display = 'block';
    };

    const hideModal = (): void => {
      logoutModal.style.display = 'none';
    };

    logoutButton.addEventListener('click', (event) => {
      event.stopPropagation();
      showModal();
    });

    logoutConfirmBtn.addEventListener('click', async () => {
      hideModal();
      await AuthService.logout();
    });

    logoutCancelBtn.addEventListener('click', () => {
      hideModal();
    });

    // Fecha o modal ao clicar fora dele
    document.addEventListener('click', (event) => {
      if (logoutModal.style.display === 'block' && 
          !logoutModal.contains(event.target as Node) && 
          !logoutButton.contains(event.target as Node)) {
        hideModal();
      }
    });
  }
}

document.addEventListener('DOMContentLoaded', () => {
  Chat.initialize();
  WebsocketService.startConnection();
});

