using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BfSound : MonoBehaviour {
	public AudioClip[] bfDisgustingSFX;
	public AudioSource bfAS;

	public int disgustingRatio;
	private int randomNum;
	private int disgustingRandom;

	// Use this for initialization
	void Start () {
		bfAS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void bfDisgusting(){
		disgustingRandom = Random.Range (0, 100);
		//		Debug.Log (disgustingRandom<=disgustingRatio);
		if (disgustingRandom <= disgustingRatio) {
			randomNum = Random.Range (0, bfDisgustingSFX.Length);
			bfAS.clip = bfDisgustingSFX [randomNum];
			bfAS.volume = 0.8f;
			bfAS.Play();
		}
	}
}
