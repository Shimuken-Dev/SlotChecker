using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallRoleCounter_Original_CreatePopup : MonoBehaviour {

	[SerializeField]
	Image ViewBtn_Img;

	[SerializeField]
	Text ViewBtn_Text;

	[SerializeField]
	InputField Input;

	[SerializeField]
	GameObject 
	BtnTempleteObj,
	ListTempleteObj;

	[SerializeField]
	Transform 
	Content_Trns,
	List_Trns;

//関数
	void Initialize(){
		ViewBtn_Img.color = Color.white;
		OnEditEnd_BtnText ("");
		Input.text = "";
	}

//UGUI
	//ボタン色選択ボタン
	public void OnClick_ColorBtn(Image BtnImg){
		ViewBtn_Img.color = BtnImg.color;
	}
	//ボタン内テキスト
	public void OnEditEnd_BtnText(string text){
		ViewBtn_Text.text = text;
	}
	//リセットボタン
	public void OnClick_ResetBtn(){
		Initialize ();
	}
	//作成ボタン
	public void OnClick_CreateBtn(){
		//ボタン作成
		GameObject BtnObj = Instantiate (BtnTempleteObj);
		BtnObj.transform.SetParent (Content_Trns,false);
		BtnObj.transform.Find ("Btn").GetComponent<Image> ().color = ViewBtn_Img.color;
		if (ViewBtn_Text.text != "") {
			BtnObj.transform.Find ("Btn/Text").GetComponent<Text> ().text = ViewBtn_Text.text;
			BtnObj.name = ViewBtn_Text.text;
		}else{
			BtnObj.name = string.Format ("counter {0}",Content_Trns.childCount);
		}
		BtnObj.SetActive (true);
		//リスト作成
		GameObject ListContentObj = Instantiate (ListTempleteObj);
		ListContentObj.transform.SetParent (List_Trns, false);
		ListContentObj.transform.Find ("NameText").GetComponent<Text> ().text = BtnObj.name;
		ListContentObj.name = BtnObj.name;
		ListContentObj.SetActive (true);
		Initialize ();
	}
	//スタートボタン
	public void OnClick_StartBtn(){
		gameObject.SetActive (false);
	}
	//小役ボタン削除ボタン
	public void OnClick_DeleteBtn(GameObject Obj){
		foreach(Transform child in Content_Trns){
			if(child.name == Obj.name){
				Destroy (child.gameObject);
				Destroy (Obj);
			}
		}
	}
}
