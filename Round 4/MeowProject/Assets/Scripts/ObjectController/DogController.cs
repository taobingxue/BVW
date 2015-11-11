using UnityEngine;
using System.Collections;

public class DogController : MonoBehaviour {
	Sprite[] walking_image;
	Sprite[] climbing_image;
	// 0 idle, 1 walking, 2 climbing
	public int flag = 0;
	bool flag_h = false;
	GameObject cat;
	// Use this for initialization
	void Start () {
		walking_image = new Sprite[311];
		walking_image = Resources.LoadAll<Sprite> ("dog_walk");

		climbing_image = new Sprite[2];
		climbing_image = Resources.LoadAll<Sprite> ("dog_climb");

		cat = GameObject.Find("screemcat");
	}
	// Update is called once per frame
	void Update () {
		if (flag_h)
			this.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		else 
			this.GetComponent<SpriteRenderer> ().color = Color.white;
		if (this.transform.localScale.x * (cat.transform.position.x - this.transform.position.x) > 0) {
			Vector3 vec = this.transform.localScale;
			vec.x *= -1;
			this.transform.localScale = vec;
		}
		if (flag == 0) this.GetComponent<SpriteRenderer>().sprite = walking_image[0];
	}
	public void Idle() {
		Debug.Log("idle");
		flag = 0;
	}

	public void Climb() {
		Debug.Log("climb");
		if (flag != 2) {
			flag = 2;
			//sStop ();
			StartCoroutine (Climbing ());
		}
	}

	public void StopClimb() {
		Debug.Log("stop climb");
		flag = 0;
		play ();
	}
	
	public void play() {
		Debug.Log("play");
		if (flag != 1) {
			flag = 1;
			StartCoroutine(Walking ());
		}
	}
	
	public void sStop() {
		Debug.Log("stop");
		flag = 0;
	}
	
	IEnumerator Walking() {
		for (int i = 0; flag == 1; i = (i + 1) % walking_image.Length) {
			this.GetComponent<SpriteRenderer>().sprite = walking_image[i];
			yield return new WaitForSeconds(0.05f);
		}
	}

	IEnumerator Climbing() {
		for (int i = 0; flag == 2; i = (i + 1) % climbing_image.Length) {
			this.GetComponent<SpriteRenderer>().sprite = climbing_image[i];
			yield return new WaitForSeconds(0.04f);
		}
	}
	public void hide() {
		flag_h = true;
	}
	public void show() {
		flag_h = false;
	}
}
