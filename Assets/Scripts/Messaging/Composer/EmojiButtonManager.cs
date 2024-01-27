using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Messaging.Composer {
	public class EmojiButtonManager : MonoBehaviour {
		[SerializeField] private TMP_Text[] buttons;

		public void SetButtonImages(string[] emojis) {
			for (int i = 0; i < emojis.Length; i++) {
				buttons[i].SetText(emojis[i]);
			}
		}
	}
}