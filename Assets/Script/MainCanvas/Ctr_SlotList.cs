using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_SlotList : MonoBehaviour {

	[SerializeField]
	GameObject TempleteObj;

	[SerializeField]
	Transform ContentTrns;

	[SerializeField]
	Sprite NoImage_Spr;

	Manager_ConnectingCanvas ConnectingMng;
	Manager_MainCanvas MainCanvasMng;
	Manager_FadeCanvas FadeCanvasMng;
	Manager_SubCanvas SubCanvasMng;

	void Start (){
		Initialized ();
		GetSlotList ();
	}


//関数
	//初期化
	void Initialized (){
		ConnectingMng = GameObject.Find ("ConnectingCanvas").GetComponent<Manager_ConnectingCanvas> ();
		MainCanvasMng = GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
	}

	//スロット一覧取得
	void GetSlotList(){
		//通信中表示
		ConnectingMng.Start_Connecting ();
		//スロット一覧
		NCMBQuery<NCMBObject> SlotList = new NCMBQuery<NCMBObject> ("SlotList");
		SlotList.FindAsync ((List<NCMBObject> objList, NCMBException ListExcep) => {
			if (ListExcep != null) {
				//検索失敗時の処理
				ConnectingMng.Stop_Connecting ();
				OnClick_CloseBtn ();
			} else {
				//検索成功時の処理
				CreateSlotContent (objList);
			}
		});
	}

	//コンテンツ作成
	void CreateSlotContent(List<NCMBObject> SlotList){
		foreach(NCMBObject obj in SlotList){
			//生成
			GameObject InsObj = Instantiate (TempleteObj);
			InsObj.transform.SetParent (ContentTrns, false);
			//コンテンツ表示
			InsObj.name = obj.ObjectId;
			if(obj["thumbnail"].ToString() != null){
				GetThumbnail (obj ["thumbnail"].ToString (), InsObj.transform.Find ("Thum").GetComponent<Image> ());
			}else{
				InsObj.transform.Find ("Thum").GetComponent<Image> ().sprite = NoImage_Spr;
			}

			InsObj.transform.Find ("TypeBand/TypeText").GetComponent<Text> ().text = obj ["type"].ToString ();
			InsObj.transform.Find ("Info/Name").GetComponent<Text> ().text = obj ["name"].ToString ();
			InsObj.transform.Find ("Info/Maker").GetComponent<Text> ().text = obj ["maker"].ToString ();
			InsObj.transform.Find ("ComingSoon").gameObject.SetActive (bool.Parse (obj ["comingSoon"].ToString ()));
			DateTime ReleaseTime = DateTime.Parse (obj ["releaseDate"].ToString ());
			InsObj.transform.Find ("Info/ReleaseDay").GetComponent<Text> ().text = string.Format ("{0}年{1}月{2}日",ReleaseTime.Year,ReleaseTime.Month,ReleaseTime.Day);
			InsObj.SetActive (true);
		}
		//通信完了
		ConnectingMng.Stop_Connecting ();
		StartCoroutine(FadeCanvasMng.FadeOut (null));
	}

	//サムネイル画像取得
	void GetThumbnail(string file_name,Image image){
		NCMBFile file = new NCMBFile (file_name);
		file.FetchAsync ((byte [] fileData, NCMBException error) => {
			if (error != null) {
				// 失敗
				image.sprite = NoImage_Spr;
			} else {
				// 成功
				image.sprite = Sprite.Create (CommonFunctionsCtr.CreateTextureFormBytes (fileData), new Rect (0, 0, 200, 300), Vector2.zero);
			}
		});
	}

//UGUI
	//閉じるボタン
	public void OnClick_CloseBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title)));
	}

	//台選択ボタン
	public void OnClick_ChoiceBtn(GameObject Obj){
		Ctr_SlotListMenu.ChoiceMachine = Obj.name;
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.SlotListMenu);
	}
}
