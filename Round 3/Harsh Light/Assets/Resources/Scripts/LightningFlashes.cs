using UnityEngine;
using System.Collections;

public class LightningFlashes : MonoBehaviour {
	SpriteRenderer view_sprite;
	// Use this for initialization
	void Start () {
		view_sprite = GameObject.Find ("SightView(Clone)").GetComponent<SpriteRenderer> ();
		float flash_time = 93;
		while (flash_time <114) {
			Invoke ("Flash" , flash_time);
			flash_time +=3;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Flash (){
		Debug.Log ("Coroutine Started");
		StartCoroutine ("FlashScene");
	}

	IEnumerator FlashScene (){
		float alpha = 1;
		bool double_flash = false;
		float rand = Random.Range (-1.0f, 1.0f);
		if (rand > 0.5f) {
			double_flash = true;
		}
		while (alpha > 0) {
			alpha -=0.4f;
			view_sprite.material.color = new Color (view_sprite.material.color.r, view_sprite.material.color.g, view_sprite.material.color.b, alpha);
			yield return new WaitForSeconds (0.1f);
		}
		while (alpha < 1) {
			alpha +=0.4f;
			view_sprite.material.color = new Color (view_sprite.material.color.r, view_sprite.material.color.g, view_sprite.material.color.b, alpha);
			yield return new WaitForSeconds (0.1f);
		}
		if (double_flash) {
			while (alpha > 0) {
				alpha -=0.4f;
				view_sprite.material.color = new Color (view_sprite.material.color.r, view_sprite.material.color.g, view_sprite.material.color.b, alpha);
				yield return new WaitForSeconds (0.1f);
			}
			while (alpha < 1) {
				alpha +=0.4f;
				view_sprite.material.color = new Color (view_sprite.material.color.r, view_sprite.material.color.g, view_sprite.material.color.b, alpha);
				yield return new WaitForSeconds (0.1f);
			}
		}
	}
}
