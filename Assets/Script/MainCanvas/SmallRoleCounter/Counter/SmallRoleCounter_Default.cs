using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallRoleCounter_Default : MonoBehaviour {

	[SerializeField]
	GameObject
	PlusTextObj,
	MinusTextObj;

	[SerializeField]
	Transform
	MonitorTrns,
	BtnsTrns;

	[SerializeField]
	InputField Input_GameCnt;

	Button [] Btns;
	InputField [] InputFields;
	Text [] MathTexts;

	int [] Numbers;
	int GameCnt;

	bool Mode;
	// falseが +
	// trueが -


	void Start (){
		Initialize ();
	}


//関数

	//初期化
	void Initialize(){
		Btns = new Button[BtnsTrns.childCount];
		InputFields = new InputField [MonitorTrns.childCount];
		MathTexts = new Text [MonitorTrns.childCount];
		Numbers = new int [MonitorTrns.childCount];
		//Monitorの子供たちを取得
		int i = 0;
		foreach(Transform child in MonitorTrns){
			InputFields [i] = child.transform.Find (string.Format("InputField_{0}",i)).GetComponent<InputField> ();
			MathTexts[i] = child.transform.Find ("MathText/Text").GetComponent<Text> ();
			InputFields [i].text = "0";
			MathTexts [i].text = "0/0";
			i++;
		}
		//Btnsの子供たちを取得
		i = 0;
		foreach(Transform child in BtnsTrns){
			Btns[i] = child.transform.Find (string.Format ("Button_{0}", i)).GetComponent<Button> ();
			i++;
		}
	}

//UGUI

	//モード切り替え
	public void OnClick_ModeChange(){
		if (Mode == false) {
			Mode = true;
			PlusTextObj.SetActive (false);
			MinusTextObj.SetActive (true);
		} else{
			Mode = false;
			PlusTextObj.SetActive (true);
			MinusTextObj.SetActive (false);
		}
	}

	//Game数入力のInputField
	public void OnEndEdit_GameCnt(string text){
		//取得
		GameCnt = int.Parse (text);
		//全小役の確率を更新
		double Average = 0;
		for (int i = 0; i < MathTexts.Length; i++){
			Average = Numbers [i] / GameCnt;
			MathTexts [i].text = string.Format ("1/{0}",Math.Ceiling (Average));
		}
	}
	//Game数入力のボタン
	public void OnClick_GameCnt(){
		if(Mode == false){
			GameCnt += 1;
			Input_GameCnt.text = GameCnt.ToString ();
		}else{
			GameCnt -= 1;
			Input_GameCnt.text = GameCnt.ToString ();
		}
		//全小役の確率更新
		double Average = 0;
		for (int i = 0; i < MathTexts.Length; i++) {
			Average = Numbers [i] / GameCnt;
			MathTexts [i].text = string.Format ("1/{0}", Math.Ceiling (Average));
		}
	}

	//InputField
	public void OnEndEdit_InputFeild(GameObject Obj){
		int index = Obj.name.LastIndexOf ('_');
		int number = int.Parse(Obj.name.Substring (index+1));
		//変数を更新
		Numbers [number] = int.Parse(InputFields [number].text);
		//小役確率を更新
		double Average = Numbers [number] / GameCnt;
		MathTexts[number].text = string.Format("1/{0}", Math.Ceiling (Average));
	}
	//ボタン
	public void OnClick_Button(GameObject Obj){
		int index = Obj.name.LastIndexOf ('_');
		int number = int.Parse (Obj.name.Substring (index + 1));
		//数値を更新
		if(Mode == false){
			Numbers [number] += 1;
			InputFields [number].text = Numbers [number].ToString();
		}else{
			Numbers [number] -= 1;
			InputFields [number].text = Numbers [number].ToString ();
		}
		//小役確率を更新
		double Average = Numbers [number] / GameCnt;
		MathTexts [number].text = string.Format ("1/{0}", Math.Ceiling (Average));
	}
}
