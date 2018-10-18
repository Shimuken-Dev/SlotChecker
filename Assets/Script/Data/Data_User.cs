using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_User {
	public static string
	Device_ID,
	User_Name,
	Mail_Address,
	Pass_Word,
	Tester,
	Choice_Machine;

}

public class Load_Data_User : MonoBehaviour{
	public static void Load(){
		//デバイス ID
		if (PlayerPrefs.HasKey ("deviceId") == true) {
			Data_User.Device_ID = PlayerPrefs.GetString ("deviceId");
		}else{
			Data_User.Device_ID = SystemInfo.deviceUniqueIdentifier;
		}
		//ユーザーネーム
		if (PlayerPrefs.HasKey ("userName") == true) {
			Data_User.User_Name = PlayerPrefs.GetString ("userName");
		} else {
			Data_User.User_Name = "";
		}
		//メールアドレス
		if (PlayerPrefs.HasKey ("mailAddress") == true) {
			Data_User.Mail_Address = PlayerPrefs.GetString ("mailAddress");
		} else {
			Data_User.Mail_Address = "";
		}
		//パスワード
		if (PlayerPrefs.HasKey ("passWord") == true) {
			Data_User.Pass_Word = PlayerPrefs.GetString ("passWord");
		} else {
			Data_User.Pass_Word = "";
		}
	}
}

public class Save_Data_User : MonoBehaviour {
	public static void Save(){
		PlayerPrefs.SetString ("deviceId", Data_User.Device_ID);
		PlayerPrefs.SetString ("userName", Data_User.User_Name);
		PlayerPrefs.SetString ("mailAddress", Data_User.Mail_Address);
		PlayerPrefs.SetString ("passWord", Data_User.Pass_Word);
		PlayerPrefs.Save ();
	}
}
