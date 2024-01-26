using System;
using TMPro;
using UnityEngine;

namespace Messaging {
	public class Message : MonoBehaviour{
		public RectTransform RectTransform { private set; get; }
		public string Text => messageText.text;

		[SerializeField] private TMP_Text messageText;

		private void Awake() {
			RectTransform = GetComponent<RectTransform>();
		}

		public void SetMessageText(string text) {
			messageText.text = text;
		}
	}
}