using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_MainCanvas : MonoBehaviour {

	public enum Main{
		Title
	}

	GameObject PrefabObj;

	string 
	Path_Format = "Prefab/Main/{0}",
	Prefab_Name;


/**パブリック関数**/
	//メインを開く(生成)
	public void OpenMain(Main page){
		//Viewシーンをアクティブに
		SceneManager.SetActiveScene (SceneManager.GetSceneByName ("ViewScene"));
		//プレハブ名を取得
		Prefab_Name = page.ToString ();
		//生成オブジェクトを指定
		PrefabObj = Resources.Load (string.Format (Path_Format, page)) as GameObject;
		CreateMain (PrefabObj);
	}
/**関数**/
	//コンテンツ生成
	void CreateMain (GameObject Obj){
		//サブキャンバスのコンテンツを全て削除

		//メインキャンバスのコンテンツを全て削除
		Destory_MainContent ();
		//プレハブ生成
		GameObject InstansObj = Instantiate (Obj) as GameObject;
		InstansObj.transform.SetParent (gameObject.transform, false);
		InstansObj.name = Prefab_Name;
		InstansObj.SetActive (true);
	}
	//メインコンテンツ全削除
	void Destory_MainContent (){
		if (gameObject.transform.childCount != 0) {
			foreach (Transform n in gameObject.transform) {
				Destroy (n.gameObject);
			}
		}
	}
}
