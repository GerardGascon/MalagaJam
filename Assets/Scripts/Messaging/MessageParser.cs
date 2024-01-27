using System.Collections.Generic;

namespace Messaging {
	public static class MessageParser {
		private const char MessageDifferentiator = '|';

		public static KeyValuePair<string, string> SplitMessage(string message) {
			string[] result = message.Split(MessageDifferentiator);
			return new KeyValuePair<string, string>(result[0], result[1]);
		}
	}
}