using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_JudgeSetting : MonoBehaviour {

	[SerializeField]
	GameObject Obj_Templete;

	[SerializeField]
	Transform Trns_Content;

	[SerializeField]
	Text Text_MachineTitle;

	Manager_MainCanvas MainCanvasMng;
	Manager_SubCanvas SubCanvasMng;
	Manager_FadeCanvas FadeCanvasMng;
	Manager_MessageCanvas MessageMng;

	JSONObject Json_JudgeSetting;

	void Start (){
		Initialize ();
	}

//関数

	//初期化
	void Initialize(){
		//取得
		MainCanvasMng = GameObject.Find ("MainCanvas").GetComponent<Manager_MainCanvas> ();
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		MessageMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
		//コンテンツJSONを取得
		NCMBFile file = new NCMBFile (Data_User.Choice_Machine + "_JudgeSetting.json");
		file.FetchAsync ((byte [] fileData, NCMBException error) => {
			if (error != null) {
				// 失敗
				MessageMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
				MainCanvasMng.OpenMain (Manager_MainCanvas.Main.SlotList);
			} else {
				// 成功
				Json_JudgeSetting = JSONObject.Create (CommonFunctionsCtr.BytesToString (fileData));
				Create ();
			}
		});
	}

	//コンテンツ作成
	void Create (){
		//タイトル設定
		Text_MachineTitle.text = Json_JudgeSetting ["data"] ["name"].str;
		foreach( JSONObject Json_InputContent in Json_JudgeSetting ["input_content"].list){
			//枠作成
			GameObject ParentObj = Instantiate (Obj_Templete);
			ParentObj.transform.SetParent (Trns_Content, false);
			ParentObj.name = "window";
			//表示更新
			ParentObj.transform.Find ("Bar/Text").GetComponent<Text> ().text = Json_InputContent ["title"].str;
			ParentObj.SetActive (true);
			//入力フォームを格納
			GameObject Ins_ChildObj = ParentObj.transform.Find ("Input").gameObject;
			foreach(JSONObject Json_Content in Json_InputContent["content"].list){
				//入力フォームを作成
				GameObject ChildObj = Instantiate (Ins_ChildObj);
				ChildObj.transform.SetParent (ParentObj.transform, false);
				ChildObj.name = Json_Content ["name"].str;
				//表示更新
				ChildObj.transform.Find ("KeyText").GetComponent<Text> ().text = Json_Content ["key"].str;
				ChildObj.transform.Find ("UnitText").GetComponent<Text> ().text = Json_Content ["unit"].str;
				ChildObj.SetActive (true);
			}
		}

		StartCoroutine(FadeCanvasMng.FadeOut (null));
	}

//UGUI

	//閉じるボタン
	public void OnClick_ClosenBtn(){
		StartCoroutine (FadeCanvasMng.FadeIn (() => MainCanvasMng.OpenMain (Manager_MainCanvas.Main.SlotList)));
	}
	//戻るボタン
	public void OnClick_BackBtn(){
		
	}
	//台情報ボタン
	public void OnClick_InfoBtn(){
		SubCanvasMng.OpenSub (Manager_SubCanvas.Sub.MachineInfo);
	}
	//設定判別ボタン
	public void OnClick_JudgeBtn(){
		
	}
}
