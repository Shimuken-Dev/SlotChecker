using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NCMB;

public class Manager_Game : MonoBehaviour {

	Manager_FadeCanvas 	FadeCanvasMng;
	Manager_MainCanvas 	MainCanvasMng;
	Manager_SubCanvas 	SubCanvasMng;


/**スタンダード関数**/
	void Awake (){
		//シーンを展開
		StartCoroutine (LoadScene_View ());
	}
	IEnumerator LoadScene_View (){
		yield return SceneManager.LoadSceneAsync ("ViewScene", LoadSceneMode.Additive);
		StartCoroutine (LoadScene_Common ());
	}
	IEnumerator LoadScene_Common (){
		yield return SceneManager.LoadSceneAsync ("CommonScene", LoadSceneMode.Additive);
		Scene_Load_Fnish ();
	}

	void Scene_Load_Fnish(){
		//PlayerPrefs.DeleteAll ();
		//ログアウト
		NCMBUser.LogOutAsync ();
		//PlayerPrefsをロード
		Load_Data_User.Load ();
		Initialize ();
		MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title);
		StartCoroutine (FadeCanvasMng.FadeOut (CheckLogin));
	}


/**関数**/
	//初期化
	void Initialize(){
		FadeCanvasMng 	= GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		SubCanvasMng 	= GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		MainCanvasMng 	= GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
	}
	//最初のフェードアウト完了コールバック
	void CheckLogin(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.Login);
	}
}
