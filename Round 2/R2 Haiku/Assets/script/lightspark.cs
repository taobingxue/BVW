using UnityEngine;
using System.Collections;

public class lightspark : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("spark");
	}

	IEnumerator spark() {
		Light thislight = this.GetComponent<Light> ();
		while (true) {
			for (float r = 1; r >= 0; r -= 0.1f) {
				thislight.color = Color.red * r;
				thislight.intensity = r * 5;
				yield return new WaitForSeconds(0.1f);
			}
			for (float r = 0; r <= 1; r += 0.1f) {
				thislight.color = Color.red * r;
				thislight.intensity = r * 5;
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
