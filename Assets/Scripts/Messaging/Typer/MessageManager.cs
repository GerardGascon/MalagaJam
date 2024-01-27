using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flow;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Messaging {
	public class MessageManager : MonoBehaviour {
		[SerializeField] private MessageStructureGenerator messageStructureGenerator;

		private Message[] _messages;

		[SerializeField] private TextAsset[] jokes;
		[SerializeField, Min(0)] private float sendJokeDelay = 2f;
		[SerializeField, Min(0)] private float sendJokeLongDelay = 8f;
		[SerializeField, Min(0)] private float initialSendJokeDelay = 4f;

		private List<TextAsset> _jokesBag;

		private MessageData.MessageData _currentJoke;
		private int _currentJokeIndex;
		
		private Lives _lives;

		private void Awake() {
			_messages = messageStructureGenerator.GenerateMessages();
			_jokesBag = new List<TextAsset>(jokes);
			_lives = FindObjectOfType<Lives>();
		}

		private void Start() {
			SendRandomJoke(initialSendJokeDelay);
		}

		private async void SendRandomJoke(float delay) {
			_currentJoke = GetRandomJoke();
			await Task.Delay((int)(delay * 1000));
			CreateMessage(_currentJoke.QuestionMessage.Key, false);
		}

		public void CreateMessage(string message, bool isAnswer) {
			ModifyMessageText(message, _messages.Length - 1, isAnswer);
			if (isAnswer) {
				_currentJokeIndex--;
				_currentJokeIndex = Mathf.Max(_currentJokeIndex, 0);
				if (message == _currentJoke.AnswerMessage.Key) {
					StartCoroutine(ShowRealTexts());
					if(_currentJokeIndex == 0)
						SendRandomJoke(sendJokeLongDelay);
					else
						SendRandomJoke(sendJokeDelay);
				} else {
					_lives.Wrong();
					if (_lives.CurrentLives == 0) {
						StartCoroutine(ShowRealTexts());
						SendRandomJoke(sendJokeLongDelay);
						_lives.ResetLives();
					}
				}
			} else {
				_currentJokeIndex = _messages.Length - 1;
			}
		}

		IEnumerator ShowRealTexts() {
			yield return ModifyRealMessageText(_currentJoke.QuestionMessage.Value, _currentJokeIndex, false);
			yield return ModifyRealMessageText(_currentJoke.AnswerMessage.Value, _messages.Length - 1, true);
		}

		private void ModifyMessageText(string message, int index, bool isAnswer) {
			ModifyPreviousMessage(index);
			_messages[index].SetMessageText(message, true, isAnswer);
		}

		private Coroutine ModifyRealMessageText(string message, int index, bool isAnswer) {
			return _messages[index].SetMessageRealText(message, true, isAnswer);
		}

		private Coroutine ModifyPreviousMessage(int index) {
			ModifyPreviousMessage(_messages[index].Text, _messages[index].RealText, index - 1,
				_messages[index].IsAnswer, _messages[index].IsReal);

			_messages[index].SetMessageText("", false, _messages[index].IsAnswer);
			return _messages[index].SetMessageRealText("", false, _messages[index].IsAnswer);
		}

		private MessageData.MessageData GetRandomJoke() {
			TextAsset joke = _jokesBag[Random.Range(0, _jokesBag.Count)];
			if (_jokesBag.Count == 0)
				_jokesBag = new List<TextAsset>(jokes);

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