using System.Collections.Generic;

namespace Messaging.MessageData {
	public struct MessageData {
		public KeyValuePair<string, string> QuestionMessage;
		public KeyValuePair<string, string> AnswerMessage;

		public MessageData(string text) {
			string[] jokeParts = text.Split(':');
			
			string[] question = jokeParts[0].Split('|');
			string[] answer = jokeParts[1].Split('|');
			
			QuestionMessage = new KeyValuePair<string, string>(question[0], question[1]);
			AnswerMessage = new KeyValuePair<string, string>(answer[0], answer[1]);
		}
	}
}