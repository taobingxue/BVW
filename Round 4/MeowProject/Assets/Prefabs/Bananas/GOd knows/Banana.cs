using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour {

	void Start() {
		Destroy (gameObject, 3.0f);
	}
	void OnTriggerEnter2D(Collider2D other) {
		Destroy (gameObject, 1.0f);
	}

	~Banana() {
	
	}

}
