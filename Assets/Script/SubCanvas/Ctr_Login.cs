using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NCMB;

public class Ctr_Login : MonoBehaviour {

	public enum ActivePopup{
		DontTouchWindow,
		WarningWindow,
		SingWindow,
		SingUpInputWindow,
		SingInInputWindow,
		CheckWindow
	}


	Login_SingWindowCtr 		SingWindowCtr;
	Login_SingInInputWindowCtr 	SingInCtr;
	Login_DontTouchWindowCtr 	DontTouchWindowCtr;
	Login_WarningWindow 		WarningWindowCtr;

	public ActivePopup Active_Popup;


	void Start (){
		Initialize ();
		Login_Check ();
	}


//public関数
	public void Finish_Login(string text){
		WarningWindowCtr.OpenWindow (text, Login_WarningWindow.ButtonType.Fnish);
	}
//関数
	//初期化
	void Initialize (){
		SingWindowCtr 		= gameObject.transform.Find ("SingWindow").GetComponent<Login_SingWindowCtr> ();
		SingInCtr 		= gameObject.transform.Find ("SingInInputWindow").GetComponent<Login_SingInInputWindowCtr> ();
		DontTouchWindowCtr 	= gameObject.transform.Find ("DontTouchObj").GetComponent<Login_DontTouchWindowCtr> ();
		WarningWindowCtr	= gameObject.transform.Find ("WarningWindow").GetComponent<Login_WarningWindow> ();
	}
	//自動ログインが出来るか
	void Login_Check (){
		if (Data_User.User_Name != "" && Data_User.Pass_Word != "") {
			//Popup設定
			string text = string.Format ("{0}で\nログイン中です",Data_User.User_Name);
			DontTouchWindowCtr.OpenWindow (text);
			Active_Popup = ActivePopup.DontTouchWindow;
			Login_Auto ();
		} else {
			SingWindowCtr.OpenWindow ();
			Active_Popup = ActivePopup.SingWindow;
		}
	}
	//自動ログイン
	void Login_Auto (){
		NCMBUser.LogInAsync (Data_User.User_Name, Data_User.Pass_Word, (NCMBException e) => {
			if (e != null) {
				DontTouchWindowCtr.CloseWindow ();
				WarningWindowCtr.OpenWindow ("ログインに失敗した為\nお手数ではございますが\n再ログインをお願い致します", Login_WarningWindow.ButtonType.Close);
				Active_Popup = ActivePopup.SingWindow;
				SingInCtr.OpenWindow ();
			} else {
				DontTouchWindowCtr.CloseWindow ();
				//現在のユーザ情報を取得してローカルデータを更新
				NCMBUser currentUser = NCMBUser.CurrentUser;
				Data_User.User_Name = currentUser.UserName;
				Data_User.Mail_Address = currentUser.Email;
				Data_User.Device_ID = currentUser ["deviceId"].ToString ();
				Data_User.Tester = currentUser ["tester"].ToString ();
				Save_Data_User.Save ();
				Finish_Login ("ログインが完了しました");
			}
		});
	}
}