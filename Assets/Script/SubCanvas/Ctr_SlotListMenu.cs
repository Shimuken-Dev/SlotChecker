using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctr_SlotListMenu : MonoBehaviour {

	Manager_MainCanvas MainCanvasMng;
	Manager_SubCanvas SubCanvasMng;
	Manager_FadeCanvas FadeCanvasMng;


	void Awake (){
		Initialize ();
	}

//関数
	void Initialize(){
		MainCanvasMng = GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
	}

//UGUI

	//閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
	//設定判別
	public void OnClick_JudgeSetting(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.JudgeSetting)));
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