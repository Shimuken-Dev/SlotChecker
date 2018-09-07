using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Game : MonoBehaviour {

	Manager_FadeCanvas FadeCanvasMng;
	Manager_SubCanvas SubCanvasMng;

	void Start (){
		FadeCanvasMng = GameObject.Find ("FadeCanvas").GetComponent<Manager_FadeCanvas> ();
		SubCanvasMng = GameObject.Find ("SubCanvas").GetComponent<Manager_SubCanvas> ();
		StartCoroutine (FadeCanvasMng.FadeOut (null));
	}
}
