using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class Manager_Game : MonoBehaviour {

	Manager_FadeCanvas 	FadeCanvasMng;
	Manager_MainCanvas 	MainCanvasMng;
	Manager_SubCanvas 	SubCanvasMng;


/**スタンダード関数**/
	void Start (){
		//PlayerPrefs.DeleteAll ();
		//ログアウト
		NCMBUser.LogOutAsync ();
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
		MainCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_MainCanvas> ();
	}
	//最初のフェードアウト完了コールバック
	void CheckLogin(){
		MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title);
	}
}
