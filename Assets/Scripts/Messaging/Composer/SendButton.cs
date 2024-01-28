using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Messaging.Composer {
	public class SendButton : MonoBehaviour { 
		[SerializeField] private InputField inputField;

		[SerializeField] private Sprite unlockedSprite;
		[SerializeField] private Sprite lockedSprite;
		private bool _locked;
		
		[SerializeField] private Image button;

		private MessageManager _messageManager;
		
		private void Awake() {
			_messageManager = FindObjectOfType<MessageManager>();
		}

		public void Lock() {
			_locked = true;
			button.sprite = lockedSprite;
		}
		public void Unlock() {
			_locked = false;
			button.sprite = unlockedSprite;
		}

		public void SendEmojis() {
			if (CanSendText()) {
				_messageManager.CreateMessage(inputField.Text, true);
				inputField.ClearText();
			}
		}

		private bool CanSendText() {
			bool isInputEmpty = string.IsNullOrEmpty(inputField.Text) || string.IsNullOrWhiteSpace(inputField.Text);
			return !isInputEmpty && !_locked;
		}
	}
}