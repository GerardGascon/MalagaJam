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
				bool isAnswer = i % 2 != 0;

				CreateMessage(MessageParser.SplitMessage(fLines[i % 2]).Key, isAnswer);
				await Task.Delay(2000);
				CreateRealMessage(MessageParser.SplitMessage(fLines[i % 2]).Value, isAnswer);
				await Task.Delay(2000);
				++i;
			}
		}

		public void CreateMessage(string message, bool isAnswer) {
			ModifyMessageText(message, _messages.Length - 1, isAnswer);
		}

		public void CreateRealMessage(string message, bool isAnswer) {
			ModifyRealMessageText(message, _messages.Length - 1, isAnswer);
		}

		private void ModifyMessageText(string message, int index, bool isAnswer) {
			ModifyPreviousMessage(index);
			_messages[index].SetMessageText(message, true, isAnswer);
		}

		private void ModifyRealMessageText(string message, int index, bool isAnswer) {
			_messages[index].SetMessageRealText(message, true, isAnswer);
		}

		private void ModifyPreviousMessage(int index) {
			ModifyPreviousMessage(_messages[index].Text, _messages[index].RealText, index - 1,
				_messages[index].IsAnswer, _messages[index].IsReal);

			_messages[index].SetMessageText("", false, _messages[index].IsAnswer);
			_messages[index].SetMessageRealText("", false, _messages[index].IsAnswer);
		}

		private void ModifyPreviousMessage(string message, string realMessage, int index, bool isAnswer, bool isReal) {
			if (index < 0) return;

			ModifyPreviousMessage(_messages[index].Text, _messages[index].RealText, index - 1,
				_messages[index].IsAnswer, _messages[index].IsReal);

			_messages[index].SetMessageText(message, false, isAnswer);
			_messages[index].SetMessageRealText(realMessage, false, isAnswer);
		}
	}
}