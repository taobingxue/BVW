using UnityEngine;
using System.Collections;

public class title_comic_bg : MonoBehaviour {
	AudioSource AS;
	public AudioClip ac;
	// Use this for initialization
	void Start () {
		AS = GetComponent<AudioSource> ();
		AS.clip = ac;
		AS.loop = true;
		AS.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
