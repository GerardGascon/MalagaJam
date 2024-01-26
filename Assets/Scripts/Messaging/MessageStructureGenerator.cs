using System.Collections.Generic;
using UnityEngine;

namespace Messaging {
	public class MessageStructureGenerator : MonoBehaviour {

		[SerializeField, Min(0)] private int maxVisibleMessages = 3;
		[SerializeField, Min(0)] private int messageHeight = 16;

		[SerializeField] private Message messagePrefab;

		public Message[] GenerateMessages() {
			List<Message> messages = new();
			for (int i = 0; i < maxVisibleMessages; i++) {
				Message message = GenerateMessage(i * messageHeight);
				messages.Add(message);
			}
			return messages.ToArray();
		}

		private Message GenerateMessage(int offsetY) {
			Message message = Instantiate(messagePrefab, Vector2.zero, Quaternion.identity, transform);
			message.RectTransform.anchoredPosition = new Vector2(0, -offsetY);
			return message;
		}
	}
}