using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ctr_Omikuzi_God : MonoBehaviour {


	Manager_MainCanvas MainCanvasMng;
	Manager_FadeCanvas FadeCanvasMng;


	void Start(){
		Initialize ();
	}

/** 関数 **/
	void Initialize(){
		MainCanvasMng = gameObject.transform.parent.GetComponent<Manager_MainCanvas> ();
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();

		StartCoroutine (FadeCanvasMng.FadeOut (null));
	}

/** UGUI **/
	//Playボタン
	public void OnClick_PlayBtn(){
		
	}
	//HowToPlayボタン
	public void OnClick_HowToPlayBtn(){
		
	}
	//戻るボタン
	public void OnClick_BackBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.MiniGame)));
	}


}
