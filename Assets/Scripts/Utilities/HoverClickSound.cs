using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverClickSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
	public void OnPointerEnter(PointerEventData eventData) {
		AudioManager.instance.PlayOneShot("hover");	
	}

	public void OnPointerClick(PointerEventData eventData) {
		AudioManager.instance.PlayOneShot("click");
	}
}