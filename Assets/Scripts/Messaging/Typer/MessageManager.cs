using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flow;
using Messaging.Composer;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Messaging {
	public class MessageManager : MonoBehaviour {
		[SerializeField] private MessageStructureGenerator messageStructureGenerator;

		private Message[] _messages;

		[SerializeField] private TextAsset[] jokes;
		[SerializeField, Min(0)] private float sendJokeDelay = 2f;
		[SerializeField, Min(0)] private float initialSendJokeDelay = 4f;

		private List<TextAsset> _jokesBag;

		private MessageData.MessageData _currentJoke;
		private int _currentJokeIndex;
		
		private Lives _lives;

		private readonly Regex _emojiGetter = new("<([a-z]+)(?![^>]*\\/>)[^>]*>");
		private const int NumEmojis = 35;
		private EmojiButtonManager _emojiButtonManager;
		private SendButton _sendButton;

		private void Awake() {
			_messages = messageStructureGenerator.GenerateMessages();
			_jokesBag = new List<TextAsset>(jokes);
			_lives = FindObjectOfType<Lives>();
			_emojiButtonManager = FindObjectOfType<EmojiButtonManager>();
			_sendButton = FindObjectOfType<SendButton>();
		}

		private void Start() {
			StartCoroutine(SendRandomJoke(initialSendJokeDelay));
		}

		private IEnumerator SendRandomJoke(float delay) {
			_sendButton.Lock();
			yield return new WaitForSeconds(delay);
			_sendButton.Unlock();
			
			_currentJoke = GetRandomJoke();
			CreateMessage(_currentJoke.QuestionMessage.Key, false);
			_emojiButtonManager.SetButtonImages(GenerateButtonOptions());
		}
		
		private IEnumerator SendRandomJoke(Coroutine routine, float delay) {
			yield return routine;
			_sendButton.Lock();
			yield return new WaitForSeconds(delay);
			_sendButton.Unlock();
			
			_currentJoke = GetRandomJoke();
			CreateMessage(_currentJoke.QuestionMessage.Key, false);
			_emojiButtonManager.SetButtonImages(GenerateButtonOptions());
		}

		public void CreateMessage(string message, bool isAnswer) {
			ModifyMessageText(message, _messages.Length - 1, isAnswer, true);
			if (isAnswer) {
				_currentJokeIndex--;
				_currentJokeIndex = Mathf.Max(_currentJokeIndex, 0);
				if (message == _currentJoke.AnswerMessage.Key) {
					_sendButton.Lock();
					bool isCorrect = _currentJokeIndex != 0;
					Coroutine routine = StartCoroutine(ShowRealTexts(isCorrect));
					StartCoroutine(SendRandomJoke(routine, sendJokeDelay));
					
					if (isCorrect) {
						FindObjectOfType<ProgressBar>().AddProgress();
					}
				} else {
					_lives.Wrong();
					if (_lives.CurrentLives == 0) {
						_sendButton.Lock();
						Coroutine routine = StartCoroutine(ShowRealTexts(false));
						StartCoroutine(SendRandomJoke(routine, initialSendJokeDelay));
						_lives.ResetLives();
					}
				}
			} else {
				_currentJokeIndex = _messages.Length - 1;
			}
		}

		private IEnumerator ShowRealTexts(bool isCorrect) {
			yield return ModifyRealMessageText(_currentJoke.QuestionMessage.Value, _currentJokeIndex, false);
			if (!isCorrect) {
				yield return ModifyMessageText(_currentJoke.AnswerMessage.Key, _messages.Length - 1, true, false);
			}
			yield return ModifyRealMessageText(_currentJoke.AnswerMessage.Value, _messages.Length - 1, true);
		}

		private string[] GenerateButtonOptions() {
			List<string> neededEmojis = GetResultEmojis();

			while (neededEmojis.Count < 6) {
				string emoji;
				do {
					emoji = $"<sprite={Random.Range(0, NumEmojis)}>";
				} while (neededEmojis.Contains(emoji));
				neededEmojis.Add(emoji);
			}

			neededEmojis.Shuffle();
			return neededEmojis.ToArray();
		}

		private List<string> GetResultEmojis() {
			MatchCollection emojis = _emojiGetter.Matches(_currentJoke.AnswerMessage.Key);
			FindObjectOfType<InputField>().NumberIcons = emojis.Count;

			List<string> emojiStrings = new();
			foreach (Match emoji in emojis) {
				emojiStrings.Add(emoji.Value);
			}

			return emojiStrings;
		}

		private Coroutine ModifyMessageText(string message, int index, bool isAnswer, bool createNew) {
			if(createNew)
				ModifyPreviousMessage(index);
			
			return _messages[index].SetMessageText(message, true, isAnswer);
		}

		private Coroutine ModifyRealMessageText(string message, int index, bool isAnswer) {
			return _messages[index].SetMessageRealText(message, true, isAnswer);
		}

		private void ModifyPreviousMessage(int index) {
			ModifyPreviousMessage(_messages[index].Text, _messages[index].RealText, index - 1,
				_messages[index].IsAnswer, _messages[index].IsReal);

			_messages[index].SetMessageText("", false, _messages[index].IsAnswer);
			_messages[index].SetMessageRealText("", false, _messages[index].IsAnswer);
		}

		private MessageData.MessageData GetRandomJoke() {
			TextAsset joke = _jokesBag[Random.Range(0, _jokesBag.Count)];
			_jokesBag.Remove(joke);
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