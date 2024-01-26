using System;
using UnityEngine;

namespace Messaging.Composer {
	public class UndoButton : MonoBehaviour {
		private InputField _inputField;

		private void Awake() {
			_inputField = FindObjectOfType<InputField>();
		}

		public void UndoClick() {
			_inputField.RemoveEmoji();
		}
	}
}