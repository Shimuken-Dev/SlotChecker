using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ctr_SmallRoleCounter_Popup : MonoBehaviour {

	[SerializeField]
	Transform
	CounterTrns;

	[SerializeField]
	GameObject
	BackBtnObj,
	CloseBtnObj;

	public enum Type{
		Counter_Select
	}


	public void Open(Type type){
		switch(type){
		case Type.Counter_Select:
			PopupAnimation (true);
			break;
		}
	}

	public void Close(){
		PopupAnimation (false);
	}

//関数

	//Popupアニメーション関数
	void PopupAnimation (bool flg){
		//Open
		if (flg == true) {
			gameObject.GetComponent<RectTransform>().DOScale (Vector3.one, 0.5f)
				 .SetEase (Ease.OutBack);
		}
		//Close
		else {
			gameObject.GetComponent<RectTransform> ().DOScale (Vector3.zero, 0.5f)
				 .SetEase (Ease.InBack)
				 .OnComplete (() => {
					 // アニメーションが終了時によばれる
				 });
		}
	}

//UGUI

	//通常カウンターを使用するボタン
	public void OnClick_TemplateBtn (){
		GameObject Counter = Instantiate (Resources.Load ("Prefab/SmallRoleCounter/Default") as GameObject);
		Counter.transform.SetParent (CounterTrns, false);
		Close ();
		BackBtnObj.SetActive (true);
		CloseBtnObj.SetActive (false);
	}
	//オリジナルカウンター作成ボタン
	public void OnClick_OriginalBtn (){
		GameObject Counter = Instantiate (Resources.Load ("Prefab/SmallRoleCounter/Original") as GameObject);
		Counter.transform.SetParent (CounterTrns, false);
		Close ();
		BackBtnObj.SetActive (true);
		CloseBtnObj.SetActive (false);
	}
}
