using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMailerCtr : MonoBehaviour {
	//メール
	static string MAIL_ADRESS = "shimuken.developer@gmail.com";
	static string CAUTION_STATEMENT = "---------以下の内容はそのままで---------\n";

	// <summary>
	// メーラーを起動する
	// </summary>
	public static void Open (){
		//タイトルはアプリ名
		string subject = Application.productName;
		//本文は端末名、OS、アプリバージョン、言語
		string body = "\n\n" + CAUTION_STATEMENT + "\n";
		body += "Device: " 	+ SystemInfo.deviceName + "\n";
		body += "OS: " 		+ SystemInfo.operatingSystem + "\n";
		body += "Ver: " 	+ Application.version + "\n";
		body += "Language: " 	+ Application.systemLanguage.ToString () + "\n";
		//エスケープ処理
		string Escape_Subject = WWW.EscapeURL (subject);
		string Escape_Body = WWW.EscapeURL (body);
		string url = string.Format ("mailto:{0}?subject={1}&body={2}", MAIL_ADRESS, Escape_Subject, Escape_Body);
		//起動
		Application.OpenURL (url);
	}
}