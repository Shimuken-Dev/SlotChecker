using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_MiniGame : MonoBehaviour {

	[SerializeField]
	Text Text_Point;

	[SerializeField]
	Transform Trns_Content;

	public GameObject Obj_Templete;

	Manager_MainCanvas MainCanvasMng;
	Manager_FadeCanvas FadeCanvasMng;
	Manager_MessageCanvas MessageCanvasMng;

	NCMBUser currentUser;
	

	void Start (){
		Initialize ();
		View_Initialize ();
		GetContent ();
	}

//関数

	//初期化
	void Initialize(){
		MainCanvasMng = GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		MessageCanvasMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
		currentUser = NCMBUser.CurrentUser;
	}
	//描画初期化
	void View_Initialize(){
		Text_Point.text = Data_User.Point.ToString();
	}

	//コンテンツ取得通信
	void GetContent(){
		//ミニゲーム一覧
		NCMBQuery<NCMBObject> MiniGameList = new NCMBQuery<NCMBObject> ("MiniGame");
		MiniGameList.FindAsync ((List<NCMBObject> objList, NCMBException ListExcep) => {
			if (ListExcep != null) {
				//検索失敗時の処理
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
				OnClick_CloseBtn ();
			} else {
				//検索成功時の処理
				CreateMiniGame (objList);
			}
		});
	}
	//コンテンツ作成
	void CreateMiniGame(List<NCMBObject> MiniGameList){
		foreach (NCMBObject obj in MiniGameList) {
			/** 生成 **/
			GameObject InsObj = Instantiate (Obj_Templete);
			InsObj.transform.SetParent (Trns_Content, false);
			InsObj.name = obj.ObjectId;
			/** コンテンツ更新 **/
			InsObj.transform.Find ("DescriText").GetComponent<Text> ().text = obj ["description"].ToString ();
			/** タイトルロゴ **/
			if (obj ["titleLogo"].ToString () != null) {
				GetImage (obj ["titleLogo"].ToString (), InsObj.transform.Find ("TitleLogo").GetComponent<Image> (),450f,150f);
			} else {
				InsObj.transform.Find ("Thum").GetComponent<Image> ().sprite = null;
			}
			/** サムネイル **/
			if (obj ["thumbnail1"].ToString () != null) {
			}
			if (obj ["thumbnail2"].ToString () != null) {
			}
			if (obj ["thumbnail3"].ToString () != null) {
			}
			/** 解放確認 **/
			if(obj["price"].ToString() == "0"){
				Destroy(InsObj.transform.Find ("Lock").gameObject);
			}else{
				string[] id = currentUser ["buy_minigame_id"].ToString ().Split(',');
				for (int i = 0; i < id.Length; i++){
					if(id[i] == obj.ObjectId){
						Destroy (InsObj.transform.Find ("Lock").gameObject);
					}
				}
			}
			/** メンテナンス **/
			if(obj["maintenance"].ToString() == "False"){
				Destroy (InsObj.transform.Find ("Maintenance").gameObject);
			}
		}
		Destroy (Obj_Templete);
		StartCoroutine (FadeCanvasMng.FadeOut (null));
	}
	//画像取得
	void GetImage (string file_name, Image image,float width,float height){
		NCMBFile file = new NCMBFile (file_name);
		file.FetchAsync ((byte [] fileData, NCMBException error) => {
			if (error != null) {
				// 失敗
				image.sprite = null;
			} else {
				// 成功
				image.sprite = Sprite.Create (CommonFunctionsCtr.CreateTextureFormBytes (fileData), new Rect (0, 0, width, height), Vector2.zero);
			}
		});
	}
//UGUI

	//閉じるボタン
	public void OnClick_CloseBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title)));
	}
}
