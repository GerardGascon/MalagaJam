using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flow {
	public class WinScreen : MonoBehaviour {
		[SerializeField] private CanvasGroup canvasGroup;

		[SerializeField] private RectTransform winBackground;
		[SerializeField] private RectTransform winText;

		private bool _hasWon;
		
		public void Win() {
			_hasWon = true;
			
			canvasGroup.blocksRaycasts = true;
			canvasGroup.DOFade(1f, .5f);
			
			winBackground.localScale = new Vector3(0, 0, 1);
			winText.localScale = new Vector3(0, 0, 1);
			winBackground.DOScale(1f, .25f).SetDelay(.5f).SetEase(Ease.OutBack);
			winText.DOScale(1f, .25f).SetDelay(.55f).SetEase(Ease.OutBack);
		}

		private void Update() {
			if (_hasWon && Input.anyKeyDown) {
				Application.Quit();
			}
		}
	}
}