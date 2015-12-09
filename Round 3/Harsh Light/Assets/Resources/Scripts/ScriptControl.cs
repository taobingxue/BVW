using UnityEngine;
using System.Collections;

public class ScriptControl : MonoBehaviour {
	public float waitTime = 8;
	public int   nextScene = 2;
	public string fadein_name;
	public int fadein_length;
	public string fadeout_name;
	public int fadeout_length;
	Sprite sprite;
	// Use this for initialization
	void Awake() {
		//fadein = Resources.LoadAll<Sprite> (fadein_name);
		//fadeout = Resources.LoadAll<Sprite> (fadeout_name);
	}
	void Start () {
		StartCoroutine(fadeout_());
		Sprite a = Resources.Load (fadein_name + "/" + fadein_name + "00", typeof(Sprite)) as Sprite;
		Debug.Log (fadein_name + "/" + fadein_name + "00");
		Debug.Log(a);
	}
	
	IEnumerator fadeout_() {
		GameObject plane = GameObject.Find("fade");
		for (int i = 0; i < fadein_length; ++i) {
			string name = fadein_name;
			if (i < 10) name = name + "0" + i;
			else name = name + i;
			
			plane.GetComponent<SpriteRenderer>().sprite = Resources.Load(fadein_name + "/" + name, typeof(Sprite)) as Sprite;
			yield return new WaitForSeconds(0.01f);
		}

		yield return new WaitForSeconds (waitTime);

		for (int i = 0; i < fadeout_length; ++i) {
			string name = fadeout_name;
			if (i < 10) name = name + "0" + i;
			else name = name + i;

			plane.GetComponent<SpriteRenderer>().sprite = Resources.Load(fadeout_name + "/" + name, typeof(Sprite)) as Sprite;
			yield return new WaitForSeconds(0.01f);
		}
		if (nextScene >= 0) Application.LoadLevel (nextScene);
		/*
		((MovieTexture)GameObject.Find("fadein").GetComponent<Renderer>().material.mainTexture).Play();
		while (((MovieTexture)GameObject.Find("fadein").GetComponent<Renderer>().material.mainTexture).isPlaying)
			yield return new WaitForSeconds(Constant.timestep);

		yield return new WaitForSeconds (waitTime);
		GameObject.Find ("fadein").SetActive (false);

		((MovieTexture)GameObject.Find("fadeout").GetComponent<Renderer>().material.mainTexture).Play();
		while (((MovieTexture)GameObject.Find("fadeout").GetComponent<Renderer>().material.mainTexture).isPlaying)
			yield return new WaitForSeconds(Constant.timestep)
		Color c = Color.black; 
		GameObject blackbg = GameObject.Find("black");
		for (float rest_time = Constant.fadeouttime; rest_time > 0; rest_time -= Constant.timestep) {
			c.a = 1 - rest_time / Constant.fadeouttime;
			blackbg.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.timestep);
		}*/
	}
}
