using UnityEngine;
using System.Collections;

public class dancingwoman : MonoBehaviour {
	public Sprite[] woman;
	// Use this for initialization
	void Start () {
		StartCoroutine("anim");
	}

	void OnEnable() {
//Debug.Log ("enalbe");
		StartCoroutine("anim");
	}

	IEnumerator anim() {
		while (true) {
//			Debug.Log ("dancing");
			for (int i = 0; i < 4; i++) {
				this.GetComponent<SpriteRenderer>().sprite = woman[i];
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
}
