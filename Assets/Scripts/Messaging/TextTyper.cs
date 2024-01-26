using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Messaging {
	public class TextTyper {
		private bool _textAnimating;
		private bool _stopAnimating;

		private readonly TMP_Text _textBox;

		public TextTyper(TMP_Text textBox) {
			_textBox = textBox;
		}

		public IEnumerator AnimateTextIn(List<DialogueCommand> commands, string processedMessage, Action onFinish) {
			_textAnimating = true;
			float secondsPerCharacter = 1f / 150f;
			float timeOfLastCharacter = 0;

			TMP_TextInfo textInfo = _textBox.textInfo;
			foreach (TMP_MeshInfo meshInfer in textInfo.meshInfo) {
				if (meshInfer.vertices == null) continue;
				for (int j = 0; j < meshInfer.vertices.Length; j++) {
					meshInfer.vertices[j] = Vector3.zero;
				}
			}

			_textBox.text = processedMessage;
			_textBox.ForceMeshUpdate();

			Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];
			for (int i = 0; i < originalColors.Length; i++) {
				Color32[] theColors = textInfo.meshInfo[i].colors32;
				originalColors[i] = new Color32[theColors.Length];
				Array.Copy(theColors, originalColors[i], theColors.Length);
			}
			int charCount = textInfo.characterCount;
			float[] charAnimStartTimes = new float[charCount];
			for (int i = 0; i < charCount; i++) {
				charAnimStartTimes[i] = -1;
			}
			int visibleCharacterIndex = 0;
			while (true) {
				if (_stopAnimating) {
					for (int i = visibleCharacterIndex; i < charCount; i++) {
						charAnimStartTimes[i] = Time.unscaledTime;
					}
					visibleCharacterIndex = charCount;
					FinishAnimating(onFinish);
				}
				if (ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter)) {
					if (visibleCharacterIndex <= charCount) {
						ExecuteCommandsForCurrentIndex(commands, visibleCharacterIndex, ref secondsPerCharacter,
							ref timeOfLastCharacter);
						if (visibleCharacterIndex < charCount &&
						    ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter)) {
							charAnimStartTimes[visibleCharacterIndex] = Time.unscaledTime;
							visibleCharacterIndex++;
							timeOfLastCharacter = Time.unscaledTime;
							if (visibleCharacterIndex == charCount) {
								FinishAnimating(onFinish);
							}
						}
					}
				}
				
				_textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				for (int i = 0; i < textInfo.meshInfo.Length; i++) {
					TMP_MeshInfo theInfo = textInfo.meshInfo[i];
					theInfo.mesh.vertices = theInfo.vertices;
					_textBox.UpdateGeometry(theInfo.mesh, i);
				}
				yield return null;
			}
		}

		private static void ExecuteCommandsForCurrentIndex(List<DialogueCommand> commands, int visableCharacterIndex,
			ref float secondsPerCharacter, ref float timeOfLastCharacter) {
			for (int i = 0; i < commands.Count; i++) {
				DialogueCommand command = commands[i];
				if (command.Position != visableCharacterIndex) continue;
				switch (command.Type) {
					case DialogueCommandType.Pause:
						timeOfLastCharacter = Time.unscaledTime + command.FloatValue;
						break;
					case DialogueCommandType.TextSpeedChange:
						secondsPerCharacter = 1f / command.FloatValue;
						break;
				}
				commands.RemoveAt(i);
				i--;
			}
		}

		private void FinishAnimating(Action onFinish) {
			_textAnimating = false;
			_stopAnimating = false;
			onFinish?.Invoke();
		}

		private static bool ShouldShowNextCharacter(float secondsPerCharacter, float timeOfLastCharacter) {
			return (Time.unscaledTime - timeOfLastCharacter) > secondsPerCharacter;
		}

		public void SkipToEndOfCurrentMessage() {
			if (_textAnimating) {
				_stopAnimating = true;
			}
		}

		public bool IsMessageAnimating() {
			return _textAnimating;
		}
	}
}