using System;
using Messaging.Typer;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Messaging {
	public class Message : MonoBehaviour {
		public RectTransform RectTransform { private set; get; }
		public string Text { private set; get; }
		public string RealText { private set; get; }
		public bool IsAnswer { private set; get; }
		public bool IsReal { private set; get; }

		[SerializeField] private MessageContainer questionContainer;
		[SerializeField] private MessageContainer answerContainer;

		private void Awake() {
			RectTransform = GetComponent<RectTransform>();

			questionContainer.InitializeTypers();
			answerContainer.InitializeTypers();
		}

		public Coroutine SetMessageText(string text, bool animate, bool isAnswer) {
			Text = text;
			IsAnswer = isAnswer;
			IsReal = false;

			if (isAnswer) {
				SetText(ref answerContainer.TypingCoroutine, answerContainer.messageText, answerContainer.TextTyper,
					text, animate);
				
				if(string.IsNullOrEmpty(text)) return null;
				answerContainer.container.SetActive(true);
				questionContainer.container.SetActive(false);
				return answerContainer.TypingCoroutine;
			}
			
			SetText(ref questionContainer.TypingCoroutine, questionContainer.messageText,
				questionContainer.TextTyper, text, animate);
				
			if(string.IsNullOrEmpty(text)) return null;
			questionContainer.container.SetActive(true);
			answerContainer.container.SetActive(false);
			return questionContainer.TypingCoroutine;
		}

		public Coroutine SetMessageRealText(string text, bool animate, bool isAnswer) {
			RealText = text;
			IsAnswer = isAnswer;
			IsReal = true;

			if (isAnswer) {
				SetText(ref answerContainer.RealTypingCoroutine, answerContainer.realMessageText,
					answerContainer.RealTextTyper, text, animate);
				
				if(string.IsNullOrEmpty(text)) return null;
				answerContainer.container.SetActive(true);
				questionContainer.container.SetActive(false);
				return answerContainer.RealTypingCoroutine;
			}
			
			SetText(ref questionContainer.RealTypingCoroutine, questionContainer.realMessageText,
				questionContainer.RealTextTyper, text, animate);
				
			if(string.IsNullOrEmpty(text)) return null;
			questionContainer.container.SetActive(true);
			answerContainer.container.SetActive(false);
			return questionContainer.RealTypingCoroutine;
		}

		private void SetText(ref Coroutine routine, TMP_Text tmpText, TextTyper typer, string text, bool animate) {
			this.EnsureCoroutineStopped(ref routine);

			if (animate)
				routine = StartCoroutine(typer.AnimateTextIn(text, null));
			else
				tmpText.text = text;
		}

		[Serializable]
		private class MessageContainer {
			public GameObject container;
			public TMP_Text messageText;
			public TMP_Text realMessageText;

			public Coroutine TypingCoroutine;
			public Coroutine RealTypingCoroutine;

			public TextTyper TextTyper;
			public TextTyper RealTextTyper;

			public void InitializeTypers() {
				TextTyper = new TextTyper(messageText);
				RealTextTyper = new TextTyper(realMessageText);
				
				container.SetActive(false);
			}
		}
	}
}