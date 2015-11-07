using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Btn : MonoBehaviour {
	
	Image img;
	Button btn;
	float w, h;
	// Use this for initialization
	void Start () {
		Debug.Log("Start: Scene_level");
		
		GameObject imgObj = GameObject.Find("Image");
		img = imgObj.GetComponent<Image>();
		GameObject btnSObj = GameObject.Find("Button");
		btn = btnSObj.GetComponent<Button>();
		btn.onClick.AddListener(delegate() {this.OnClick(btnSObj); });
		
		
		fitImg();
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
			btn.image.rectTransform.sizeDelta *= r;
			btn.image.rectTransform.anchoredPosition *= r;
		}
	}
	
	// button click
	public void OnClick(GameObject sender) {
		Debug.Log("click");
		Application.LoadLevel (0);
	}

	// Update is called once per frame
	void Update () {

	}
}
