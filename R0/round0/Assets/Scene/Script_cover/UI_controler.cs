using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_controler : MonoBehaviour {
	
	Image img;
	Button btnStart,btnChallenge;
	float w, h;
	// Use this for initialization
	void Start () {
		Debug.Log("Start: Scene_cover");
		
		GameObject imgObj = GameObject.Find("CovImg");
		img = imgObj.GetComponent<Image>();
		GameObject btnSObj = GameObject.Find("btnStart");
		btnStart = btnSObj.GetComponent<Button>();
		GameObject btnCObj = GameObject.Find("btnChallenge");
		btnChallenge = btnCObj.GetComponent<Button>();
		
		fitImg();
		btnStart.onClick.AddListener(delegate() {this.OnClick(btnSObj); });
		btnChallenge.onClick.AddListener(delegate() {this.OnClick(btnCObj); });
	}
	
	// make UI fit the window size
	void fitImg() {
		w = img.rectTransform.sizeDelta.x;
		h = img.rectTransform.sizeDelta.y;
		Debug.Log("w,h: " + w + "," + h);
		
		float height = Screen.height;
		float width = Screen.width;
		Debug.Log("S w,h: " + width + "," + height);
		
		if (height - h > 1 || h - height > 1) {
			float r = height / h;
			// fig cover image
			h *= r; w *= r;
			img.rectTransform.sizeDelta = new Vector2(w, h);
			Debug.Log("N w,h: " + w + "," + h);
			// fit button
			btnStart.image.rectTransform.sizeDelta *= r;
			btnStart.image.rectTransform.anchoredPosition *= r;
			btnChallenge.image.rectTransform.sizeDelta *= r;
			btnChallenge.image.rectTransform.anchoredPosition *= r;
		}
	}
	
	// button click
	public void OnClick(GameObject sender) {
		Debug.Log("click: " + sender.name);
		Constant.flag = 5;
		if (sender.name == "btnStart") {
			Constant.levelnow = Constant.levelstart;
			if (Constant.levelnow == 0) Constant.flag = 0;
		} else Constant.levelnow = Constant.levelMax;
		Application.LoadLevel (1);
	}

	// Update is called once per frame
	void Update () {

	}
}
