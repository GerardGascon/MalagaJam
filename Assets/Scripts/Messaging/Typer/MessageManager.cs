using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Messaging {
	public class MessageManager : MonoBehaviour {
		[SerializeField] private MessageStructureGenerator messageStructureGenerator;

		private Message[] _messages;

		[SerializeField] private TextAsset[] jokes;
		[SerializeField, Min(0)] private float sendJokeDelay = 2f;
		[SerializeField, Min(0)] private float initialSendJokeDelay = 4f;

		private List<TextAsset> jokesBag;

		private MessageData.MessageData _currentJoke;
		private int _currentJokeIndex;

		private void Awake() {
			_messages = messageStructureGenerator.GenerateMessages();
			jokesBag = new List<TextAsset>(jokes);
		}

		private void Start() {
			SendRandomJoke();
		}

		private async void SendRandomJoke() {
			_currentJoke = GetRandomJoke();
			await Task.Delay((int)(initialSendJokeDelay * 1000));
			CreateMessage(_currentJoke.QuestionMessage.Key, false);
		}

		public void CreateMessage(string message, bool isAnswer) {
			ModifyMessageText(message, _messages.Length - 1, isAnswer);
			if (isAnswer) {
				_currentJokeIndex--;
				_currentJokeIndex = Mathf.Max(_currentJokeIndex, 0);
				if (message == _currentJoke.AnswerMessage.Key) {
					ModifyRealMessageText(_currentJoke.QuestionMessage.Value, _currentJokeIndex, false);
					ModifyRealMessageText(_currentJoke.AnswerMessage.Value, _messages.Length - 1, true);
					SendRandomJoke();
				} else {
					//TODO: Add lives support
				}
			} else {
				_currentJokeIndex = _messages.Length - 1;
			}
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

		private MessageData.MessageData GetRandomJoke() {
			TextAsset joke = jokesBag[Random.Range(0, jokesBag.Count)];
			if (jokesBag.Count == 0)
				jokesBag = new List<TextAsset>(jokes);

			return new MessageData.MessageData(joke.text);
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