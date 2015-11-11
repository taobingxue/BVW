using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraAnimator : MonoBehaviour {

	public Transform target;
	public float camSpeed = 5f;
	public int targetIndex = 1;

	private Transform[] targetArray;
	private static CameraAnimator instance = null;
	// Use this for initialization
	void Start () {
		targetArray = target.GetComponentsInChildren<Transform> ();
		this.transform.position = targetArray [targetIndex].position;
		instance = this;
	}

	public static CameraAnimator getInstance() {
		return instance;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			changeIndex();
		}
		transform.position = Vector3.Lerp (this.transform.position,targetArray[targetIndex].position,Time.deltaTime * camSpeed);
	}

	public void changeIndex(){
		targetIndex++;
	}
}
