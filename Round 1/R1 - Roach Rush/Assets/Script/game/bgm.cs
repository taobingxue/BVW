using UnityEngine;
using System.Collections;

public class bgm : MonoBehaviour {

	public AudioSource bgmAS;
	// Use this for initialization
	void Start () {
		bgmAS = GetComponent<AudioSource> ();
		bgmAS.Play ();
	}
}
