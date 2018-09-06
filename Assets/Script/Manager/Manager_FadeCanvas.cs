using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Manager_FadeCanvas : MonoBehaviour {

	public Image FadeImg;

	float FadeSpeed = 0.02f;


/**Public関数**/
	public IEnumerator FadeIn (UnityAction CallBack){
	
		FadeImg.raycastTarget = true;

		for (float i = 0; i < 1; i += FadeSpeed) {
			FadeImg.color = new Color(0f,0f,0f,i);
			yield return null;
		}
		FadeImg.color = new Color (0f, 0f, 0f, 1f);

		CallBack ();
	}
	public IEnumerator FadeOut (UnityAction CallBack){

		for (float i = 1; i > 0; i -= FadeSpeed) {
			FadeImg.color = new Color (0f, 0f, 0f, i);
			yield return null;
		}

		FadeImg.color = new Color (0f, 0f, 0f, 0f);
		FadeImg.raycastTarget = false;

		CallBack ();
	}
}