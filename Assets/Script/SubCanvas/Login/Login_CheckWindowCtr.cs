using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NCMB;

public class Login_CheckWindowCtr : MonoBehaviour {

	RectTransform RectTrns;

	public enum BeforePopup{
		SingUpInputWindow,
		SingInInputWindow
	}
	public BeforePopup Before_Popup;

	Ctr_Login LoginCtr;
	Login_SingInInputWindowCtr SingInInputWindowCtr;
	Login_SingUpInputWindowCtr SingUpInputWindowCtr;
	Login_DontTouchWindowCtr DontTouchWindowCtr;
	Login_WarningWindow 	WarningWindowCtr;


	void Start (){
		Initialize ();
	}


	/// <summary>
	/// Public関数
	/// </summary>
	//開く
	public void OpenWindow (){
		Initialize_Text ();
		PopupAnimation (true);
	}
	//閉じる
	public void CloseWindow (){
		PopupAnimation (false);
	}


	/// <summary>
	/// UGUI
	/// </summary>
	//閉じるボタン
	public void OnClick_CloseBtn(){
		switch(Before_Popup){
		case BeforePopup.SingInInputWindow:
			LoginCtr.Active_Popup = Ctr_Login.ActivePopup.SingInInputWindow;
			SingInInputWindowCtr.OpenWindow ();
			break;
		case BeforePopup.SingUpInputWindow:
			LoginCtr.Active_Popup = Ctr_Login.ActivePopup.SingUpInputWindow;
			SingUpInputWindowCtr.OpenWindow ();
			break;
		}
		CloseWindow ();
	}
	//送信ボタン
	public void OnClick_PushBtn(){
		DontTouchWindowCtr.OpenWindow ("サーバーに情報を送信中");
		switch (Before_Popup) {
		case BeforePopup.SingInInputWindow:
			SingIn ();
			break;
		case BeforePopup.SingUpInputWindow:
			SingUp ();
			break;
		}
	}


	/// <summary>
	/// 関数
	/// </summary>
	//初期化
	void Initialize (){
		RectTrns = gameObject.GetComponent<RectTransform> ();
		LoginCtr = gameObject.transform.parent.GetComponent<Ctr_Login> ();
		SingInInputWindowCtr = gameObject.transform.parent.Find ("SingInInputWindow").GetComponent<Login_SingInInputWindowCtr> ();
		SingUpInputWindowCtr = gameObject.transform.parent.Find ("SingUpInputWindow").GetComponent<Login_SingUpInputWindowCtr> ();
		DontTouchWindowCtr = gameObject.transform.parent.Find ("DontTouchObj").GetComponent<Login_DontTouchWindowCtr> ();
		WarningWindowCtr = gameObject.transform.parent.Find ("WarningWindow").GetComponent<Login_WarningWindow> ();
	}
	//表示テキストを初期化
	void Initialize_Text(){
		gameObject.transform.Find ("Content/UserName/ContentText").GetComponent<Text> ().text = Data_User.User_Name;
		gameObject.transform.Find ("Content/UserMailAddress/ContentText").GetComponent<Text> ().text = Data_User.Mail_Address;
		gameObject.transform.Find ("Content/UserPassword/ContentText").GetComponent<Text> ().text = Data_User.Pass_Word;
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
				});
		}
	}
	//新規ユーザー会員作成リクエスト
	void SingUp(){
		//Userインスタンスの生成
		NCMBUser user = new NCMBUser ();
		// ユーザー名・パスワードを設定
		user.UserName = Data_User.User_Name; 		/* ユーザー名 */
		user.Password = Data_User.Pass_Word; 		/* パスワード */
		user.Email = Data_User.Mail_Address; 		/* メールアドレス */
		user.Add ("read_news", "");			/* 既読ニュース*/
		user.Add ("deviceId", Data_User.Device_ID); 	/* 端末ID */
		user.Add ("tester", false); 			/* テスター */
		// ユーザーの新規登録処理
		user.SignUpAsync ((NCMBException SingUp_Excep) => {
			if (SingUp_Excep != null) {
				DontTouchWindowCtr.CloseWindow ();
				WarningWindowCtr.OpenWindow ("新規登録に失敗しました\n入力情報を見直して頂くか\n通信環境の良い場所で再度お試しください\n"+SingUp_Excep.ErrorMessage, Login_WarningWindow.ButtonType.Close);
			} else {
				DontTouchWindowCtr.CloseWindow ();
				Debug.Log ("ユーザーの新規登録に成功");
				NCMBUser currentUser = NCMBUser.CurrentUser;
				Data_User.User_Name = currentUser.UserName;
				Data_User.Mail_Address = currentUser.Email;
				Data_User.Device_ID = currentUser ["deviceId"].ToString ();
				Data_User.Tester = currentUser ["tester"].ToString ();
				Save_Data_User.Save ();
				LoginCtr.Finish_Login ("登録されたメールアドレスに\n確認メールを送信致しました\n確認メール内にある\nリンクを押して登録アドレスを\n有効にして下さい");
			}
		});
	}
	//ログインリクエスト
	void SingIn(){
		NCMBUser.LogInAsync (Data_User.User_Name, Data_User.Pass_Word, (NCMBException e) => {
			if (e != null) {
				DontTouchWindowCtr.CloseWindow ();
				WarningWindowCtr.OpenWindow ("ログインに失敗しました\n入力情報を見直して頂くか\n通信環境の良い場所で再度お試しください\n" + e.ErrorMessage, Login_WarningWindow.ButtonType.Close);
			} else {
				DontTouchWindowCtr.CloseWindow ();
				Debug.Log ("ログインに成功！");
				NCMBUser currentUser = NCMBUser.CurrentUser;
				Data_User.User_Name = currentUser.UserName;
				Data_User.Mail_Address = currentUser.Email;
				Data_User.Device_ID = currentUser ["deviceId"].ToString ();
				Data_User.Tester = currentUser ["tester"].ToString ();
				Save_Data_User.Save ();
				LoginCtr.Finish_Login ("ログインが完了しました");
			}
		});
	}
}
