using UnityEngine;
using System.Collections;

public class particlesorting : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<ParticleSystemRenderer> ().sortingOrder = 2;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnBecameInvisible()
	{
	     Destroy(this.gameObject);
	}
}
