using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctr_SlotListMenu : MonoBehaviour {


	Manager_SubCanvas SubCanvasMng;


	void Awake (){
		Initialize ();
	}

	//関数
	void Initialize(){
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
	}

//UGUI
	//閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
}