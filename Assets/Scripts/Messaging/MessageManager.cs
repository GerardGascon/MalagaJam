using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Messaging {
	public class MessageManager : MonoBehaviour{
		[SerializeField] private MessageStructureGenerator messageStructureGenerator;

		private Message[] _messages;
		
		private void Awake() {
			_messages = messageStructureGenerator.GenerateMessages();
		}

		private async void Start() {
			await Task.Delay(2000);
			CreateMessage("asl;dkrhjufioe qvnfioeqlcnnmcuq");
			await Task.Delay(2000);
			CreateMessage("asl;dkrhjufioe qvnfioeqlcnnmcuq");
			await Task.Delay(2000);
			CreateMessage("asl;dkrhjufioe qvnfioeqlcnnmcuq");
			await Task.Delay(2000);
			CreateMessage("asl;dkrhjufioe qvnfioeqlcnnmcuq");
			await Task.Delay(2000);
			CreateMessage("asl;dkrhjufioe qvnfioeqlcnnmcuq");
		}

		public void CreateMessage(string message) {
			ModifyMessageText(message, _messages.Length - 1);
		}

		private void ModifyMessageText(string message, int index) {
			ModifyPreviousMessageText(_messages[index].Text, index - 1);
			_messages[index].SetMessageText(message, true);
		}

		private void ModifyPreviousMessageText(string message, int index) {
			if(index < 0) return;
			
			ModifyPreviousMessageText(_messages[index].Text, index - 1);
			
			_messages[index].SetMessageText(message, false);
		}
	}
}