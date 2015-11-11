using UnityEngine;
using System.Collections;

public class FadeEffectTrigger : MonoBehaviour {

	// Use this for initialization
	int index = -1;
	private static FadeEffectTrigger instance;

	void Start () {
	
	}
	 public static FadeEffectTrigger getInstance() {
		return instance;
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void FadeToWhite(){
		GetComponent<Animator> ().SetInteger ("effectIndex", 0);
	}

	public void FadeToTransparent() {
		GetComponent<Animator> ().SetInteger ("effectIndex", 1);
	}
	public void resetIndex() {
		GetComponent<Animator> ().SetInteger ("effectIndex", -1);
	}
}
