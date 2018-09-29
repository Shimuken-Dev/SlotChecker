using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_Setting : MonoBehaviour {

	[SerializeField]
	Text
	UserName_Text,
	MailAddress_Text;

	Manager_SubCanvas SubCanvasMng;
	Manager_MessageCanvas MessageCanvasMng;

	void Start(){
		Initialized ();
	}


//関数
	//初期化
	void Initialized (){
		UserName_Text.text = Data_User.User_Name;
		MailAddress_Text.text = Data_User.Mail_Address;
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		MessageCanvasMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
	}


//UGUI
	//閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
	//パスワード変更ボタン
	public void OnClick_PasswordChangeBtn(){
		NCMBUser.RequestPasswordResetAsync (Data_User.Mail_Address, (error) => {
			if (error != null) {
				// エラー処理
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "サーバー通信でエラーが発生しました");
			} else {
				// 送信後処理
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "登録されているメールアドレス宛に\nメールを送信致しました\nそちらのメールから変更を行って下さい");
			}
		});

	}
	//再ログインボタン
	public void OnClick_ReLoginBtn(){
		NCMBUser.LogOutAsync ();
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.Login);
	}
	//お問い合わせボタン
	public void OnClick_ContactBtn(){
		OpenMailerCtr.Open ();
	}
}
