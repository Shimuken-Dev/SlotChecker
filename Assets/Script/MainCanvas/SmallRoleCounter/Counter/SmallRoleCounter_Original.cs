using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallRoleCounter_Original : MonoBehaviour {


	[SerializeField]
	Text Text_Mode;

	[SerializeField]
	InputField GameCnt_Input;

	[SerializeField]
	Text GameCnt_MathText;

	[SerializeField]
	Transform ContentTrns;

	float GameCntNumber;

	bool Mode;
	// falseが +
	// trueが -


//関数
	void AllMath(){
		foreach(Transform child in ContentTrns){
			int Number = int.Parse (child.transform.Find ("Texts/InputField").GetComponent<InputField> ().text);
			if(Number != 0){
				float Math = GameCntNumber / Number;
				child.transform.Find("Texts/MathText").GetComponent<Text> ().text = string.Format("1/{0}",Math);
			}else{
				child.transform.Find ("Texts/MathText").GetComponent<Text> ().text = "1/0";
			}
		}
	}


//UGUI
	//モード切り替えボタン
	public void OnClick_ChangeMode(){
		if(Mode == false){
			Mode = true;
			Text_Mode.text = "-";
		}else{
			Mode = false;
			Text_Mode.text = "+";
		}
	}
	//小役数
	public void OnEditEnd_InputFeild(GameObject Obj){
		InputField input = Obj.transform.Find ("Texts/InputField").GetComponent<InputField> ();
		Text MathText = Obj.transform.Find ("Texts/MathText").GetComponent<Text> ();
		int Number = int.Parse (input.text);

		input.text = Number.ToString ();
		if (Number != 0) {
			float Math = GameCntNumber / Number;
			MathText.text = string.Format ("1/{0}", Math);
		}else{
			MathText.text = "1/0";
		}
	}
	//小役数ボタン
	public void OnClick_CountBtn(GameObject Obj){
		InputField input = Obj.transform.Find ("Texts/InputField").GetComponent<InputField> ();
		Text MathText = Obj.transform.Find ("Texts/MathText").GetComponent<Text> ();
		int Number = int.Parse(input.text);

		if (Mode == false) {
			Number += 1;
		} else {
			Number -= 1;
		}

		input.text = Number.ToString ();
		if (Number != 0) {
			float Math = GameCntNumber / Number;
			MathText.text = string.Format ("1/{0}", Math);
		} else {
			MathText.text = "1/0";
		}
	}
	//ゲーム数ボタン
	public void OnClick_GameCntBtn(){
		if (Mode == false) {
			GameCntNumber += 1;
		} else {
			GameCntNumber -= 1;
		}

		GameCnt_Input.text = GameCntNumber.ToString ();
		//全部の小役確率更新
		AllMath ();
	}
	//ゲーム数入力
	public void OnEditEnd_GameCnt(string text){
		GameCntNumber = int.Parse (text);
		//全部の小役確率更新
		AllMath ();
	}
}