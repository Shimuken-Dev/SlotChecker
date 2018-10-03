using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_Coop : MonoBehaviour {

	[SerializeField]
	GameObject
	SetUpPopup,
	StatusPopup,
	TempleteObj;

	[SerializeField]
	Transform ContentTrns;

	Manager_FadeCanvas FadeCanvasMng;
	Manager_MainCanvas MainCanvasMng;
	Manager_ConnectingCanvas ConnectingCanvasMng;
	Manager_MessageCanvas MessageCanvasMng;

	NCMBObject TeamObj;

	void Start () {
		Initialize ();
		StartCoroutine (FadeCanvasMng.FadeOut (null));
	}


//関数
	//初期化
	void Initialize(){
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		MainCanvasMng = GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
		ConnectingCanvasMng = GameObject.Find ("ConnectingCanvas").GetComponent<Manager_ConnectingCanvas> ();
		MessageCanvasMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
		TeamObj = null;
	}

	//チーム作成
	void PostCreateTeam(){
		string number = UnityEngine.Random.Range (0, 10).ToString() + UnityEngine.Random.Range (0, 10).ToString () + UnityEngine.Random.Range (0, 10).ToString () + UnityEngine.Random.Range (0, 10).ToString ();
		string className = "Team"+number;
		Debug.Log (className);
		NCMBObject NewTeamObj = new NCMBObject (className);
		NewTeamObj.Add ("player", Data_User.User_Name);
		NewTeamObj.Add ("playMachine", "null");
		NewTeamObj.Add ("usedMoney", 0);
		NewTeamObj.Add ("haveMedal", 0);
		NewTeamObj.Add ("haveBall", 0);
		NewTeamObj.SaveAsync ((NCMBException e) => {
			if (e != null) {
				//エラー処理
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "チーム作成に失敗しました");
			} else {
				//成功時の処理
				TeamObj = NewTeamObj;
				SetUpPopup.SetActive (false);
				StatusPopup.SetActive (true);
				CreatePlayer (null);
			}
		});
	}

	//プレイヤー作成
	void CreatePlayer(List<NCMBObject> PlayerList){
		if(PlayerList == null){
			//新規チーム作成時
			GameObject InsObj = Instantiate (TempleteObj);
			InsObj.transform.SetParent (ContentTrns, false);
			InsObj.name = TeamObj.ObjectId; 
			//プレイヤーアイコン
			int i = UnityEngine.Random.Range (0,5);
			InsObj.transform.Find ("Player/Icon").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/Coop/PlayerIcon/"+i.ToString());
			//プレイヤーネーム
			InsObj.transform.Find ("Player/Texts/NameText").GetComponent<Text> ().text = Data_User.User_Name;
			//投資金額
			InsObj.transform.Find ("Info/UsedMoneyText").GetComponent<Text> ().text = "投資金額: 0円";
			//持ちメダル数
			InsObj.transform.Find ("Info/HaveInfo/HaveMedalText").GetComponent<Text> ().text = "持ちメダル: 0枚";
			//持ち玉数
			InsObj.transform.Find ("Info/HaveInfo/HaveBallText").GetComponent<Text> ().text = "持ち玉: 0玉";
			//遊戯中の機種
			InsObj.transform.Find ("PlayMachineInfo/Text").GetComponent<Text> ().text = "遊戯中の機種: なし";
			InsObj.SetActive (true);
		}else{
			//既存チーム参加時
			foreach (NCMBObject obj in PlayerList) {
			}
		}
	}


//UGUI
	//閉じるボタン
	public void OnClick_CloseBtn(){
		if (TeamObj != null) {
			TeamObj.DeleteAsync ((NCMBException e) => {
				StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title)));
			});
		}else{
			StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title)));
		}
	}
	//チーム参加ボタン
	public void OnClick_SingInBtn(){
		
	}
	//チーム作成ボタン
	public void OnClick_CreateBtn(){
		PostCreateTeam ();
	}
}
