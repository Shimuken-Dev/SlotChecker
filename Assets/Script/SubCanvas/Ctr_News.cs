using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_News : MonoBehaviour {

	[SerializeField]
	GameObject
	MoreDetailNews_obj,
	NewsTemp_obj;

	[SerializeField]
	Transform Content_trns;

	Manager_ConnectingCanvas ConnectingMng;
	Manager_SubCanvas SubCanvasMng;

	Dictionary<string, string> MoreDetailNews_main;
	NCMBQuery<NCMBObject> NewsQuery;

	void Start(){
		Initialize ();
		GetNews ();
	}

//関数
	//初期化
	void Initialize(){
		ConnectingMng = GameObject.Find ("ConnectingCanvas").GetComponent<Manager_ConnectingCanvas> ();
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		MoreDetailNews_main = new Dictionary<string, string> ();
	}
	//ニュース取得
	void GetNews(){
		//通信中表示
		ConnectingMng.Start_Connecting ();
		//News情報を取得
		NewsQuery = new NCMBQuery<NCMBObject> ("News");
		NewsQuery.FindAsync ((List<NCMBObject> objList, NCMBException ListExcep) => {
			if (ListExcep != null) {
				//検索失敗時の処理
			} else {
				//検索成功時の処理
				if(objList.Count > 0){
					CreateNewsContent (objList);
				}
			}
		});
	}
	//ニュース作成
	void CreateNewsContent(List<NCMBObject> NewsList){
		foreach (NCMBObject obj in NewsList) {
			//生成
			GameObject InsObj = Instantiate (NewsTemp_obj);
			InsObj.transform.SetParent (Content_trns, false);
			//コンテンツ表示切り替え
			InsObj.name = obj.ObjectId;
			InsObj.transform.Find ("TextContent/TitleText").GetComponent<Text> ().text = obj ["title"].ToString ();
			InsObj.transform.Find ("TextContent/DayText").GetComponent<Text> ().text = obj.UpdateDate.ToString ();
			InsObj.SetActive (true);
			//dictionaryへObject IDと本文を格納
			MoreDetailNews_main.Add (obj.ObjectId, obj ["main"].ToString ());
		}
		//通信完了
		ConnectingMng.Stop_Connecting ();
	}

//UGUI
	//Popup閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
	//閉じるボタン
	public void OnClick_MoreDetailCloseBtn(){
		MoreDetailNews_obj.SetActive (false);
	}
	//詳細ボタン
	public void OnClick_MoreDetailNewsBtn(GameObject Obj){
		//TODO banner画像がまだ
		string text = CommonFunctionsCtr.UnEscape(MoreDetailNews_main [Obj.name]);
		string title = Obj.transform.Find ("TextContent/TitleText").GetComponent<Text> ().text;
		string Day = Obj.transform.Find ("TextContent/DayText").GetComponent<Text> ().text;

		MoreDetailNews_obj.transform.Find ("Header/TextContent/TitleText").GetComponent<Text> ().text = title;
		MoreDetailNews_obj.transform.Find("Header/TextContent/DayText").GetComponent<Text>().text = Day;
		MoreDetailNews_obj.transform.Find ("View/MainText").GetComponent<Text> ().text = text;
		MoreDetailNews_obj.SetActive (true);
	}
}