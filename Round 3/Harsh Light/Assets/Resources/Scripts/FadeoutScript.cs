using UnityEngine;
using System.Collections;

public class FadeoutScript : MonoBehaviour {

	public float waitTime = 8;
	// Use this for initialization
	void Start () {
		StartCoroutine(fadeout());
	}
	
	IEnumerator fadeout() {
		yield return new WaitForSeconds(waitTime);
		
		Color c = Color.black; 
		GameObject blackbg = GameObject.Find("black");
		for (float rest_time = Constant.fadeouttime; rest_time > 0; rest_time -= Constant.timestep) {
			c.a = 1 - rest_time / Constant.fadeouttime;
			blackbg.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.timestep);
		}
		
		Application.LoadLevel (2);
	}
}
