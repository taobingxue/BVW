using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class title_main_controller : MonoBehaviour {
	public AudioSource titleAS;
	GameObject startroach;
	void Start () {
		startroach = GameObject.Find("startroach");
		titleAS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	// push
	public void push(float x, float y) {
		Vector3 pos3 = startroach.transform.position;
		Vector2 pos2 = new Vector2 (pos3.x, pos3.y);
		Vector2 pos = new Vector2 (x, y);
		Vector2 vecdis = pos2 - pos;
		float dis = vecdis.magnitude;
		if (dis < 13) StartCoroutine("fall");
	}

	IEnumerator fall() {
		titleAS.Play ();
		for (int i = 0; i < 8; i ++) {
			Vector3 vec = startroach.transform.position;
			vec += Vector3.down * 5;
			startroach.transform.position = vec;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		Application.LoadLevel (1);
	}
	
}
