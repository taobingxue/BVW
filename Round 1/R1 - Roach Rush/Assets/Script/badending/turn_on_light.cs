using UnityEngine;
using System.Collections;

public class turn_on_light : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("turnonlight");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator turnonlight() {
		float delta = 100 / 50.0f;
		float deltatime = Constant.endinglightingtime / 50.0f;

		GameObject dirlight = GameObject.Find ("Directional Light");

		for (float ratio = 0; ratio <= 100; ratio += delta) {
			GetComponent<Renderer>().material.color = Color.white * ratio / 100.0f;
			dirlight.GetComponent<Light>().intensity = ratio / 100.0f;
			yield return new WaitForSeconds(deltatime);
		}
	}
}
