using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class tiltle_comic_main_controller : MonoBehaviour {
	GameObject footl, footr, nextroach, uiimage, background;
	public bool playing;
	public AudioClip stepAC;
	bool final;
	
	AudioSource AS;
	AudioClip ac;
	// Use this for initialization
	void Start () {
		AS = this.GetComponent<AudioSource> ();

		footl = GameObject.Find("footL");
		footr = GameObject.Find("footR");
		nextroach = GameObject.Find("nextroach");
		uiimage = GameObject.Find("footinstruction");
		background = GameObject.Find("background");
		
		footl.SetActive(false);
		footr.SetActive(false);
		nextroach.SetActive(false);
		uiimage.SetActive(false);
		final = false;
		ac = ((MovieTexture)background.GetComponent<Renderer> ().material.mainTexture).audioClip;
		AS.clip = ac;
		((MovieTexture)background.GetComponent<Renderer>().material.mainTexture).Play();
		AS.Play ();
		playing = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (((MovieTexture)background.GetComponent<Renderer> ().material.mainTexture).isPlaying) return ;
		// show things
		if (playing) {
			footl.SetActive(true);
			footr.SetActive(true);
			nextroach.SetActive(true);
			uiimage.SetActive(true);
			playing = false;
		}
	}

	public void stepon(float x) {
		if (final) return ;
		float nx = nextroach.transform.position.x;
		// if (x - nx >= -8 && x - nx <= 8) StartCoroutine("finalmove");
		if (x <= -38 && x >= -63) StartCoroutine("finalmove");
	}

	IEnumerator finalmove() {
		final = true;
		AS.clip = stepAC;
		AS.Play ();
		Vector3 r = nextroach.transform.rotation.eulerAngles;
		r.z += 180;
		nextroach.transform.rotation = Quaternion.Euler (r);
		yield return new WaitForSeconds (1.5f);
		Application.LoadLevel (2);
	}
}
