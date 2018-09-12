using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctr_Title : MonoBehaviour {
	
	Manager_SubCanvas SubCanvasMng;


	void Start (){
		Initialize ();
	}


//UGUI
	//スタートボタン
	public void OnClick_StartBtn(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.News);
	}

//関数
	//初期化
	void Initialize(){
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
	}
}
