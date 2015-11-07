using UnityEngine;
using System.Collections;

public class bgm : MonoBehaviour {
	public AudioClip ac;

	// Use this for initialization
	void Start () {
		AudioSource audioS = this.GetComponent<AudioSource> ();
		audioS.clip = ac;
		audioS.Play ();
	}

	void OnAwake() {
		AudioSource audioS = this.GetComponent<AudioSource> ();
		audioS.clip = ac;
		audioS.Play ();
	}

	void OnDisable() {
		AudioSource audioS = this.GetComponent<AudioSource> ();
		audioS.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Fade out audio instead of disable
	public void FadeOutAudio() {
		StartCoroutine(FadeAudio());
	}
	IEnumerator FadeAudio(){
		AudioSource src = this.GetComponent<AudioSource> ();
		float vol = src.volume;
		while (vol >= 0)
		{
			vol -=  0.05f;
			//Debug.Log("Volume: "+vol);
			src.volume = vol;
			yield return new WaitForSeconds(0.1f);
		}
		src.Stop ();
		src.volume = 1;
		this.gameObject.SetActive (false);
	}
}
