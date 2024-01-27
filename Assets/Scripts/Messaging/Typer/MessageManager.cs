using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Messaging {
	public class MessageManager : MonoBehaviour {
		[SerializeField] private MessageStructureGenerator messageStructureGenerator;

		private Message[] _messages;

		[SerializeField] private TextAsset textAsset;

		private void Awake() {
			_messages = messageStructureGenerator.GenerateMessages();
		}

		private async void Start() {
			string[] fLines = textAsset.text.Split(':');
			int i = 0;
			while (Application.isPlaying) {
				Debug.Log(textAsset.text);
				CreateMessage(MessageParser.SplitMessage(fLines[i % 2]).Key);
				++i;
				await Task.Delay(2000);
			}
		}

		public void CreateMessage(string message) {
			ModifyMessageText(message, _messages.Length - 1);
		}
		public void CreateRealMessage(string message) {
			ModifyRealMessageText(message, _messages.Length - 1);
		}

		private void ModifyMessageText(string message, int index) {
			ModifyPreviousMessageText(_messages[index].Text, index - 1);
			_messages[index].SetMessageText(message, true);
		}
		private void ModifyRealMessageText(string message, int index) {
			ModifyPreviousRealMessageText(_messages[index].Text, index - 1);
			_messages[index].SetMessageRealText(message, true);
		}

		private void ModifyPreviousMessageText(string message, int index) {
			if (index < 0) return;

			ModifyPreviousMessageText(_messages[index].Text, index - 1);

			_messages[index].SetMessageText(message, false);
		}
		
		private void ModifyPreviousRealMessageText(string message, int index) {
			if (index < 0) return;

			ModifyPreviousRealMessageText(_messages[index].RealText, index - 1);

			_messages[index].SetMessageRealText(message, false);
		}
	}
}