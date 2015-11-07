using UnityEngine;
using System.Collections;

public class handprint : MonoBehaviour {
	bool flag = true;
	// Use this for initialization
	void Start () {
		this.transform.localPosition = new Vector3 (-0.35f, -0.35f, 0.8f);
		StartCoroutine("moveforward");
	}
	
	// Update is called once per frame
	void Update () {
		//
	}

	IEnumerator moveforward() {
		Vector3 pos = this.transform.localPosition;
		Color c = this.GetComponent<Renderer> ().material.color;
		float a = c.a;
		while (true) {
			for (int i=0; i < 32; i++) {
				this.transform.localPosition += Vector3.forward * 0.02f;
				Color newcolor = c;
				newcolor.a = a * (31 - i) / 32.0f;
				this.GetComponent<Renderer>().material.color = newcolor;
				yield return new WaitForSeconds (0.03f);
			}
			this.transform.localPosition = pos;
		}
	}
}
