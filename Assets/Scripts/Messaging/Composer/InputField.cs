using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Messaging.Composer {
	public class InputField : MonoBehaviour {
		[SerializeField] private TMP_Text fieldText;
		[SerializeField] private TMP_Text remainingIconsText;
		
		public string Text => fieldText.text;

		private int _numberIcons;
		public int NumberIcons {
			set {
				_iconsWritten = 0;
				_numberIcons = value;
				remainingIconsText.text = GetRemainingIcons().ToString();
			}
			get => _numberIcons;
		}

		private int _iconsWritten;

		private readonly Regex _removeRegex = new(@"<([a-z]+)(?![^>]*\/>)[^>]*>(?!.*<([a-z]+)(?![^>]*\/>)[^>]*>)");
		
		public void WriteEmoji(string emoji) {
			if (GetRemainingIcons() <= 0) return;
			
			_iconsWritten++;
			fieldText.text += emoji;
			remainingIconsText.text = GetRemainingIcons().ToString();
		}

		private int GetRemainingIcons() => _numberIcons - _iconsWritten;

		public void RemoveEmoji() {
			string newText = _removeRegex.Replace(fieldText.text, "");
			if (newText != fieldText.text) {
				_iconsWritten--;
				remainingIconsText.text = GetRemainingIcons().ToString();
			}
			fieldText.SetText(newText);
		}

		public void ClearText() {
			_iconsWritten = 0;
			remainingIconsText.text = GetRemainingIcons().ToString();
			fieldText.SetText("");
		}
	}
}