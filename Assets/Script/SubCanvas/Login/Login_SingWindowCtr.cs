using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Login_SingWindowCtr : MonoBehaviour {

	RectTransform RectTrns;

	Ctr_Login LoginCtr;
	Login_SingUpInputWindowCtr SingUpInputWindowCtr;
	Login_SingInInputWindowCtr SingInInputWindowCtr;


	void Start (){
		Initialize ();
	}

	/// <summary>
	/// Public関数
	/// </summary>

	//開く
	public void OpenWindow(){
		PopupAnimation (true);
	}
	//閉じる
	public void CloseWindow (){
		PopupAnimation (false);
	}

	/// <summary>
	/// UGUI
	/// </summary>

	//ログインウィンドウ表示ボタン
	public void OnClick_SingUpWindowBtn (){
		SingUpInputWindowCtr.OpenWindow ();
		CloseWindow ();
		LoginCtr.Active_Popup = Ctr_Login.ActivePopup.SingUpInputWindow;
	}
	//会員登録ウィンドウ表示ボタン
	public void OnClick_SingInWindowBtn (){
		SingInInputWindowCtr.OpenWindow ();
		CloseWindow ();
		LoginCtr.Active_Popup = Ctr_Login.ActivePopup.SingInInputWindow;
	}

	/// <summary>
    	/// 関数
   	/// </summary>

	//初期化
	void Initialize(){
		RectTrns = gameObject.GetComponent<RectTransform> ();
		LoginCtr = gameObject.transform.parent.GetComponent<Ctr_Login> ();
		SingUpInputWindowCtr = gameObject.transform.parent.Find ("SingUpInputWindow").GetComponent<Login_SingUpInputWindowCtr> ();
		SingInInputWindowCtr = gameObject.transform.parent.Find ("SingInInputWindow").GetComponent<Login_SingInInputWindowCtr> ();
	}
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
					RectTrns.gameObject.SetActive (false);
				}); ;
		}
	}
}
