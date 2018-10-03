using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ctr_Coop_InputTeamNumber : MonoBehaviour {




	public void Open(){
		PopupAnimation (true, gameObject.GetComponent<RectTransform> ());
	}
	public void Close(){
		PopupAnimation (false, gameObject.GetComponent<RectTransform> ());
	}

//UGUI

	//インプット
	public void OnEndEdit (string text){
		gameObject.transform.parent.GetComponent<Ctr_Coop>().number = text;
	}


	//Popupアニメーション関数
	void PopupAnimation (bool flg, RectTransform ThisPopup){
		//Open
		if (flg == true) {
			ThisPopup.DOScale (Vector3.one, 0.5f)
				 .SetEase (Ease.OutBack);
		}
		//Close
		else {
			ThisPopup.DOScale (Vector3.zero, 0.5f)
				 .SetEase (Ease.InBack)
				 .OnComplete (() => {
					 // アニメーションが終了時によばれる
				 });
		}
	}
}
