using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Game : MonoBehaviour {

	Manager_FadeCanvas 	FadeCanvasMng;
	Manager_SubCanvas 	SubCanvasMng;


/**スタンダード関数**/
	void Start (){
		//PlayerPrefsをロード
		Load_Data_User.Load ();
		Initialize ();
		StartCoroutine (FadeCanvasMng.FadeOut (CheckLogin));
	}


/**関数**/
	//初期化
	void Initialize(){
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
	}
	//最初のフェードアウト完了コールバック
	void CheckLogin(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.Login);
	}
}
