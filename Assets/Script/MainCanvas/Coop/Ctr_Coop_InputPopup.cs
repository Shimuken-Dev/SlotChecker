using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NCMB;

public class Ctr_Coop_InputPopup : MonoBehaviour {

	public enum Type{
		TeamNumber,
		UsedMoney,
		HaveMedal,
		HaveBall,
		ChoiceMachine
	}

	enum InputType{
		UsedMoney,
		HaveMedal,
		HaveBall,
		ChoiceMachine
	}
	InputType input_type;

	[SerializeField]
	InputField input;

	[SerializeField]
	Button Btn;

	[SerializeField]
	Text 
	Text_Description, 
	Text_Btn;

	Manager_ConnectingCanvas ConnectingCanvasMng;
	Manager_MessageCanvas MessageCanvasMng;
	Ctr_Coop CoopCtr;
	Ctr_Coop_StatusPopup StatusPopupCtr;

	NCMBObject MyObj;

	int input_int;
	string input_str;


	void Start (){
		CoopCtr = gameObject.transform.parent.GetComponent<Ctr_Coop> ();
		StatusPopupCtr = gameObject.transform.parent.Find ("StatusPopup").GetComponent<Ctr_Coop_StatusPopup> ();
		ConnectingCanvasMng = GameObject.Find ("ConnectingCanvas").GetComponent<Manager_ConnectingCanvas> ();
		MessageCanvasMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
		Initialized ();
	}


	//関数
	void Initialized (){
		Text_Description.text = "";
		input.text = "";
		Text_Btn.text = "";
		input.onEndEdit.RemoveAllListeners ();
		Btn.onClick.RemoveAllListeners ();
	}

	//開く
	public void Open(Type type){
		switch(type){
		case Type.TeamNumber:
			Text_Description.text = "参加したいチームのナンバーを\n入力して下さい";
			Text_Btn.text = "参加";
			input.onEndEdit.AddListener (OnEndEdit_TeamNumber);
			Btn.onClick.AddListener (OnClick_SingInBtn);
			input.characterLimit = 4;
			input.contentType = InputField.ContentType.IntegerNumber;
			break;
		case Type.UsedMoney:
			Text_Description.text = "使用した金額を\n入力して下さい";
			Text_Btn.text = "送信";
			input.onEndEdit.AddListener (OnEndEdit_UserdMoney);
			Btn.onClick.AddListener (OnClick_PostBtn);
			input.characterLimit = 6;
			input.contentType = InputField.ContentType.IntegerNumber;
			break;
		case Type.HaveMedal:
			Text_Description.text = "所持メダル枚数を\n入力して下さい";
			Text_Btn.text = "送信";
			input.onEndEdit.AddListener (OnEndEdit_HaveMedal);
			Btn.onClick.AddListener (OnClick_PostBtn);
			input.characterLimit = 5;
			input.contentType = InputField.ContentType.IntegerNumber;
			break;
		case Type.HaveBall:
			Text_Description.text = "所持玉数を\n入力して下さい";
			Text_Btn.text = "送信";
			input.onEndEdit.AddListener (OnEndEdit_HaveBall);
			Btn.onClick.AddListener (OnClick_PostBtn);
			input.characterLimit = 6;
			input.contentType = InputField.ContentType.IntegerNumber;
			break;
		case Type.ChoiceMachine:
			Text_Description.text = "遊戯している機種名を\n入力して下さい";
			Text_Btn.text = "送信";
			input.onEndEdit.AddListener (OnEndEdit_ChoiceMachine);
			Btn.onClick.AddListener (OnClick_PostBtn);
			input.characterLimit = 0;
			input.contentType = InputField.ContentType.Standard;
			input.lineType = InputField.LineType.MultiLineNewline;
			break;
		}
		PopupAnimation (true, gameObject.GetComponent<RectTransform> ());
	}
	//閉じる
	public void Close(){
		PopupAnimation (false, gameObject.GetComponent<RectTransform> ());
	}

//UGUI

//インプット
	//チームナンバー入力後検知
	void OnEndEdit_TeamNumber (string text){
		gameObject.transform.parent.GetComponent<Ctr_Coop>().number = text;
	}
	//総投資金額入力検知
	void OnEndEdit_UserdMoney(string text){
		input_type = InputType.UsedMoney;
		input_int = int.Parse (text);
	}
	//所持メダル枚数入力検知
	void OnEndEdit_HaveMedal(string text){
		input_type = InputType.HaveMedal;
		input_int = int.Parse (text);
	}
	//所持玉数入力検知
	void OnEndEdit_HaveBall (string text){
		input_type = InputType.HaveBall;
		input_int = int.Parse (text);
	}
	//遊戯機種入力検知
	void OnEndEdit_ChoiceMachine (string text){
		input_type = InputType.ChoiceMachine;
		input_str = text;
	}
//ボタン
	//チームナンバー入力後ボタン
	void OnClick_SingInBtn(){
		CoopCtr.OnClicked_PushTeamNumberBtn ();
	}
	//送信ボタン
	void OnClick_PostBtn(){
		ConnectingCanvasMng.Start_Connecting ();
		//自分のクラスフィードを取得
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Team" + CoopCtr.number);
		query.WhereEqualTo ("player", Data_User.User_Name);
		query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
			//検索成功したら
			if (e == null) {
				MyObj = objList [0];
				//要素に応じて更新
				switch (input_type) {
				case InputType.UsedMoney:
					MyObj ["usedMoney"] = input_int;
					break;
				case InputType.HaveMedal:
					MyObj ["haveMedal"] = input_int;
					break;
				case InputType.HaveBall:
					MyObj ["haveBall"] = input_int;
					break;
				case InputType.ChoiceMachine:
					MyObj ["playMachine"] = input_str;
					break;
				}
				MyObj.SaveAsync ((NCMBException excep) => {
					if (excep != null) {
						//エラー処理
						ConnectingCanvasMng.Stop_Connecting ();
						MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
						Close ();
					} else {
						//成功時の処理
						ConnectingCanvasMng.Stop_Connecting ();
						StatusPopupCtr.OnClick_ReloadBtn ();
						Close ();
					}
				});
			}else{
				ConnectingCanvasMng.Stop_Connecting ();
				MessageCanvasMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
				Close ();
			}
		});
	}

	//Popupアニメーション関数
	void PopupAnimation (bool flg, RectTransform ThisPopup){
		//Open
		if (flg == true) {
			ThisPopup.DOScale (Vector3.one, 0.5f)
				 .SetEase (Ease.OutBack);
		}
		//Close
		else {
			ThisPopup.DOScale (Vector3.zero, 0.5f)
				 .SetEase (Ease.InBack)
				 .OnComplete (() => {
					 // アニメーションが終了時によばれる
					 Initialized ();
				 });
		}
	}
}
