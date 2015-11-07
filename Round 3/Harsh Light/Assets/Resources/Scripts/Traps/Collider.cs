using UnityEngine;
using System.Collections;

public class Collider: MonoBehaviour {

	void OnTriggerEnter2D() {
		Debug.Log("hit");
		EdgeCollider2D collider = this.GetComponent<EdgeCollider2D> ();
		Destroy (collider);
	}
}
