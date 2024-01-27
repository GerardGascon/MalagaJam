using System;
using Messaging.Typer;
using TMPro;
using UnityEngine;
using Utilities;

namespace Messaging {
	public class Message : MonoBehaviour {
		public RectTransform RectTransform { private set; get; }
		public string Text => messageText.text;
		public string RealText => realMessageText.text;

		[SerializeField] private TMP_Text messageText;
		[SerializeField] private TMP_Text realMessageText;

		private TextTyper _textTyper;
		private TextTyper _realTextTyper;
		private Coroutine _typingCoroutine;
		private Coroutine _realTypingCoroutine;

		private void Awake() {
			RectTransform = GetComponent<RectTransform>();
			_textTyper = new TextTyper(messageText);
			_realTextTyper = new TextTyper(realMessageText);
		}

		public void SetMessageText(string text, bool animate) => 
			SetText(ref _typingCoroutine, messageText, _textTyper, text, animate);
		public void SetMessageRealText(string text, bool animate) => 
			SetText(ref _realTypingCoroutine, realMessageText, _realTextTyper, text, animate);

		private void SetText(ref Coroutine routine, TMP_Text tmpText, TextTyper typer, string text, bool animate) {
			this.EnsureCoroutineStopped(ref routine);

			if (animate)
				routine = StartCoroutine(typer.AnimateTextIn(text, null));
			else
				tmpText.text = text;
		}
	}
}