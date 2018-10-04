using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Login_WarningWindow : MonoBehaviour {

	public enum ButtonType{
		Close,
		Fnish
	}

	[SerializeField]
	Text 
	Text_text,
	Btn_text;

	[SerializeField]
	RectTransform RectTrns;

	[SerializeField]
	Button Btn;

	Manager_SubCanvas SubCanvasMng;


	void Start(){
		Initialize ();
	}


	/// <summary>
	/// Public関数
	/// </summary>
	//ウィンドウを開く
	public void OpenWindow (string text,ButtonType type){
		Text_text.text = text;
		switch(type){
		case ButtonType.Close:
			Btn_text.text = "OK";
			Btn.onClick.AddListener (OnClick_CloseBtn);
			break;
		case ButtonType.Fnish:
			Btn_text.text = "OK";
			Btn.onClick.AddListener (OnClick_FnishBtn);
			break;
		}
		PopupAnimation (true);
	}
	//ウィンドウを閉じる
	public void CloseWindow (){
		PopupAnimation (false);
	}


	/// <summary>
	/// UGUI
	/// </summary>
	//閉じるボタン
	public void OnClick_CloseBtn(){
		Btn.onClick.RemoveAllListeners ();
		CloseWindow ();
	}
	//ログインPopupを閉じる
	public void OnClick_FnishBtn(){
		SubCanvasMng.CloseSub (false, gameObject.transform.parent.GetComponent<RectTransform> ());
	}


	/// <summary>
	/// 関数
	/// </summary>
	//初期化
	void Initialize (){
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
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
					RectTrns.transform.parent.gameObject.SetActive (false);
				}); ;
		}
	}
}
