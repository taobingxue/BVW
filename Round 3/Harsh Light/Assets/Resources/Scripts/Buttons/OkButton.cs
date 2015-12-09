using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OkButton : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void StartPush () {
		Constant.nameFist = GameObject.Find ("InputField").GetComponent<InputField> ().text;
		if (Constant.nameFist == "") Constant.nameFist = "Harsh Light";
	}
}
