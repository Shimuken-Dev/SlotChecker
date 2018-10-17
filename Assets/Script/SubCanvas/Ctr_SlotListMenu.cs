using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctr_SlotListMenu : MonoBehaviour {

	static public string ChoiceMachine;

	Manager_SubCanvas SubCanvasMng;


	void Awake (){
		Initialize ();
	}

//関数
	void Initialize(){
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		Debug.Log ("選択中の名前 : "+ChoiceMachine);
	}

//UGUI

	//閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
	//設定判別
	public void OnClick_JudgeSetting(){
		
	}
	//実践予報
	public void OnClick_PracticeForecast(){
		
	}
	//台情報
	public void OnClick_MachineInfo(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.MachineInfo);
	}
	//しむけんボタン
	public void OnClick_ShimukenBtn(){
		
	}
}