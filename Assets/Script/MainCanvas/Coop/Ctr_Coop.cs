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
	TempleteObj,
	BackBtnObj,
	CloseBtnObj;

	[SerializeField]
	Transform ContentTrns;

	[SerializeField]
	Text HeaderText;

	Manager_FadeCanvas FadeCanvasMng;
	Manager_MainCanvas MainCanvasMng;
	Manager_ConnectingCanvas ConnectingCanvasMng;
	Manager_MessageCanvas MessageCanvasMng;

	Ctr_Coop_InputPopup Input_Coop_InputPopup;

	public string number;

	string className;

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
		Input_Coop_InputPopup = gameObject.transform.Find ("InputPopup").GetComponent<Ctr_Coop_InputPopup> ();
		className = "";
		number = "";
	}

	//ヘッダーコントローラー
	void HeaderCtr(){
		if(SetUpPopup.activeSelf == true){
			BackBtnObj.SetActive (false);
			CloseBtnObj.SetActive (true);
		}else if(StatusPopup.activeSelf == true){
			BackBtnObj.SetActive (true);
			CloseBtnObj.SetActive (false);
		}

	}

	//チーム作成
	void PostCreateTeam(){
		ConnectingCanvasMng.Start_Connecting ();
		number = UnityEngine.Random.Range (0, 10).ToString() + UnityEngine.Random.Range (0, 10).ToString () + UnityEngine.Random.Range (0, 10).ToString () + UnityEngine.Random.Range (0, 10).ToString ();
		className = "Team"+number;
		NCMBObject NewTeamObj = new NCMBObject (className);
		NewTeamObj.Add ("player", Data_User.User_Name);
		NewTeamObj.Add ("playMachine", "なし");
		NewTeamObj.Add ("usedMoney", 0);
		NewTeamObj.Add ("haveMedal", 0);
		NewTeamObj.Add ("haveBall", 0);
		NewTeamObj.SaveAsync ((NCMBException e) => {
			if (e != null) {
				//エラー処理
				ConnectingCanvasMng.Stop_Connecting ();
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "チーム作成に失敗しました");
			} else {
				/*成功時の処理*/
				//ヘッダーとPopupを切り替え
				SetUpPopup.SetActive (false);
				StatusPopup.SetActive (true);
				HeaderCtr ();
				HeaderText.text = className;
				//コンテンツ作成
				CreatePlayer (null,NewTeamObj.ObjectId);
				ConnectingCanvasMng.Stop_Connecting ();
			}
		});
	}

	//プレイヤー作成
	void CreatePlayer(List<NCMBObject> PlayerList,string id = null){
		if(PlayerList == null){
			//新規チーム作成時
			GameObject InsObj = Instantiate (TempleteObj);
			InsObj.transform.SetParent (ContentTrns, false);
			InsObj.name = id;
			//プレイヤーアイコン
			int i = UnityEngine.Random.Range (0,5);
			InsObj.transform.Find ("Player/Icon").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/Coop/PlayerIcon/"+i.ToString());
			//プレイヤーネーム
			InsObj.transform.Find ("Player/Texts/NameText").GetComponent<Text> ().text = Data_User.User_Name;
			//称号
			int range = UnityEngine.Random.Range (0, 4);
			InsObj.transform.Find ("Player/Texts/TitileImg").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/Coop/PlayerTitle/" + range.ToString ());
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
				GameObject InsObj = Instantiate (TempleteObj);
				InsObj.transform.SetParent (ContentTrns, false);
				InsObj.name = obj.ObjectId;
				//プレイヤーアイコン
				int i = UnityEngine.Random.Range (0, 5);
				InsObj.transform.Find ("Player/Icon").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/Coop/PlayerIcon/" + i.ToString ());
				//プレイヤーネーム
				InsObj.transform.Find ("Player/Texts/NameText").GetComponent<Text> ().text = obj["player"].ToString();
				//称号
				int range = UnityEngine.Random.Range (0, 4);
				InsObj.transform.Find ("Player/Texts/TitileImg").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/Coop/PlayerTitle/" + range.ToString ());
				//投資金額
				InsObj.transform.Find ("Info/UsedMoneyText").GetComponent<Text> ().text = string.Format("投資金額: {0}円",obj["usedMoney"]);
				//持ちメダル数
				InsObj.transform.Find ("Info/HaveInfo/HaveMedalText").GetComponent<Text> ().text = string.Format("持ちメダル: {0}枚",obj ["haveMedal"]);
				//持ち玉数
				InsObj.transform.Find ("Info/HaveInfo/HaveBallText").GetComponent<Text> ().text = string.Format("持ち玉: {0}玉",obj ["haveBall"]);
				//遊戯中の機種
				InsObj.transform.Find ("PlayMachineInfo/Text").GetComponent<Text> ().text = string.Format("遊戯中の機種: {0}",obj ["playMachine"]);
				InsObj.SetActive (true);
			}
		}
	}

//UGUI

	//閉じるボタン
	public void OnClick_CloseBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title)));
	}
	//戻るボタン
	public void OnClick_BackBtn(){
		SetUpPopup.SetActive (true);
		StatusPopup.SetActive (false);
		HeaderCtr ();
		HeaderText.text = "";
	}
	//チーム参加ボタン
	public void OnClick_SingInBtn(){
		Input_Coop_InputPopup.Open (Ctr_Coop_InputPopup.Type.TeamNumber);
	}
	//チーム作成ボタン
	public void OnClick_CreateBtn(){
		PostCreateTeam ();
	}
	//参戦ボタン
	public void OnClicked_PushTeamNumberBtn(){
		ConnectingCanvasMng.Start_Connecting ();
		className = "Team" + number;
		ConnectingCanvasMng.Start_Connecting ();
		NCMBQuery<NCMBObject> TeamQuery = new NCMBQuery<NCMBObject> (className);
		TeamQuery.CountAsync ((int count, NCMBException excep) => {
			if (excep != null) {
				//検索失敗時の処理
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
				ConnectingCanvasMng.Stop_Connecting ();
			} else {
				//検索成功時の処理
				if (count != 0) {
					//既にチーム内に自分がいるかどうか
					NCMBQuery<NCMBObject> TeamQuery2 = new NCMBQuery<NCMBObject> (className);
					TeamQuery2.WhereEqualTo ("player", Data_User.User_Name);
					TeamQuery2.FindAsync ((List<NCMBObject> objList2, NCMBException e) => {
						if (e == null) {
							if (objList2.Count == 0) {
								//初参戦
								NCMBObject NewTeamObj = new NCMBObject (className);
								NewTeamObj.Add ("player", Data_User.User_Name);
								NewTeamObj.Add ("playMachine", "なし");
								NewTeamObj.Add ("usedMoney", 0);
								NewTeamObj.Add ("haveMedal", 0);
								NewTeamObj.Add ("haveBall", 0);
								NewTeamObj.SaveAsync ((NCMBException Save_Ex) => {
									if (Save_Ex != null) {
										//エラー処理
										MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
										ConnectingCanvasMng.Stop_Connecting ();
									} else {
										//成功時の処理
										TeamQuery.FindAsync ((List<NCMBObject> objList, NCMBException Find_Ex) => {
											if (Find_Ex != null) {
												MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
												ConnectingCanvasMng.Stop_Connecting ();
											} else {
												//ヘッダーとPopupを切り替え
												SetUpPopup.SetActive (false);
												StatusPopup.SetActive (true);
												HeaderCtr ();
												HeaderText.text = className;
												//コンテンツ作成
												CreatePlayer (objList);
												Input_Coop_InputPopup.Close ();
												ConnectingCanvasMng.Stop_Connecting ();
											}
										});
									}
								});
							}else{
								//自分が既にいる
								TeamQuery.FindAsync ((List<NCMBObject> objList, NCMBException Find_Ex) => {
									if (Find_Ex != null) {
										MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
										ConnectingCanvasMng.Stop_Connecting ();
									} else {
										//ヘッダーとPopupを切り替え
										SetUpPopup.SetActive (false);
										StatusPopup.SetActive (true);
										HeaderCtr ();
										HeaderText.text = className;
										//コンテンツ作成
										CreatePlayer (objList);
										Input_Coop_InputPopup.Close ();
										ConnectingCanvasMng.Stop_Connecting ();
									}
								});
							}
						}else{
							MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
							ConnectingCanvasMng.Stop_Connecting ();
						}
					});
				}else{
					//検索失敗時の処理
					MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "入力されてチームナンバーが\n存在していません");
					ConnectingCanvasMng.Stop_Connecting ();
				}
			}
		});
	}
}
