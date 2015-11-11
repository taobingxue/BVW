using UnityEngine;
using System.Collections;

public class AngleCalculator : MonoBehaviour {

	public GameObject gem;
	private Vector2 offset;
	public Vector2 gemRange;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (gem.transform.position.y);
		offset = gem.transform.position;
		if (Mathf.Abs (offset.x) > gemRange.x) {
			offset.x = gemRange.x * Mathf.Sign(offset.x);
		}
		if (Mathf.Abs (offset.y) > gemRange.y) {
			offset.y = gemRange.y * Mathf.Sign(offset.y);
		}
		offset.x = offset.x / gemRange.x;
		offset.y = offset.y / gemRange.y;
		/*
		Vector3 direction = gem.transform.position - this.transform.position;
		angle = Vector3.Angle (direction, Vector3.forward);
		float sign = Mathf.Sign (Vector3.Dot (direction, Vector3.forward));
		Debug.Log (sign*angle);
		if (sign < 0) {
			angle = 90 - angle;
		} else {
			angle += 90;
		}*/
		//Debug.Log (angle);
	}

	public Vector2 getOffset() {
		return offset;
	}
}
