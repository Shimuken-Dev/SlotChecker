using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Manager_SubCanvas : MonoBehaviour {

	public enum Sub{
		Login,
		News
	}

	GameObject PrefabObj;

	string 
	Path_Format = "Prefab/Sub/{0}",
	Prefab_Name;


/**パブリック関数**/
	//サブを開く(生成)
	public void OpenSub(Sub popup){
		//Sceneをアクティブに
		SceneManager.SetActiveScene (SceneManager.GetSceneByName ("ViewScene"));
		Prefab_Name = popup.ToString ();
		PrefabObj = Resources.Load (string.Format (Path_Format, popup)) as GameObject;
		CreateSub ();
	}
	//サブを閉じる
	public void CloseSub (bool AllDeth, RectTransform ObjRectTrns = null){
		if (AllDeth == true) {
			foreach (Transform n in gameObject.transform) {
				if (n.name != "DontTouch") {
					PopupAnimation (false, n.GetComponent<RectTransform> ());
				} else {
					Destroy (n.gameObject);
				}
			}
		} else {
			if(gameObject.transform.childCount <=2){
				Destroy (gameObject.transform.Find ("DontTouch").gameObject);
			}
			PopupAnimation (false, ObjRectTrns);
		}
	}
/**関数**/
	//コンテンツ作成
	void CreateSub(){
		//黒背景設定
		CheckDontTouchImg ();
		//プレハブ生成
		GameObject InstansObj = Instantiate (PrefabObj) as GameObject;
		InstansObj.transform.SetParent (gameObject.transform, false);
		InstansObj.transform.localScale = Vector3.zero;
		InstansObj.name = Prefab_Name;
		InstansObj.SetActive (true);
		PopupAnimation (true, InstansObj.GetComponent<RectTransform>());
	}
	//Popupアニメーション関数
	void PopupAnimation(bool flg,RectTransform ThisPopup){
		//Open
		if(flg == true){
			ThisPopup.DOScale (Vector3.one, 0.5f)
			         .SetEase (Ease.OutBack);
		}
		//Close
		else{
			ThisPopup.DOScale (Vector3.zero, 0.5f)
			         .SetEase (Ease.InBack)
			         .OnComplete (() => {
					// アニメーションが終了時によばれる
					Destroy_Popup (ThisPopup.gameObject);
				});
		}
	}
	//Popup削除
	void Destroy_Popup(GameObject Obj){
		Destroy (Obj);
	}
	//黒背景設定
	void CheckDontTouchImg(){
		if (gameObject.transform.Find ("DontTouch") == false) {
			CreateDontTouchObj ();
		}
	}
	//黒背景生成
	void CreateDontTouchObj(){
		//GameObject生成
		GameObject DontTouchObj = new GameObject ("DontTouch");
		DontTouchObj.transform.SetParent (gameObject.transform);
		DontTouchObj.transform.localScale = Vector3.one;
		DontTouchObj.transform.localPosition = Vector3.zero;
		//Imageコンポーネント付与
		Image DontTouchImg = DontTouchObj.AddComponent<Image> ();
		DontTouchImg.color = new Color (0f, 0f, 0f, 200f / 255f);
		//RectTransformコンポーネント設定
		RectTransform DontTouchRect = DontTouchObj.GetComponent<RectTransform> ();
		DontTouchRect.sizeDelta = new Vector2 (750f, 1334f);
	}
}