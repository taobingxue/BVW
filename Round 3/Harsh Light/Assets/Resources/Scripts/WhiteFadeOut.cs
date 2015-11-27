using UnityEngine;
using System.Collections;

public class WhiteFadeOut : MonoBehaviour {
	
	public float 		waitTime = 8;
	// public int			levelToLoad = 2;
	public float		fadeOutTime = 1f;
	public float		fadeSpeed = 0.01f;

	GameObject 	fadeOutObject;

	// Use this for initialization
	void Start () {
		fadeOutObject = GameObject.Find ("white");
	}

	public void StartFadeOut(){
		StartCoroutine(CoFadeOut());
	}
	
	IEnumerator CoFadeOut() {
		yield return new WaitForSeconds(waitTime);

		Color colorAlpha = Color.white; 
		for (float rest_time = fadeOutTime; rest_time > 0; rest_time -= fadeSpeed) {
			colorAlpha.a = 1 - rest_time / Constant.fadeouttime;
			fadeOutObject.GetComponent<SpriteRenderer>().color = colorAlpha;
			yield return new WaitForSeconds(Constant.timestep);
		}
		
		Application.LoadLevel (Constant.SCENE_VICTORY);
	}
}
