using System;
using Messaging.Typer;
using TMPro;
using UnityEngine;
using Utilities;

namespace Messaging {
	public class Message : MonoBehaviour {
		public RectTransform RectTransform { private set; get; }
		public string Text => messageText.text;

		[SerializeField] private TMP_Text messageText;

		private TextTyper _textTyper;
		private Coroutine _typingCoroutine;

		private void Awake() {
			RectTransform = GetComponent<RectTransform>();
			_textTyper = new TextTyper(messageText);
		}

		public void SetMessageText(string text, bool animate) {
			this.EnsureCoroutineStopped(ref _typingCoroutine);

			if (animate)
				_typingCoroutine = StartCoroutine(_textTyper.AnimateTextIn(text, null));
			else
				messageText.text = text;
		}
	}
}