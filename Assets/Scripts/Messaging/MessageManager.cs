using System;
using UnityEngine;

namespace Messaging {
	public class MessageManager : MonoBehaviour{
		[SerializeField] private MessageStructureGenerator messageStructureGenerator;

		private Message[] _messages;
		
		private void Awake() {
			_messages = messageStructureGenerator.GenerateMessages();
		}
	}
}