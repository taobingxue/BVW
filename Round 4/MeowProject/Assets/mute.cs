using UnityEngine;
using System.Collections;

public class mute : MonoBehaviour {
	AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<SoundManager>().bgmSource;
	
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetKeyDown(KeyCode.M))
		{
			if (audio.mute)
				audio.mute = false;
			else
				audio.mute = true;
		}
	}
}
