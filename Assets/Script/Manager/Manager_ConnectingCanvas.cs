using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_ConnectingCanvas : MonoBehaviour {

	GameObject Connecting_obj;
	Text Connecting_text;
	bool Flg_Animation;


	void Start(){
		Initialize ();
	}


//パブリック関数
	//通信開始
	public void Start_Connecting(){
		Connecting_obj.SetActive (true);
		Flg_Animation = true;
		StartCoroutine (Animation_Text ());
	}
	//通信完了
	public void Stop_Connecting(){
		Flg_Animation = false;
		Connecting_obj.SetActive (false);
	}


//関数
	//初期化
	void Initialize(){
		Flg_Animation = false;
		Connecting_obj = gameObject.transform.Find ("ConnectingImg").gameObject;
		Connecting_text = Connecting_obj.transform.Find ("Text").GetComponent<Text> ();
	}
	//コネクトアニメーション
	IEnumerator Animation_Text (){
		while(Flg_Animation){
			switch(Connecting_text.text){
			case "通信中":
				Connecting_text.text = "通信中.";
			break;
			case "通信中.":
				Connecting_text.text = "通信中..";
				break;
			case "通信中..":
				Connecting_text.text = "通信中...";
				break;
			case "通信中...":
				Connecting_text.text = "通信中";
				break;
			}
			yield return new WaitForSeconds (1f);
		}
		yield break;
	}
}