using TMPro;
using UnityEngine;

namespace Messaging.Composer {
	public class InputField : MonoBehaviour {
		[SerializeField] private TMP_Text fieldText;

		public void EmptyText() {
			fieldText.SetText("");
		}
		
		public void WriteEmoji(string emoji) {
			fieldText.text += emoji;
		}
	}
}