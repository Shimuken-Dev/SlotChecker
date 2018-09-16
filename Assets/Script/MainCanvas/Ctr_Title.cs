using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ctr_Title : MonoBehaviour {

	[SerializeField]
	RectTransform 
	StartArea_RectTrns,
	Menu_RectTrns;

	[SerializeField]
	Text 
	AppVer_text,
	UserName_text;


	Manager_SubCanvas SubCanvasMng;


	void Start (){
		Initialize ();
	}


//UGUI
	//スタートボタン
	public void OnClick_StartBtn(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.News);
		Animation_Menu ();
	}
	//セッテイングボタン
	public void OnClick_SettingBtn(){
		
	}
 	/*メニューのボタンたち*/
	//

//関数
	//初期化
	void Initialize(){
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		AppVer_text.text = "アプリバージョン : " + Application.version;
		UserName_text.text = "アカウント : " + Data_User.User_Name;
	}
	//メニューウィンドウアニメーション
	void Animation_Menu(){
		Menu_RectTrns.DOScale (Vector3.one, 0.25f);
		StartArea_RectTrns.DOScale (Vector3.zero, 0.25f);
	}
}
