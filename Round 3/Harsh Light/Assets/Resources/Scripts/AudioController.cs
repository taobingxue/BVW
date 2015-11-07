using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	public float wait_to_play =6;
	public AudioSource Audio;
	// Use this for initialization
	void Start () {
		Audio.PlayDelayed (wait_to_play);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
