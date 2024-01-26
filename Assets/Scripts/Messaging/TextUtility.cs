using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Messaging {
	public class DialogueUtility : MonoBehaviour {
		private const string REMAINDER_REGEX = "(.*?((?=>)|(/|$)))";
		private const string PAUSE_REGEX_STRING = "<p:(?<pause>" + REMAINDER_REGEX + ")>";
		private static readonly Regex PauseRegex = new(PAUSE_REGEX_STRING);
		private const string SPEED_REGEX_STRING = "<sp:(?<speed>" + REMAINDER_REGEX + ")>";
		private static readonly Regex SpeedRegex = new(SPEED_REGEX_STRING);

		private static readonly Dictionary<string, float> PauseDictionary = new() {
			{ "tiny", .1f },
			{ "short", .25f },
			{ "normal", 0.666f },
			{ "long", 1f },
			{ "read", 2f },
		};

		public static List<DialogueCommand> ProcessInputString(string message, out string processedMessage) {
			List<DialogueCommand> result = new();
			processedMessage = message;

			processedMessage = HandlePauseTags(processedMessage, result);
			processedMessage = HandleSpeedTags(processedMessage, result);

			return result;
		}

		private static string HandleSpeedTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection speedMatches = SpeedRegex.Matches(processedMessage);
			foreach (Match match in speedMatches) {
				string stringVal = match.Groups["speed"].Value;
				if (!float.TryParse(stringVal, out float val)) {
					val = 150f;
				}
				result.Add(new DialogueCommand {
					Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					Type = DialogueCommandType.TextSpeedChange,
					FloatValue = val
				});
			}
			processedMessage = Regex.Replace(processedMessage, SPEED_REGEX_STRING, "");
			return processedMessage;
		}

		private static string HandlePauseTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection pauseMatches = PauseRegex.Matches(processedMessage);
			foreach (Match match in pauseMatches) {
				string val = match.Groups["pause"].Value;
				string pauseName = val;
				Debug.Assert(PauseDictionary.ContainsKey(pauseName), "no pause registered for '" + pauseName + "'");
				result.Add(new DialogueCommand {
					Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					Type = DialogueCommandType.Pause,
					FloatValue = PauseDictionary[pauseName]
				});
			}
			processedMessage = Regex.Replace(processedMessage, PAUSE_REGEX_STRING, "");
			return processedMessage;
		}

		private static int VisibleCharactersUpToIndex(string message, int index) {
			int result = 0;
			bool insideBrackets = false;
			for (int i = 0; i < index; i++) {
				if (message[i] == '<') {
					insideBrackets = true;
				} else if (message[i] == '>') {
					insideBrackets = false;
					result--;
				}
				if (!insideBrackets) {
					result++;
				} else if (i + 6 < index && message.Substring(i, 6) == "sprite") {
					result++;
				}
			}
			return result;
		}
	}

	public struct DialogueCommand {
		public int Position;
		public DialogueCommandType Type;
		public float FloatValue;
	}

	public enum DialogueCommandType {
		Pause,
		TextSpeedChange,
	}
}