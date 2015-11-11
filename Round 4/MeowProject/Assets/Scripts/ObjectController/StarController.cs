using UnityEngine;
using System.Collections;

public class StarController : MonoBehaviour {
	public Sprite[] zZZ;
	public Sprite[] killing;
	public Sprite wake;

	public GameObject[] dieShrimps;
	public GameObject Bubbles, ZZZ;
	// Use this for initialization
	void Start () {
		zZZ = new Sprite[32];
		zZZ = Resources.LoadAll<Sprite> ("zzz");
		ZZZ = this.transform.Find ("ZZZ").gameObject;
		killing = new Sprite[8];
		killing = Resources.LoadAll<Sprite> ("catfork_animation");
		StartCoroutine("Sparkle");
	}

	void Update() {
		if (Input.GetKey (KeyCode.D)) 
			fall ();
	}
	
	public void fall() {
		ZZZ.SetActive(false);
		this.GetComponent<SpriteRenderer> ().sprite = wake;

		StopCoroutine("Sparkle");
		StartCoroutine("Rotate");
		StartCoroutine("falling");
		this.transform.parent = null;
	}

	IEnumerator Sparkle() {
		while (true) {
			for (int i = 0; i < zZZ.Length; ++i) {
				ZZZ.GetComponent<SpriteRenderer>().sprite = zZZ[i];
				yield return new WaitForSeconds(0.03f);
			}
		}
	}

	IEnumerator Rotate() {
		while (true) {
			Vector3 vec = this.transform.rotation.eulerAngles;
			vec.z += 33;
			this.transform.rotation = Quaternion.Euler( vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
	}

	IEnumerator falling() {
		// count goa
		Vector3 goa = Vector3.zero;
		int ss = 0;
		for (int i = 0; i < dieShrimps.Length; ++i) {
			if (dieShrimps[i].activeSelf) {
				goa += dieShrimps[i].transform.position;
				ss ++;
			}
		}
		if (ss != 0) goa *= 1.0f / ss;
		// fall
		for (float rest_time = Constant.starFallingTime; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			float tratio = rest_time / Constant.starFallingTime;
			Vector3 vec = this.transform.position;
			this.transform.position += (goa - vec) * (Constant.TIMESTEPJUMP / rest_time);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		StopCoroutine("Rotate");
		StartCoroutine("kill");
		// sound, smoke
		for (int i = 0; i < dieShrimps.Length; ++i) dieShrimps[i].GetComponent<ShrimpController>().die();
		Vector3 vecc = this.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
		GameObject bubble = Instantiate(Bubbles);
		bubble.transform.position = vecc;
	}

	IEnumerator kill() {
		while (true) {
			for (int i = 0; i < killing.Length; ++i) {
				this.GetComponent<SpriteRenderer>().sprite = killing[i];
				yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
			}
		}
	}
}
