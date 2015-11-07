using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += Vector3.left * Constant.bgspeed * Time.deltaTime;
		if (!Constant.spacelimit.Contains (new Vector2 (this.transform.position.x, this.transform.position.y)))
			Destroy (this.gameObject);
	}
}
