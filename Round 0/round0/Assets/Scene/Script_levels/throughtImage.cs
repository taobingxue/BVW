using UnityEngine;
using System.Collections;

public class throughtImage : MonoBehaviour, ICanvasRaycastFilter {
	
	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
		return false;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
