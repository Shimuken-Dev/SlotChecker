using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunctionsCtr : MonoBehaviour {

	//**** stringをUnescapeして返す関数 ****//
	public static string UnEscape (string str){
		if (!string.IsNullOrEmpty (str)) {
			string finish_str = System.Text.RegularExpressions.Regex.Unescape (str);
			return finish_str;
		} else {
			return "Null";
		}
	}
}
