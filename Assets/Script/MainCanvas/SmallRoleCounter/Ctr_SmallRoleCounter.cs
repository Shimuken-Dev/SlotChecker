using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctr_SmallRoleCounter : MonoBehaviour {

	[SerializeField]
	Transform
	CounterTrns;

	[SerializeField]
	GameObject 
	BackBtnObj,
	CloseBtnObj;

	Manager_FadeCanvas FadeCanvasMng;
	Manager_MainCanvas MainCanvasMng;

	Ctr_SmallRoleCounter_Popup PopupCtr;

	void Start (){
		MainCanvasMng = GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		PopupCtr = gameObject.transform.Find ("MenuPopup").GetComponent<Ctr_SmallRoleCounter_Popup> ();
		Initialize ();
		StartCoroutine (FadeCanvasMng.FadeOut (() => PopupCtr.Open (Ctr_SmallRoleCounter_Popup.Type.Counter_Select)));
	}

	void OnEnable (){
		Initialize ();
	}

	//関数
	void Initialize(){
		BackBtnObj.SetActive (false);
		CloseBtnObj.SetActive(true);
	}


//UGUI

	//閉じるボタン
	public void OnClick_CloseBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.Title)));
	}
	//戻るボタン
	public void OnClick_BackBtn(){
		if(CounterTrns.childCount != 0){
			foreach(Transform child in CounterTrns){
				Destroy (child.gameObject);
			}
		}
		BackBtnObj.SetActive (false);
		CloseBtnObj.SetActive (true);
		PopupCtr.Open (Ctr_SmallRoleCounter_Popup.Type.Counter_Select);
	}
}
