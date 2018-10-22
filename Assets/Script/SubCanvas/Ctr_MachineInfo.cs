using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class Ctr_MachineInfo : MonoBehaviour {

	[SerializeField]
	GameObject
	Obj_Content_Slot,
	Obj_Content_Pachinko,
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
				if (Json_Info ["type"].str == "Slot") {
					gameObject.transform.Find ("Popup").GetComponent<ScrollRect> ().content = Obj_Content_Slot.GetComponent<RectTransform> ();
					Obj_Content_Slot.SetActive (true);
					Create_Slot ();
				}else if(Json_Info ["type"].str == "Pachinko"){
					gameObject.transform.Find ("Popup").GetComponent<ScrollRect> ().content = Obj_Content_Pachinko.GetComponent<RectTransform> ();
					Obj_Content_Pachinko.SetActive (true);
					Create_Pachinko ();
				}
			}
		});
	}

	//コンテンツ作成
	void Create_Slot(){
		/** 基本情報 **/
		Obj_Content_Slot.transform.Find ("TitleBox/Text").GetComponent<Text>().text = 					Json_Info ["main_info"] ["machine_name"].str;		//タイトル
		Obj_Content_Slot.transform.Find ("MainInfo/Box/maker/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["main_info"] ["maker"].str;			//メーカー
		Obj_Content_Slot.transform.Find ("MainInfo/Box/release_day/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["main_info"] ["release_day"].str;		//導入日
		Obj_Content_Slot.transform.Find ("MainInfo/Box/number_introduction/Detail/Text").GetComponent<Text> ().text = 	Json_Info ["main_info"] ["number_introduction"].str;	//販売台数
		Obj_Content_Slot.transform.Find ("MainInfo/Box/machine_type/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["main_info"] ["machine_type"].str;		//タイプ
		/** スペック情報 **/
		//機械割表
		foreach(JSONObject json in Json_Info["spec"]["machine_dividend"].list){
			//生成
			GameObject InsObj = Instantiate (Obj_MachineDividend);
			InsObj.transform.SetParent (Trns_MachineDividend, false);
			InsObj.name = json ["setting"].str;
			//コンテンツ書き換え
			InsObj.transform.Find ("Setting/Text").GetComponent<Text> ().text = 		json ["setting"].str;		//設定
			InsObj.transform.Find ("MachineDividend/Text").GetComponent<Text> ().text = 	json ["normal"].str;            //機械割
			InsObj.transform.Find ("MachineDividend").GetComponent<Image> ().color = 	Color.white;
			InsObj.transform.Find ("CompleteCapture/Text").GetComponent<Text> ().text = 	json ["complete_capture"].str;	//完全攻略
			InsObj.transform.Find ("CompleteCapture").GetComponent<Image> ().color = 	Color.white;
		}
		//ボーナス解析表
		foreach(JSONObject json in Json_Info["spec"]["bonus_probability"].list){
			//生成
			GameObject InsObj = Instantiate (Obj_BonusProbability);
			InsObj.transform.SetParent (Trns_BonusProbability, false);
			InsObj.name = json ["setting"].str;
			//コンテンツ書き換え
			InsObj.transform.Find ("Setting/Text").GetComponent<Text> ().text = 	json ["setting"].str;	//設定
			InsObj.transform.Find ("Big/Text").GetComponent<Text> ().text = 	json ["big"].str;       //ビック確率
			InsObj.transform.Find ("Big").GetComponent<Image> ().color = 		Color.white;
			InsObj.transform.Find ("Reg/Text").GetComponent<Text> ().text = 	json ["reg"].str;	//レギュラー確率
			InsObj.transform.Find ("Reg").GetComponent<Image> ().color = 		Color.white;
			InsObj.transform.Find ("Sum/Text").GetComponent<Text> ().text = 	json ["sum"].str;	//合算確率
			InsObj.transform.Find ("Sum").GetComponent<Image> ().color = 		Color.white;
		}
		//その他の情報
		Obj_Content_Slot.transform.Find ("Spec/EtcBox/rotation_number/Detail/Text").GetComponent<Text>().text = 	Json_Info ["spec"] ["rotation_number"].str;		//回転数
		Obj_Content_Slot.transform.Find ("Spec/EtcBox/big_acquisition_number/Detail/Text").GetComponent<Text> ().text = Json_Info ["spec"] ["big_acquisition_number"].str;	//ビック獲得枚数
		Obj_Content_Slot.transform.Find ("Spec/EtcBox/reg_acquisition_number/Detail/Text").GetComponent<Text> ().text = Json_Info ["spec"] ["reg_acquisition_number"].str;     	//レギュラー獲得枚数
		Obj_Content_Slot.transform.Find ("Spec/EtcBox/net_increase/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["spec"] ["net_increase"].str;                //純増枚数
		/** 天井解析 **/
		Obj_Content_Slot.transform.Find("CeilingSystem/Box/ceiling_system/Detail/Text").GetComponent<Text>().text = Json_Info ["ceiling_system"].str;	//天井情報

		ConnectingCanvasMng.Stop_Connecting ();
	}
	void Create_Pachinko(){
		/** 基本情報 **/
		Obj_Content_Pachinko.transform.Find ("TitleBox/Text").GetComponent<Text> ().text = 					Json_Info ["main_info"] ["machine_name"].str;		//タイトル
		Obj_Content_Pachinko.transform.Find ("MainInfo/Box/maker/Detail/Text").GetComponent<Text> ().text = 			Json_Info ["main_info"] ["maker"].str;                  //メーカー
		Obj_Content_Pachinko.transform.Find ("MainInfo/Box/release_day/Detail/Text").GetComponent<Text> ().text =		Json_Info ["main_info"] ["release_day"].str;            //導入日
		Obj_Content_Pachinko.transform.Find ("MainInfo/Box/number_introduction/Detail/Text").GetComponent<Text> ().text = 	Json_Info ["main_info"] ["number_introduction"].str;    //販売台数
		Obj_Content_Pachinko.transform.Find ("MainInfo/Box/machine_type/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["main_info"] ["machine_type"].str;           //タイプ
		/** スペック情報 **/
		Obj_Content_Pachinko.transform.Find ("Spec/Box/probability_of_hit/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["spec"] ["probability_of_hit"].str;			//大当たり確率
		Obj_Content_Pachinko.transform.Find ("Spec/Box/residual_rush_rate/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["spec"] ["residual_rush_rate"].str;			//確変突入率
		Obj_Content_Pachinko.transform.Find ("Spec/Box/probability_continuation_rate/Detail/Text").GetComponent<Text> ().text = Json_Info ["spec"] ["probability_continuation_rate"].str;	//確変継続率
		Obj_Content_Pachinko.transform.Find ("Spec/Box/number_of_award_balls/Detail/Text").GetComponent<Text> ().text = 	Json_Info ["spec"] ["number_of_award_balls"].str;		//賞球
		Obj_Content_Pachinko.transform.Find ("Spec/Box/round/Detail/Text").GetComponent<Text> ().text = 			Json_Info ["spec"] ["round"].str;				//ラウンド
		Obj_Content_Pachinko.transform.Find ("Spec/Box/count/Detail/Text").GetComponent<Text> ().text = 			Json_Info ["spec"] ["count"].str;				//カウント
		Obj_Content_Pachinko.transform.Find ("Spec/Box/devotion/Detail/Text").GetComponent<Text> ().text = 			Json_Info ["spec"] ["devotion"].str;				//出玉
		Obj_Content_Pachinko.transform.Find ("Spec/Box/time_saving/Detail/Text").GetComponent<Text> ().text = 			Json_Info ["spec"] ["time_saving"].str;				//時短
		/**ボーダー**/
		Obj_Content_Pachinko.transform.Find ("Border/Box/4_exchange/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["border"] ["4_exchange"].str;	//4円交換
		Obj_Content_Pachinko.transform.Find ("Border/Box/3.57_exchange/Detail/Text").GetComponent<Text> ().text = 	Json_Info ["border"] ["3.57_exchange"].str;	//3.57円交換
		Obj_Content_Pachinko.transform.Find ("Border/Box/3.3_exchange/Detail/Text").GetComponent<Text> ().text = 	Json_Info ["border"] ["3.3_exchange"].str;	//3.3円交換
		Obj_Content_Pachinko.transform.Find ("Border/Box/3_exchange/Detail/Text").GetComponent<Text> ().text = 		Json_Info ["border"] ["3_exchange"].str;	//3円交換

		ConnectingCanvasMng.Stop_Connecting ();
	}

//UGUI

	//閉じるボタン
	public void OnClick_CloseBtn(){
		SubCanvasMng.CloseSub (false, gameObject.GetComponent<RectTransform> ());
	}
}
