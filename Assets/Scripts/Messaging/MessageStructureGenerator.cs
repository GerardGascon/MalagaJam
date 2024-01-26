using UnityEngine;

namespace Messaging {
	public class MessageStructureGenerator : MonoBehaviour {

		[SerializeField, Min(0)] private int maxVisibleMessages = 3;
		[SerializeField, Min(0)] private int messageHeight = 16;

		[SerializeField] private RectTransform messagePrefab;
		
		private void Awake() {
			GenerateMessages();
		}

		private void GenerateMessages() {
			for (int i = 0; i < maxVisibleMessages; i++) {
				GenerateMessage(i * messageHeight);
			}
		}

		private void GenerateMessage(int offsetY) {
			RectTransform message = Instantiate(messagePrefab, Vector2.zero, Quaternion.identity, transform);
			message.anchoredPosition = new Vector2(0, -offsetY);
		}
	}
}