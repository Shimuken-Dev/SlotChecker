using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ctr_Setting : MonoBehaviour {

	[SerializeField]
	Text
	UserName_Text,
	MailAddress_Text;

	Manager_SubCanvas SubCanvasMng;

	void Start(){
		Initialized ();
	}


//関数
	//初期化
	void Initialized (){
		UserName_Text.text = Data_User.User_Name;
		MailAddress_Text.text = Data_User.Mail_Address;
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
	}


//UGUI
	//閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
}
