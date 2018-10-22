using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_MachineInfo : MonoBehaviour {

	[SerializeField]
	Text
	Text_Title,
	Text_Maker,
	Text_ReleaseDay,
	Text_NumberIntroduction,
	Text_MachineType,
	Text_RotationNumber,
	Text_BigAcquisitionNumber,
	Text_RegAcquisitionNumber,
	Text_NetIncrease,
	Text_CelingSystem;

	[SerializeField]
	GameObject
	Obj_MachineDividend,
	Obj_BonusProbability;

	[SerializeField]
	Transform
	Trns_MachineDividend,
	Trns_BonusProbability;

	Manager_SubCanvas SubCanvasMng;
	Manager_ConnectingCanvas ConnectingCanvasMng;
	Manager_MessageCanvas MessageMng;

	JSONObject Json_Info;


	void Start (){
		Initialize ();
	}

//関数

	//初期化
	void Initialize(){
		//初期化
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		ConnectingCanvasMng = GameObject.Find ("ConnectingCanvas").GetComponent<Manager_ConnectingCanvas> ();
		MessageMng = GameObject.Find ("MessageCanvas").GetComponent<Manager_MessageCanvas> ();
		ConnectingCanvasMng.Start_Connecting ();
		//コンテンツJSON取得
		NCMBFile file = new NCMBFile (Data_User.Choice_Machine + "_Info.json");
		file.FetchAsync ((byte [] fileData, NCMBException error) => {
			if (error != null) {
				// 失敗
				ConnectingCanvasMng.Stop_Connecting ();
				MessageMng.OpenMessagePopup (Manager_MessageCanvas.PopUpType.Normal, "通信に失敗しました");
				Destroy (gameObject);
			} else {
				// 成功
				Json_Info = JSONObject.Create (CommonFunctionsCtr.BytesToString (fileData));
				Create ();
			}
		});
	}

	//コンテンツ作成
	void Create(){
		/** 基本情報 **/
		Text_Title.text = Json_Info ["main_info"] ["machine_name"].str;				//タイトル
		Text_Maker.text = Json_Info ["main_info"] ["maker"].str;				//メーカー
		Text_ReleaseDay.text = Json_Info ["main_info"] ["release_day"].str;			//導入日
		Text_NumberIntroduction.text = Json_Info ["main_info"] ["number_introduction"].str;	//販売台数
		Text_MachineType.text = Json_Info ["main_info"] ["machine_type"].str;			//タイプ
		/** スペック情報 **/
		//機械割表
		foreach(JSONObject json in Json_Info["spec"]["machine_dividend"].list){
			//生成
			GameObject InsObj = Instantiate (Obj_MachineDividend);
			InsObj.transform.SetParent (Trns_MachineDividend, false);
			InsObj.name = json ["setting"].str;
			//コンテンツ書き換え
			InsObj.transform.Find ("Setting/Text").GetComponent<Text> ().text = json ["setting"].str;			//設定
			InsObj.transform.Find ("MachineDividend/Text").GetComponent<Text> ().text = json ["normal"].str;                //機械割
			InsObj.transform.Find ("MachineDividend").GetComponent<Image> ().color = Color.white;
			InsObj.transform.Find ("CompleteCapture/Text").GetComponent<Text> ().text = json ["complete_capture"].str;	//完全攻略
			InsObj.transform.Find ("CompleteCapture").GetComponent<Image> ().color = Color.white;
		}
		//ボーナス解析表
		foreach(JSONObject json in Json_Info["spec"]["bonus_probability"].list){
			//生成
			GameObject InsObj = Instantiate (Obj_BonusProbability);
			InsObj.transform.SetParent (Trns_BonusProbability, false);
			InsObj.name = json ["setting"].str;
			//コンテンツ書き換え
			InsObj.transform.Find ("Setting/Text").GetComponent<Text> ().text = json ["setting"].str;	//設定
			InsObj.transform.Find ("Big/Text").GetComponent<Text> ().text = json ["big"].str;               //ビック確率
			InsObj.transform.Find ("Big").GetComponent<Image> ().color = Color.white;
			InsObj.transform.Find ("Reg/Text").GetComponent<Text> ().text = json ["reg"].str;		//レギュラー確率
			InsObj.transform.Find ("Reg").GetComponent<Image> ().color = Color.white;
			InsObj.transform.Find ("Sum/Text").GetComponent<Text> ().text = json ["sum"].str;		//合算確率
			InsObj.transform.Find ("Sum").GetComponent<Image> ().color = Color.white;
		}
		//その他の情報
		Text_RotationNumber.text = Json_Info ["spec"] ["rotation_number"].str;			//回転数
		Text_BigAcquisitionNumber.text = Json_Info ["spec"] ["big_acquisition_number"].str;	//ビック獲得枚数
		Text_RegAcquisitionNumber.text = Json_Info ["spec"] ["reg_acquisition_number"].str;     //レギュラー獲得枚数
		Text_NetIncrease.text = Json_Info ["spec"] ["net_increase"].str;                        //純増枚数
		/** 天井解析 **/
		Text_CelingSystem.text = Json_Info ["ceiling_system"].str;	//天井情報

		ConnectingCanvasMng.Stop_Connecting ();
	}

//UGUI

	//閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
}
