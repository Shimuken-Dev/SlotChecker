using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_Coop_StatusPopup : MonoBehaviour {

	Transform ContentTrns;

	Manager_ConnectingCanvas ConnectingCanvasMng;
	Manager_MessageCanvas MessageCanvasMng;
	Ctr_Coop_InputPopup InputCtr;

	NCMBObject MyInfo_NCMBObj;
	NCMBQuery<NCMBObject> TeamQuery;

	string className;



	void Awake (){
		ConnectingCanvasMng = GameObject.Find ("ConnectingCanvas").GetComponent<Manager_ConnectingCanvas> ();
		MessageCanvasMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
		ContentTrns = gameObject.transform.Find ("ScrollView/View/Content");
		InputCtr = gameObject.transform.parent.Find ("InputPopup").GetComponent<Ctr_Coop_InputPopup> ();
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
					ContentObj.transform.Find ("Info/UsedMoneyText").GetComponent<Text> ().text = string.Format ("投資金額: {0}円", obj ["usedMoney"]);
					//持ちメダル数
					ContentObj.transform.Find ("Info/HaveInfo/HaveMedalText").GetComponent<Text> ().text = string.Format ("持ちメダル: {0}枚", obj ["haveMedal"]);
					//持ち玉数
					ContentObj.transform.Find ("Info/HaveInfo/HaveBallText").GetComponent<Text> ().text = string.Format ("持ち玉: {0}玉", obj ["haveBall"]);
					//遊戯中の機種
					ContentObj.transform.Find ("PlayMachineInfo/Text").GetComponent<Text> ().text = string.Format ("遊戯中の機種: {0}", obj ["playMachine"]);
				}

			}
		});
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
