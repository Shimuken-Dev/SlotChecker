using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_Coop_StatusPopup : MonoBehaviour {

	Transform ContentTrns;

	Text
	Text_NumberOfPeople,    //参加人数
	Text_TotalMoney,        //全投資金額
	Text_PerHeadMoney,      //一人当たりの金額
	Text_TotalMedal,        //全所持メダル
	Text_TotalBall,         //全所持玉数
	Text_Result,            //結果
	Text_ResultTotalMoney,  //トータル差分	
	Text_ResultPerHeadMoney;//一人当たりの差分

	Manager_ConnectingCanvas ConnectingCanvasMng;
	Manager_MessageCanvas MessageCanvasMng;
	Ctr_Coop_InputPopup InputCtr;

	NCMBObject MyInfo_NCMBObj;
	NCMBQuery<NCMBObject> TeamQuery;

	string className;

	int
	AllUsedMoney,
	AllHaveMedal,
	AllHaveBall;
	double 
	PerHeadMoney,
	ResultPerHeadMoney;


	void Awake (){
		ConnectingCanvasMng = GameObject.Find ("ConnectingCanvas").GetComponent<Manager_ConnectingCanvas> ();
		MessageCanvasMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
		ContentTrns = gameObject.transform.Find ("ScrollView/View/Content");
		InputCtr = gameObject.transform.parent.Find ("InputPopup").GetComponent<Ctr_Coop_InputPopup> ();
		//Team Infoのテキスト達
		Text_NumberOfPeople 	= gameObject.transform.Find ("TeamInfo/NumberOfPeople/Text").GetComponent<Text> ();
		Text_TotalMoney 	= gameObject.transform.Find ("TeamInfo/Money/TotalText").GetComponent<Text> ();
		Text_PerHeadMoney 	= gameObject.transform.Find ("TeamInfo/Money/PerHeadText").GetComponent<Text> ();
		Text_TotalMedal 	= gameObject.transform.Find ("TeamInfo/Exchange/TotalMedalText").GetComponent<Text> ();
		Text_TotalBall 		= gameObject.transform.Find ("TeamInfo/Exchange/TotalBallText").GetComponent<Text> ();
		Text_Result 		= gameObject.transform.Find ("TeamInfo/Result/Text").GetComponent<Text> ();
		Text_ResultTotalMoney 	= gameObject.transform.Find ("TeamInfo/Result/TotalMoneyText").GetComponent<Text> ();	
		Text_ResultPerHeadMoney = gameObject.transform.Find ("TeamInfo/Result/PerHeadMoneyText").GetComponent<Text> ();
	}

	void OnEnable(){
		Initialize ();
	}

	void OnDisable(){
		//要素を削除
		foreach (Transform child in ContentTrns) {
			Destroy (child.gameObject);
		}
	}


//関数

	//初期化
	void Initialize(){
		className = "Team"+gameObject.transform.parent.GetComponent<Ctr_Coop> ().number;
		TeamQuery = new NCMBQuery<NCMBObject> (className);
		Reload ();
	}
	//リロード
	void Reload(){
		AllUsedMoney = 0;
		PerHeadMoney = 0;
		AllHaveMedal = 0;
		AllHaveBall = 0;
		ResultPerHeadMoney = 0;

		ConnectingCanvasMng.Start_Connecting ();
		TeamQuery.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
			if (e != null) {
				ConnectingCanvasMng.Stop_Connecting ();
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
			}else{
				ConnectingCanvasMng.Stop_Connecting ();
				foreach(NCMBObject obj in objList){
					GameObject ContentObj = ContentTrns.Find (obj.ObjectId).gameObject;
					//投資金額
					AllUsedMoney += int.Parse(obj ["usedMoney"].ToString());
					ContentObj.transform.Find ("Info/UsedMoneyText").GetComponent<Text> ().text = string.Format ("投資金額: {0}円", obj ["usedMoney"]);
					//持ちメダル数
					AllHaveMedal += int.Parse (obj ["haveMedal"].ToString());
					ContentObj.transform.Find ("Info/HaveInfo/HaveMedalText").GetComponent<Text> ().text = string.Format ("持ちメダル: {0}枚", obj ["haveMedal"]);
					//持ち玉数
					AllHaveBall += int.Parse (obj ["haveBall"].ToString ());
					ContentObj.transform.Find ("Info/HaveInfo/HaveBallText").GetComponent<Text> ().text = string.Format ("持ち玉: {0}玉", obj ["haveBall"]);
					//遊戯中の機種
					ContentObj.transform.Find ("PlayMachineInfo/Text").GetComponent<Text> ().text = string.Format ("遊戯中の機種: {0}", obj ["playMachine"]);
				}
				//チーム戦績更新
				Update_TeamInfo ();
			}
		});
	}

	//チーム成績更新
	void Update_TeamInfo(){
		Text_NumberOfPeople.text = string.Format("参加人数: {0}人",ContentTrns.childCount);

		Text_TotalMoney.text = string.Format ("総投資額: {0}円",AllUsedMoney);

		double val = AllUsedMoney / ContentTrns.childCount;
		PerHeadMoney = Math.Ceiling(val);
		Text_PerHeadMoney.text = string.Format ("一人当たり: {0}円",PerHeadMoney);

		Text_TotalMedal.text = string.Format ("持ちメダル: {0}枚",AllHaveMedal);

		Text_TotalBall.text = string.Format ("持ち玉: {0}玉", AllHaveBall);

		int Money_Medal = 20 * AllHaveMedal;
		int Money_Ball = 4 * AllHaveBall;
		int Money = (Money_Ball + Money_Medal) - AllUsedMoney;
		double val2 = Money / ContentTrns.childCount;
		ResultPerHeadMoney = Math.Ceiling (val2);
		if(Money < 0){
			Text_Result.text = "<color=blue>負けてるよ</color>";
			Text_ResultTotalMoney.text = string.Format ("トータル {0}円", Money);
			Text_ResultPerHeadMoney.text = string.Format ("一人当たり {0}円",ResultPerHeadMoney);
		}else{
			Text_Result.text = "<color=red>勝ってるよ</color>";
			Text_ResultTotalMoney.text = string.Format ("トータル +{0}円", Money);
			Text_ResultPerHeadMoney.text = string.Format ("一人当たり +{0}円",ResultPerHeadMoney);
		}
	}

//UGUI

	//リロードボタン
	public void OnClick_ReloadBtn(){
		Reload ();
	}
	//総投資金額更新ボタン
	public void OnClick_Update_UsedMoneyBtn(){
		InputCtr.Open (Ctr_Coop_InputPopup.Type.UsedMoney);
	}
	//遊戯中機種更新ボタン
	public void OnClick_Update_ChoiceMachineBtn (){
		InputCtr.Open (Ctr_Coop_InputPopup.Type.ChoiceMachine);
	}
	//所持メダル枚数更新ボタン
	public void OnClick_Update_HaveMedalBtn (){
		InputCtr.Open (Ctr_Coop_InputPopup.Type.HaveMedal);
	}
	//所持玉数更新ボタン
	public void OnClick_Update_HaveBallBtn (){
		InputCtr.Open (Ctr_Coop_InputPopup.Type.HaveBall);
	}

}
