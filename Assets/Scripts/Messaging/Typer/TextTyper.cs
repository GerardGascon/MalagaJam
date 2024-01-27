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

			TMP_TextInfo textInfo = _textBox.textInfo;

			_textBox.maxVisibleCharacters = 0;
			_textBox.text = processedMessage;
			_textBox.ForceMeshUpdate();

			int charCount = textInfo.characterCount;
			int visibleCharacterIndex = 0;
			while (true) {
				if (ShouldShowNextCharacter(SecondsPerCharacter, timeOfLastCharacter)) {
					if (visibleCharacterIndex <= charCount) {
						visibleCharacterIndex++;
						_textBox.maxVisibleCharacters = visibleCharacterIndex;
						timeOfLastCharacter = Time.unscaledTime;
						if (visibleCharacterIndex == charCount) {
							onFinish?.Invoke();
							break;
						}
					}
				}
				
				yield return null;
			}
		}

		private static bool ShouldShowNextCharacter(float secondsPerCharacter, float timeOfLastCharacter) {
			return Time.unscaledTime - timeOfLastCharacter > secondsPerCharacter;
		}
	}
}