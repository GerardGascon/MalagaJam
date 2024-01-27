using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Messaging.Composer {
	public class SendButton : MonoBehaviour { 
		[SerializeField] private InputField inputField;

		private MessageManager _messageManager;
		
		private void Awake() {
			_messageManager = FindObjectOfType<MessageManager>();
		}

		public void SendEmojis() {
			if (CanSendText()) {
				_messageManager.CreateMessage(inputField.Text, true);
				inputField.ClearText();
			}
		}

		private bool CanSendText() {
			return !string.IsNullOrEmpty(inputField.Text) && !string.IsNullOrWhiteSpace(inputField.Text);
		}
	}
}