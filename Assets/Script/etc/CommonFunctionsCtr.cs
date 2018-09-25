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

	//**** byte配列からスプライトを返す関数 ****//
	public static Sprite CreateSpriteFromBytes (byte [] bytes){
		//横サイズの判定
		int pos = 16;
		int width = 0;
		for (int i = 0; i < 4; i++) {
			width = width * 256 + bytes [pos++];
		}
		//縦サイズの判定
		int height = 0;
		for (int i = 0; i < 4; i++) {
			height = height * 256 + bytes [pos++];
		}

		//byteからTexture2D作成
		Texture2D texture = new Texture2D (width, height);
		texture.LoadImage (bytes);

		return Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
	}

	//**** 文字の出現回数をカウント ****//
	public static int CountChar (string s, char c){
		return s.Length - s.Replace (c.ToString (), "").Length;
	}
}
