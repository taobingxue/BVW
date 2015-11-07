using UnityEngine;
using System.Collections;

public class fading2D : MonoBehaviour {
	Color mycolor;
	float delt;
	bool flagin = false, flagout = false;

	void Start() {
		Debug.Log ("start");
		mycolor = this.GetComponent<SpriteRenderer> ().color;
		delt = mycolor.a / (Constant.fadingtime2D / 0.02f);
		this.GetComponent<SpriteRenderer> ().color = new Color(mycolor.r, mycolor.g, mycolor.b, 0);
		// StartCoroutine("fadein");
	}

	public void fadein() {
		Debug.Log ("hi in");

		if (flagin)	return ;
		if (flagout) {
			StopCoroutine ("fadeout_");
			flagout = false;
		}
		StartCoroutine("fadein_");
		//this.GetComponent<SpriteRenderer> ().color = mycolor;
		/*
		StopCoroutine("fadein");
		StopCoroutine("fadeout");
		StartCoroutine("fadein");
		*/
	}

	public void fadeout() {
		Debug.Log ("hi out");

		if (flagout) return ;
		if (flagin) {
			StopCoroutine ("fadein_");
			flagin = false;
		}
		StartCoroutine("fadeout_");
	}

	IEnumerator fadein_() {
		flagin = true;
		float anow = this.GetComponent<SpriteRenderer> ().color.a;
		while (true) {
			Color cc = mycolor;
			anow += delt;
			cc.a = anow;
			if (cc.a >= mycolor.a) break;

			this.GetComponent<SpriteRenderer> ().color = cc;
			yield return new WaitForSeconds(0.02f);
		}
		this.GetComponent<SpriteRenderer> ().color = mycolor;
		flagin = false;
	}

	IEnumerator fadeout_() {
		flagout = true;
		float starta = this.GetComponent<SpriteRenderer> ().color.a;
		while (true) {
			Color cc = mycolor;
			starta -= delt;
			cc.a = starta;
			if (cc.a <= 0 ) break;

			this.GetComponent<SpriteRenderer> ().color = cc;
			yield return new WaitForSeconds(0.02f);
		}
		this.GetComponent<SpriteRenderer> ().color = new Color(mycolor.r, mycolor.g, mycolor.b, 0);
		flagout = false;
	}
}
