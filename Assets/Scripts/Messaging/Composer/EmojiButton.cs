using System;
using TMPro;
using UnityEngine;

namespace Messaging.Composer {
	public class EmojiButton : MonoBehaviour {
		private TMP_Text _text;
		private InputField _inputField;
		
		private void Awake() {
			_text = GetComponentInChildren<TMP_Text>();
			_inputField = FindObjectOfType<InputField>();
		}

		public void OnClick() {
			string emojiToWrite = _text.text;
			_inputField.WriteEmoji(emojiToWrite);
		}
	}
}