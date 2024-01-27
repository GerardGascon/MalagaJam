using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Messaging.Typer {
	public class TextTyper {
		private readonly TMP_Text _textBox;

		private const float SecondsPerCharacter = 0.07f;

		public TextTyper(TMP_Text textBox) {
			_textBox = textBox;
		}

		public IEnumerator AnimateTextIn(string processedMessage, Action onFinish) {
			float timeOfLastCharacter = 0;

			_textBox.maxVisibleCharacters = 0;
			_textBox.text = processedMessage;
			_textBox.ForceMeshUpdate();

			while (true) {
				if (ShouldShowNextCharacter(timeOfLastCharacter)) {
					if (_textBox.maxVisibleCharacters <= _textBox.textInfo.characterCount) {
						_textBox.maxVisibleCharacters++;
						if (_textBox.text[_textBox.maxVisibleCharacters - 1] == ' ') {
							_textBox.maxVisibleCharacters++;
						}
						timeOfLastCharacter = Time.unscaledTime;
						if (_textBox.maxVisibleCharacters == _textBox.textInfo.characterCount) {
							onFinish?.Invoke();
							break;
						}
					}
				}
				
				yield return null;
			}
		}

		private static bool ShouldShowNextCharacter(float timeOfLastCharacter) {
			return Time.unscaledTime - timeOfLastCharacter > SecondsPerCharacter;
		}
	}
}