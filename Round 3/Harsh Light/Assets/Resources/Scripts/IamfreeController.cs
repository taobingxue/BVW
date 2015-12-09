using UnityEngine;
using System.Collections;

public class IamfreeController : MonoBehaviour {

	public float waitTime = 8;
	public int   nextScene = 2;

	Animator _animatorController;

	// Use this for initialization
	void Awake() {

	}
	void Start () {
		StartCoroutine(fadeout_());
		_animatorController = GetComponent<Animator>();
	}
	
	IEnumerator fadeout_() {
						
		yield return new WaitForSeconds (waitTime);

		_animatorController.SetTrigger("Fadeout");

		yield return new WaitForSeconds(2f);

		if (nextScene >= 0) Application.LoadLevel (nextScene);
	}
}
