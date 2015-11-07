using UnityEngine;
using System.Collections;

public class ScriptControl : MonoBehaviour {

	public float waitTime = 8;
	public int   nextScene = 2;
	// Use this for initialization
	void Start () {
		StartCoroutine(fadeout());
	}
	
	IEnumerator fadeout() {
		((MovieTexture)GameObject.Find("fadein").GetComponent<Renderer>().material.mainTexture).Play();
		while (((MovieTexture)GameObject.Find("fadein").GetComponent<Renderer>().material.mainTexture).isPlaying)
			yield return new WaitForSeconds(Constant.timestep);

		yield return new WaitForSeconds (waitTime);
		GameObject.Find ("fadein").SetActive (false);

		((MovieTexture)GameObject.Find("fadeout").GetComponent<Renderer>().material.mainTexture).Play();
		while (((MovieTexture)GameObject.Find("fadeout").GetComponent<Renderer>().material.mainTexture).isPlaying)
			yield return new WaitForSeconds(Constant.timestep);
		/*
		Color c = Color.black; 
		GameObject blackbg = GameObject.Find("black");
		for (float rest_time = Constant.fadeouttime; rest_time > 0; rest_time -= Constant.timestep) {
			c.a = 1 - rest_time / Constant.fadeouttime;
			blackbg.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.timestep);
		}*/
		
		if (nextScene >= 0) Application.LoadLevel (nextScene);
	}
}
