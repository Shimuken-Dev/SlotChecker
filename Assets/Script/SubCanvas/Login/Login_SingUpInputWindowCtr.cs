using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Login_SingUpInputWindowCtr : MonoBehaviour {

	RectTransform RectTrns;

	Login_SingWindowCtr SingWindowCtr;
	Login_CheckWindowCtr CheckWindowCtr;
	Ctr_Login LoginCtr;

	void Start (){
		Initialize ();
	}


	/// <summary>
	/// Public関数
	/// </summary>
	//開く
	public void OpenWindow (){
		PopupAnimation (true);
	}
	//閉じる
	public void CloseWindow (){
		PopupAnimation (false);
	}


	/// <summary>
	/// UGUI
	/// </summary>
	//ユーザーネームの入力検知
	public void OnEndEdit_UserName(string text){
		Data_User.User_Name = text;
	}
	//メールアドレスの入力検知
	public void OnEndEdit_MailAddress (string text){
		if(IsMailAddress (text) == true){
			//エラー文の解除
			gameObject.transform.Find ("Content/InputMailAddress/ErrorText").GetComponent<Text> ().text = "";
			//メールアドレスの保存
			Data_User.Mail_Address = text;
		}else{
			//エラー文の表示
			gameObject.transform.Find ("Content/InputMailAddress/ErrorText").GetComponent<Text> ().text = "メールアドレスとして正しくありません";
		}
	}
	//パスワードの入力検知
	public void OnEndEdit_PassWord (string text){
		if (IsPassword (text) == true) {
			//エラー文の解除
			gameObject.transform.Find ("Content/InputPassWord/ErrorText").GetComponent<Text> ().text = "";
			//パスワードの保存
			Data_User.Pass_Word = text;
		} else {
			//エラー文の表示
			gameObject.transform.Find ("Content/InputPassWord/ErrorText").GetComponent<Text> ().text = "パスワードが正しくありません";
		}
	}
	//閉じるボタン
	public void OnClick_CloseBtn(){
		LoginCtr.Active_Popup = Ctr_Login.ActivePopup.SingWindow;
		SingWindowCtr.OpenWindow ();
		CloseWindow ();
		Initialize_Input ();
	}
	//確認ボタン
	public void OnClick_CheckInputBtn(){
		LoginCtr.Active_Popup = Ctr_Login.ActivePopup.CheckWindow;
		CloseWindow ();
		CheckWindowCtr.Before_Popup = Login_CheckWindowCtr.BeforePopup.SingUpInputWindow;
		CheckWindowCtr.OpenWindow ();
	}


	/// <summary>
	/// 関数
	/// </summary>
	//初期化
	void Initialize (){
		RectTrns = gameObject.GetComponent<RectTransform> ();
		SingWindowCtr = gameObject.transform.parent.Find ("SingWindow").GetComponent<Login_SingWindowCtr> ();
		LoginCtr = gameObject.transform.parent.GetComponent<Ctr_Login> ();
		CheckWindowCtr = gameObject.transform.parent.Find ("CheckWindow").GetComponent<Login_CheckWindowCtr> ();
	}
	//入力まわりを初期化
	void Initialize_Input (){
		gameObject.transform.Find ("Content/InputMailAddress/ErrorText").GetComponent<Text> ().text = "";
		gameObject.transform.Find ("Content/InputPassWord/ErrorText").GetComponent<Text> ().text = "";
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
}
