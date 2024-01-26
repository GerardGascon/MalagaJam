using System;
using UnityEngine;

namespace Messaging {
	public class Message : MonoBehaviour{
		public RectTransform RectTransform { private set; get; }

		private void Awake() {
			RectTransform = GetComponent<RectTransform>();
		}
	}
}