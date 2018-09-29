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

	Manager_ConnectingCanvas ConnectingMng;
	Manager_MainCanvas MainCanvasMng;
	Manager_FadeCanvas FadeCanvasMng;


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
			//InsObj.transform.Find ("Thum").GetComponent<Image> ().sprite = null;
			InsObj.transform.Find ("TypeBand/TypeText").GetComponent<Text> ().text = obj ["type"].ToString ();
			InsObj.transform.Find ("Info/Name").GetComponent<Text> ().text = obj ["name"].ToString ();
			InsObj.transform.Find ("Info/Maker").GetComponent<Text> ().text = obj ["maker"].ToString ();
			InsObj.transform.Find ("Info/ReleaseDay").GetComponent<Text> ().text = obj ["releaseDate"].ToString ();
			InsObj.SetActive (true);
		}
		//通信完了
		ConnectingMng.Stop_Connecting ();
		StartCoroutine(FadeCanvasMng.FadeOut (null));
	}


//UGUI
	//閉じるボタン
	public void OnClick_CloseBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title)));
	}
}
