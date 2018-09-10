using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NCMB;

public class Ctr_Login : MonoBehaviour {

	enum ActivePopup{
		Window,
		SingWindow,
		SingUpInputWindow,
		SingInInputWindow
	}

	public GameObject 	
	Obj_Window,
	Obj_SingWindow,
	Obj_SingUpWindow,
	Obj_SingInWindow;

	RectTransform 		
	Rect_Window,
	Rect_SingWindow;

	Text Text_Window;

	ActivePopup Active_Popup;

	string 
	Input_MailAddress_Path 	= "Content/InputMailAddress/",
	Input_Password_Path 	= "Content/InputPassWord/";


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
	//会員登録Popup初期化
	void Initialize_SingUp(){
		Obj_SingUpWindow.transform.Find (Input_MailAddress_Path + "InputField").GetComponent<InputField> ().text = "";
		Obj_SingUpWindow.transform.Find (Input_Password_Path 	+ "InputField").GetComponent<InputField> ().text = "";
		Obj_SingUpWindow.transform.Find (Input_MailAddress_Path + "ErrorText").	GetComponent<Text> ().text = "";
		Obj_SingUpWindow.transform.Find (Input_Password_Path 	+ "ErrorText").	GetComponent<Text> ().text = "";
	}
	//ログインPopup初期化
	void Initialize_SingIn (){
		Obj_SingInWindow.transform.Find (Input_MailAddress_Path + "InputField").GetComponent<InputField> ().text = "";
		Obj_SingInWindow.transform.Find (Input_Password_Path 	+ "InputField").GetComponent<InputField> ().text = "";
		Obj_SingInWindow.transform.Find (Input_MailAddress_Path + "ErrorText").	GetComponent<Text> ().text = "";
		Obj_SingInWindow.transform.Find (Input_Password_Path 	+ "ErrorText").	GetComponent<Text> ().text = "";
	}
	//アカウント情報確認
	void Login_Check(){
		if(Data_User.Mail_Address != "" && Data_User.Pass_Word != ""){
			//Popup設定
			Text_Window.text = string.Format ("{0}\n{1}\nでログイン認証中です", Data_User.Mail_Address, Data_User.User_Name);
			PopupAnimation (true, Rect_Window);
			Active_Popup = ActivePopup.Window;
			Login_Auto ();
		}
		else{
			PopupAnimation (true, Rect_SingWindow);
			Active_Popup = ActivePopup.SingWindow;
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
			RectTrns.gameObject.SetActive (true);
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
				});;
		}
	}


	/// <summary>
    	/// 指定された文字列がメールアドレスかどうかを返します
   	/// </summary>
	bool IsMailAddress (string input){
		if (string.IsNullOrEmpty (input)) {
			return false;
		}
		return Regex.IsMatch (input,@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$",RegexOptions.IgnoreCase);
	}

	/// <summary>
    	/// 指定された文字列がパスワードかどうかを返します
   	/// </summary>
	bool IsPassword (string input){
		if (input.Length < 6) {
			return false;
		} else {
			return true;
		}
	}

/**UGUI**/
//ボタン
	//ログインウィンドウ表示ボタン
	public void OnClick_SingUpWindowBtn(){
		PopupAnimation (false, Rect_SingWindow);
		PopupAnimation (true, Obj_SingUpWindow.GetComponent<RectTransform> ());
		Active_Popup = ActivePopup.SingUpInputWindow;
	}
	//会員登録ウィンドウ表示ボタン
	public void OnClick_SingInWindowBtn (){
		PopupAnimation (false, Rect_SingWindow);
		PopupAnimation (true, Obj_SingInWindow.GetComponent<RectTransform> ());
		Active_Popup = ActivePopup.SingInInputWindow;
	}
	//ウィンドウ閉じるボタン
	public void OnClick_CloseBtn(RectTransform ObjTrns){
		//Popupアニメーション
		PopupAnimation (false, ObjTrns);
		PopupAnimation (true, Rect_SingWindow);
		//表示していたPopupの初期化
		switch(Active_Popup){
		case ActivePopup.SingInInputWindow:
			Initialize_SingIn ();
			break;
		case ActivePopup.SingUpInputWindow:
			Initialize_SingUp ();
			break;
		}
		//現在表示しているPopupのenumを切り替え
		Active_Popup = ActivePopup.SingWindow;
	}

//インプットフィールド
	//メールアドレスの入力検知
	public void OnEndEdit_MailAddress (string text){
		if(IsMailAddress (text) == true){
			//エラー文の解除
			switch (Active_Popup) {
			case ActivePopup.SingInInputWindow:
				Obj_SingInWindow.transform.Find (Input_MailAddress_Path + "ErrorText").GetComponent<Text> ().text = "";
				break;
			case ActivePopup.SingUpInputWindow:
				Obj_SingUpWindow.transform.Find (Input_MailAddress_Path + "ErrorText").GetComponent<Text> ().text = "";
				break;
			}
			//メールアドレスの保存
			Data_User.Mail_Address = text;
			Save_Data_User.Save ();
		}else{
			//エラー文の表示
			switch(Active_Popup){
			case ActivePopup.SingInInputWindow:
				Obj_SingInWindow.transform.Find (Input_MailAddress_Path + "ErrorText").GetComponent<Text> ().text = "メールアドレスとして正しくありません";
				break;
			case ActivePopup.SingUpInputWindow:
				Obj_SingUpWindow.transform.Find (Input_MailAddress_Path + "ErrorText").GetComponent<Text> ().text = "メールアドレスとして正しくありません";
				break;
			}
		}
	}
	//パスワードの入力検知
	public void OnEndEdit_PassWord (string text){
		if (IsPassword (text) == true) {
			//エラー文の解除
			switch (Active_Popup) {
			case ActivePopup.SingInInputWindow:
				Obj_SingInWindow.transform.Find (Input_Password_Path + "ErrorText").GetComponent<Text> ().text = "";
				break;
			case ActivePopup.SingUpInputWindow:
				Obj_SingUpWindow.transform.Find (Input_Password_Path + "ErrorText").GetComponent<Text> ().text = "";
				break;
			}
			//パスワードの保存
			Data_User.Pass_Word = text;
			Save_Data_User.Save ();
		} else {
			//エラー文の表示
			switch (Active_Popup) {
			case ActivePopup.SingInInputWindow:
				Obj_SingInWindow.transform.Find (Input_Password_Path + "ErrorText").GetComponent<Text> ().text = "パスワードが正しくありません";
				break;
			case ActivePopup.SingUpInputWindow:
				Obj_SingUpWindow.transform.Find (Input_Password_Path + "ErrorText").GetComponent<Text> ().text = "パスワードが正しくありません";
				break;
			}
		}
	}
}
