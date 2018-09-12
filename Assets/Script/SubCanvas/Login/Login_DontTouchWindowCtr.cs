using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Login_DontTouchWindowCtr : MonoBehaviour {

	[SerializeField]
	Text Text_text;
	[SerializeField]
	RectTransform RectTrns;


	/// <summary>
    	/// Public関数
   	/// </summary>
	//ウィンドウを開く
	public void OpenWindow(string text){
		Text_text.text = text;
		PopupAnimation (true);
	}
	//ウィンドウを閉じる
	public void CloseWindow(){
		PopupAnimation (false);
	}


	/// <summary>
    	/// 関数
   	/// </summary>
	//Popupアニメーション
	void PopupAnimation (bool flg){
		//Open
		if (flg == true) {
			gameObject.SetActive (true);
			RectTrns.DOScale (Vector3.one, 0.5f)
				.SetEase (Ease.OutBack);
		}
		//Close
		else {
			RectTrns.DOScale (Vector3.zero, 0.5f)
				.SetEase (Ease.InBack)
				.OnComplete (() => {
					// アニメーションが終了時によばれる
					RectTrns.transform.parent.gameObject.SetActive (false);
				}); ;
		}
	}
}
