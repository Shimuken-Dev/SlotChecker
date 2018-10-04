using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NCMB;

public class Ctr_Title : MonoBehaviour {

	[SerializeField]
	RectTransform 
	StartArea_RectTrns,
	Menu_RectTrns;

	[SerializeField]
	Text 
	AppVer_text,
	UserName_text;

	[SerializeField]
	GameObject News_NewMarkObj;

	Manager_MainCanvas MainCanvasMng;
	Manager_SubCanvas SubCanvasMng;
	Manager_FadeCanvas FadeCanvasMng;

	static bool NewsFlg;


	void Start (){
		Initialize ();
		StartCoroutine (FadeCanvasMng.FadeOut (null));
	}


//UGUI
	//スタートボタン
	public void OnClick_StartBtn(){
		if (NewsFlg == false) {
			SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.News);
			NewsFlg = true;
		}
		Animation_Menu ();
	}
	//セッテイングボタン
	public void OnClick_SettingBtn(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.Setting);
	}
	//ニュースボタン
	public void OnClick_NewsBtn(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.News);
	}
 /*メニューのボタンたち*/
	//スロット一覧
	public void OnClick_SlotListBtn(){
		StartCoroutine(FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.SlotList)));
	}
	//ノリ打ち
	public void OnClick_CoopBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Coop)));
	}

//関数
	//初期化
	void Initialize(){
		MainCanvasMng = GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		AppVer_text.text = "アプリバージョン : " + Application.version;
		UserName_text.text = "アカウント : " + Data_User.User_Name;
	}

	//Newsの新着マーク処理
	public void NewMark_News(){
		//登録されてるニュースの数
		int NewsCount = 0;
		NCMBQuery<NCMBObject> NewsQuery = new NCMBQuery<NCMBObject> ("News");
		NewsQuery.CountAsync ((int count, NCMBException e) => {
			if (e != null) {
				//件数取得失敗時の処理
				News_NewMarkObj.SetActive (false);
			} else {
				//件数を出力
				NewsCount = count;
				//読んだニュースの数
				int ReadNews = 0;
				NCMBUser currentUser = NCMBUser.CurrentUser;
				ReadNews = CommonFunctionsCtr.CountChar (currentUser ["read_news"].ToString (), ',') + 1;
				//新着マークの管理
				if (ReadNews == NewsCount) {
					if (GameObject.Find ("Title") == true) {
						News_NewMarkObj.SetActive (false);
					}
				} else {
					if (GameObject.Find ("Title") == true) {
						News_NewMarkObj.SetActive (true);
					}
				}
			}
		});
	}

	//メニューウィンドウアニメーション
	void Animation_Menu(){
		Menu_RectTrns.DOScale (Vector3.one, 0.25f);
		StartArea_RectTrns.DOScale (Vector3.zero, 0.25f);
	}
}
