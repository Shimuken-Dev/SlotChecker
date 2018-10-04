using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ctr_Coop_InputPopup : MonoBehaviour {

	public enum Type{
		TeamNumber,
		UsedMoney,
		HaveMedal,
		HaveBall,
		ChoiceMachine
	}

	[SerializeField]
	InputField input;

	[SerializeField]
	Button Btn;

	[SerializeField]
	Text 
	Text_Description, 
	Text_Btn;

	Ctr_Coop CoopCtr;


	void Start (){
		CoopCtr = gameObject.transform.parent.GetComponent<Ctr_Coop> ();
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
		
	}
	//所持メダル枚数入力検知
	void OnEndEdit_HaveMedal(string text){
		
	}
	//所持玉数入力検知
	void OnEndEdit_HaveBall (string text){

	}
	//遊戯機種入力検知
	void OnEndEdit_ChoiceMachine (string text){

	}
//ボタン
	//チームナンバー入力後ボタン
	void OnClick_SingInBtn(){
		CoopCtr.OnClicked_PushTeamNumberBtn ();
	}
	//送信ボタン
	void OnClick_PostBtn(){
		
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
