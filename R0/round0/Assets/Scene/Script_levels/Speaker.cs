using UnityEngine;
using System.Collections;

public class Speaker : MonoBehaviour {

	GameObject speaker, player;
	Vector3 v;
	// Use this for initialization
	void Start () {
		speaker = GameObject.Find("Speak");
		player = GameObject.Find("Sphere");
	}
	
	// Update is called once per frame
	void Update () {
		v = player.transform.position;
		speaker.transform.position = new Vector3 (v.x - 30, 60, v.z + 20);
	}
	
	// speaking
	IEnumerator speaking(string s) {
		speaker.GetComponent<TextMesh>().text = s;
		yield return new WaitForSeconds(3);
		// if havn't been changed
		if (speaker.GetComponent<TextMesh>().text == s) speaker.GetComponent<TextMesh>().text = "";
	}
}
