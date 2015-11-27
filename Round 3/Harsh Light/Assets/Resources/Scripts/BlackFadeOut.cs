using UnityEngine;
using System.Collections;

public class BlackFadeOut : MonoBehaviour {
	
	public float 		waitTime = 1;
	public float		fadeOutWaitTime = 1;
	public int			levelToLoad = 2;
	public float		fadeOutTime = 1f;
	public float		fadeSpeed = 0.01f;
	public GameObject 	fadeInObject;
	public GameObject	fadeOutObject;

	// Use this for initialization
	void Start () {

	}

	public void StartFadeOut(){
		StartCoroutine(CoFadeOut());
	}
	
	IEnumerator CoFadeOut() {
		yield return new WaitForSeconds(waitTime);

		Color colorAlpha = Color.black; 
		for (float rest_time = fadeOutTime; rest_time > 0; rest_time -= fadeSpeed) {
			colorAlpha.a = 1 - rest_time / fadeOutTime;
			fadeInObject.GetComponent<SpriteRenderer>().color = colorAlpha;
			yield return new WaitForSeconds(Constant.timestep);
		}

		yield return new WaitForSeconds(fadeOutWaitTime);
		
		colorAlpha = Color.black; 
		for (float rest_time = 0; rest_time < fadeOutTime; rest_time += fadeSpeed) {
			colorAlpha.a = rest_time / fadeOutTime;
			fadeOutObject.GetComponent<SpriteRenderer>().color = colorAlpha;
			yield return new WaitForSeconds(Constant.timestep);
		}

		if(levelToLoad > -1){
			Application.LoadLevel (levelToLoad);
		}
	}
}
