using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctr_Title : MonoBehaviour {

	Manager_SubCanvas SubCanvasMng;


	void Start (){
		Initialize ();
	}


	//パブリック関数
	//ログイン完了後に呼ばれる関数
	public void CallBackFinishLogin(){
		//お知らせを開く
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.News);
	}

//UGUI
	//スタートボタン
	public void OnClick_StartBtn(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.Login);
	}

//関数
	//初期化
	void Initialize(){
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
	}
}
