using UnityEngine;
using System.Collections;

public class BoatController : MonoBehaviour {
	bool flag = false;
	GameObject row;
	// Use this for initialization
	void Start () {
		row = GameObject.Find("row");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void play() {
		if (!flag) {
			flag = true;
			StartCoroutine(Shaking ());
		}
	}
	
	public void sStop() {
		flag = false;
	}
	
	IEnumerator Shaking() {
		while (flag) {
			Vector3 vec = this.transform.rotation.eulerAngles;
			for (int i = 0; i < 20; ++i) {
				vec -= Vector3.forward * 2;
				row.transform.rotation = Quaternion.Euler(vec);
				yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
			}
			
			for (int i = 0; i < 20; ++i) {
				vec += Vector3.forward * 2;
				row.transform.rotation = Quaternion.Euler(vec);
				yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
			}
		}
	}
}
