using UnityEngine;
using System.Collections;
using System.Threading;

public class story_control : MonoBehaviour {

	GameObject[] speaker, s;
	Vector3[] v;
	// Use this for initialization
	void Start () {
		Debug.Log("start");
		speaker = new GameObject[2];
		s = new GameObject[2];
		v = new Vector3[2];
		speaker[0] = GameObject.Find("speak_P");
		speaker[1] = GameObject.Find("speak_D");
		s[0] = GameObject.Find("Sphere_P");
		s[1] = GameObject.Find("Sphere_D");
		v[0] = s[0].transform.position;
		v[1] = s[1].transform.position;
		StartCoroutine( final());
	}
	
	void speak(int p, string word) {
		speaker[1-p].GetComponent<TextMesh>().text = "";
		s[1-p].GetComponent<Animation>().Stop();
		s[1-p].transform.position = v[1-p];
		speaker[p].GetComponent<TextMesh>().text = word;
		s[p].GetComponent<Animation>().Play();
	}

	IEnumerator final() {
		speak(0, "Home Home !!");
		yield return new WaitForSeconds(2.1f);
		speak(0, "Dad !!");
		yield return new WaitForSeconds(2.1f);
		speak(1, "!!!");
		yield return new WaitForSeconds(2.1f);
		speak(1, "Where ???");
		yield return new WaitForSeconds(2.1f);
		speak(0, "#@*& !!");
		yield return new WaitForSeconds(2.1f);
		speak(0, "Maze !!");
		yield return new WaitForSeconds(2.1f);
		speak(0, "%&^~ !!");
		yield return new WaitForSeconds(2.1f);
		speak(1, "......");
		yield return new WaitForSeconds(2.1f);
		speak(1, "%d$, #%u&o@sv.");
		yield return new WaitForSeconds(2.1f);
		speak(1, "Challenge !!");
		yield return new WaitForSeconds(2.1f);
		speak(0, "!!!");
		yield return new WaitForSeconds(2.1f);
		speak(0, "Challenge ??");
		yield return new WaitForSeconds(2.1f);
		speak(1, "Right ~!");
		yield return new WaitForSeconds(2.1f);
		speak(0, "Challenge ~!");
		yield return new WaitForSeconds(6.0f);
		Application.LoadLevel (0);
	}
	// Update is called once per frame
	void Update () {

	}				
}
