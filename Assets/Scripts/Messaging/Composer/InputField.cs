using System.Text.RegularExpressions;
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

		private readonly Regex _removeRegex = new(@"<([a-z]+)(?![^>]*\/>)[^>]*>(?!.*<([a-z]+)(?![^>]*\/>)[^>]*>)");

		public void RemoveEmoji() {
			string newText = _removeRegex.Replace(fieldText.text, "");
			fieldText.SetText(newText);
		}
	}
}