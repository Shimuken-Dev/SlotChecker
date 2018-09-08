using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NCMB;

public class Ctr_Login : MonoBehaviour {

	public GameObject 	
	Obj_Window,
	Obj_SingWindow;

	RectTransform 		
	Rect_Window,
	Rect_SingWindow;

	Text 			Text_Window;

	void Start (){
		Initialize ();
		Login_Check ();
	}


/**関数**/
	//初期化
	void Initialize(){
		Rect_Window 			= Obj_Window.GetComponent<RectTransform> ();
		Rect_SingWindow 		= Obj_SingWindow.GetComponent<RectTransform>();
		Text_Window 			= Obj_Window.transform.Find ("WindowText").GetComponent<Text> ();
	}
	//アカウント情報確認
	void Login_Check(){
		if(Data_User.Mail_Address != "" && Data_User.Pass_Word != ""){
			//Popup設定
			Text_Window.text = string.Format ("{0}\n{1}\nでログイン認証中です", Data_User.Mail_Address, Data_User.User_Name);
			PopupAnimation (true, Rect_Window);
			Login_Auto ();
		}
		else{
			PopupAnimation (true, Rect_SingWindow);
		}
	}
	//自動ログイン
	void Login_Auto(){
		NCMBUser.LogInWithMailAddressAsync (Data_User.Mail_Address, Data_User.Pass_Word, (NCMBException e) => {
			if (e != null) {
				PopupAnimation (true, Rect_Window);
				Text_Window.text = "ログインに失敗しました";
			} else {
				PopupAnimation (true, Rect_Window);
				Text_Window.text = "ログインに成功しました";
			}
		});
	}


	//Popupアニメーション関数
	void PopupAnimation (bool flg,RectTransform RectTrns){
		//Open
		if (flg == true) {
			RectTrns.transform.localScale = Vector3.zero;

			RectTrns.DOScale (Vector3.one, 0.5f)
			        .SetEase (Ease.OutBack);
		}
		//Close
		else {
			RectTrns.DOScale (Vector3.zero, 0.5f)
			        .SetEase (Ease.InBack);
		}
	}
}
