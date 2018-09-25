using System.Linq;
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
	Dictionary<string, string> News_Banner;
	NCMBQuery<NCMBObject> NewsQuery;
	NCMBUser currentUser;
	List<string> ReadNews_list;

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
		News_Banner = new Dictionary<string, string> ();
		ReadNews_list = new List<string> ();
		currentUser = NCMBUser.CurrentUser;
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
					//新着ニュース管理
					string read_news = currentUser ["read_news"].ToString();
					if (read_news != "") {
						string [] read_news_hash = read_news.Split (',');
						ReadNews_list.AddRange (read_news_hash);
					}
					//コンテンツ作成
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
			//新着マーク
			if (ReadNews_list.Contains (obj.ObjectId) == true){ // ObjectIDと一致するものがあるかどうか確認する
				// 存在する
				InsObj.transform.Find ("NewMarkImg").gameObject.SetActive (false);
			}
			InsObj.SetActive (true);
			//dictionaryへObject IDと本文を格納
			MoreDetailNews_main.Add (obj.ObjectId, obj ["main"].ToString ());
			//バナー画像を格納
			if (obj ["main_banner"].ToString () != "") {
				News_Banner.Add (obj.ObjectId, obj ["main_banner"].ToString ());
			}
		}
		//通信完了
		ConnectingMng.Stop_Connecting ();
	}
	//既読チェック
	void PushNewsID(){
		string id = "";

		for (int i = 0; i < ReadNews_list.Count (); i++) {
			var data = ReadNews_list [i];
		
			if (i != ReadNews_list.Count) {
				id += data + ",";
			} else {
				id += data;
			}
		}

		currentUser ["read_news"] = id;
		currentUser.Save ();
	}
	//バナー画像表示
	void GetBannerImage(string file_name){
		NCMBFile file = new NCMBFile (file_name);
		file.FetchAsync ((byte [] fileData, NCMBException error) => {
			if (error != null) {
				// 失敗
				MoreDetailNews_obj.transform.Find ("View/BannerImg").gameObject.SetActive (false);
			} else {
				// 成功
				MoreDetailNews_obj.transform.Find ("View/BannerImg").GetComponent<Image>().sprite = CommonFunctionsCtr.CreateSpriteFromBytes (fileData);
			}
		});
	}

//UGUI
	//Popup閉じるボタン
	public void OnClick_CloseBtn(){
		PushNewsID ();
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
	//詳細を閉じるボタン
	public void OnClick_MoreDetailCloseBtn(){
		MoreDetailNews_obj.SetActive (false);
	}
	//詳細ボタン
	public void OnClick_MoreDetailNewsBtn(GameObject Obj){
		//新着マーク管理
		if (ReadNews_list.Contains(Obj.name) == false) { // ObjectIDと一致するものがあるかどうか確認する
			// 存在しない
			Obj.transform.Find ("NewMarkImg").gameObject.SetActive (false);
			ReadNews_list.Add (Obj.name);
		}
		//テキストコンテンツ
		string text = CommonFunctionsCtr.UnEscape(MoreDetailNews_main [Obj.name]);
		string title = Obj.transform.Find ("TextContent/TitleText").GetComponent<Text> ().text;
		string Day = Obj.transform.Find ("TextContent/DayText").GetComponent<Text> ().text;
		//バナー画像
		if(News_Banner.ContainsKey(Obj.name) == true){
			MoreDetailNews_obj.transform.Find ("View/BannerImg").gameObject.SetActive (true);
			GetBannerImage (News_Banner[Obj.name]);
		}else{
			MoreDetailNews_obj.transform.Find ("View/BannerImg").gameObject.SetActive (false);
		}
		//View反映
		MoreDetailNews_obj.transform.Find ("Header/TextContent/TitleText").GetComponent<Text> ().text = title;
		MoreDetailNews_obj.transform.Find("Header/TextContent/DayText").GetComponent<Text>().text = Day;
		MoreDetailNews_obj.transform.Find ("View/MainText").GetComponent<Text> ().text = text;
		MoreDetailNews_obj.SetActive (true);
	}
}