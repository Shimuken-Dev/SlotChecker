using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Manager_MessageCanvas : MonoBehaviour {

	public enum PopUpType{
		Normal
	}

	[SerializeField]
	GameObject
	PopupObj,
	DontTouchObj;

	[SerializeField]
	Text Message_Text;

	[SerializeField]
	Button Popup_Btn;

	RectTransform PopupRectTrns;
	Text Button_Text;

	void Awake (){
		PopupRectTrns = PopupObj.GetComponent<RectTransform> ();
		Button_Text = Popup_Btn.transform.Find ("Text").GetComponent<Text> ();
	}


	//ポップアップ表示
	public void OpenMessagePopup(PopUpType type,string message){
		switch(type){
		case PopUpType.Normal:
			Message_Text.text = message;
			Popup_Btn.onClick.AddListener (OnClick_CloseBtn);
			Button_Text.text = "閉じる";
			break;
		}
		PopupAnimation (true);
	}

//関数
	//Popupアニメーション関数
	void PopupAnimation (bool flg){
		//Open
		if (flg == true) {
			DontTouchObj.SetActive (true);
			PopupObj.SetActive (true);
			PopupRectTrns.DOScale (
				Vector3.one, 0.5f)
			             .SetEase (Ease.OutBack);
		}
		//Close
		else {
			PopupRectTrns.DOScale (
				Vector3.zero, 0.5f)
			             .SetEase (Ease.InBack)
			             .OnComplete (() => {
					PopupObj.SetActive (false);
					DontTouchObj.SetActive (false);
					});
		}
	}


//UGUI
	//閉じるボタン
	void OnClick_CloseBtn(){
		PopupAnimation (false);
		Popup_Btn.onClick.RemoveListener (OnClick_CloseBtn);
	}
}
