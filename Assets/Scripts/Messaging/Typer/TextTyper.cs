using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Messaging.Typer {
	public class TextTyper {
		private readonly TMP_Text _textBox;

		private static readonly Color32 Clear = new(0, 0, 0, 0);
		private const float SecondsPerCharacter = 1f / 60f;

		public TextTyper(TMP_Text textBox) {
			_textBox = textBox;
		}

		public IEnumerator AnimateTextIn(string processedMessage, Action onFinish) {
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
			int visibleCharacterIndex = 0;
			while (true) {
				if (ShouldShowNextCharacter(SecondsPerCharacter, timeOfLastCharacter)) {
					if (visibleCharacterIndex <= charCount) {
						if (visibleCharacterIndex < charCount &&
						    ShouldShowNextCharacter(SecondsPerCharacter, timeOfLastCharacter)) {
							visibleCharacterIndex++;
							timeOfLastCharacter = Time.unscaledTime;
							if (visibleCharacterIndex == charCount) {
								onFinish?.Invoke();
							}
						}
					}
				}
				
				for (int j = 0; j < charCount; j++) {
					TMP_CharacterInfo charInfo = textInfo.characterInfo[j];
					if (!charInfo.isVisible) continue;
					
					int vertexIndex = charInfo.vertexIndex;
					int materialIndex = charInfo.materialReferenceIndex;
					Color32[] destinationColors = textInfo.meshInfo[materialIndex].colors32;
					Color32 theColor = j < visibleCharacterIndex ? originalColors[materialIndex][vertexIndex] : Clear;
					destinationColors[vertexIndex + 0] = theColor;
					destinationColors[vertexIndex + 1] = theColor;
					destinationColors[vertexIndex + 2] = theColor;
					destinationColors[vertexIndex + 3] = theColor;
				}
				
				_textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				for (int i = 0; i < textInfo.meshInfo.Length; i++) {
					TMP_MeshInfo theInfo = textInfo.meshInfo[i];
					theInfo.mesh.vertices = theInfo.vertices;
					_textBox.UpdateGeometry(theInfo.mesh, i);
				}
				
				if (visibleCharacterIndex == charCount) {
					break;
				}
				
				yield return null;
			}
		}

		private static bool ShouldShowNextCharacter(float secondsPerCharacter, float timeOfLastCharacter) {
			return Time.unscaledTime - timeOfLastCharacter > secondsPerCharacter;
		}
	}
}