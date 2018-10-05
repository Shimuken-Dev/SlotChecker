using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallRoleCounter_Default : MonoBehaviour {

	[SerializeField]
	Transform
	MonitorTrns,
	BtnsTrns;

	Button [] Btns;
	InputField [] InputFields;
	Text [] MathTexts;

	int [] Numbers;

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
		} else{
			Mode = false;
		}
	}

	//InputField
	public void OnEndEdit_InputFeild(GameObject Obj){
		int index = Obj.name.LastIndexOf ('_');
		int number = int.Parse(Obj.name.Substring (index+1));
		//変数を更新
		Numbers [number] = int.Parse(InputFields [number].text);
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
	}
}
