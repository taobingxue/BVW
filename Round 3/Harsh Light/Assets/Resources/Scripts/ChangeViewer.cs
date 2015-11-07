using UnityEngine;
using System.Collections;

public class ChangeViewer : MonoBehaviour {
	public Sprite whiteviewer;
	// Use this for initialization
	void Start () {
		GameObject.Find ("SightView(Clone)").GetComponent<SpriteRenderer> ().sprite = whiteviewer;
		GameObject.Find ("focuspoint").GetComponent<SpriteRenderer> ().color = new Color(0, 0, 0, 50.0f/255);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
