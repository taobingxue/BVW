using UnityEngine;
using System.Collections;

public class WaterStrech : MonoBehaviour {

	public GameObject top;
	private GameObject bottom;
	// Use this for initialization
	void Start () {
		bottom = transform.parent.gameObject;	
	}
	
	// Update is called once per frame
	void Update () {
		float yOffset = Mathf.Abs(bottom.transform.position.y - top.transform.position.y);
		Vector3 _scale = this.transform.localScale;
		_scale.y = yOffset * 0.75f;
		this.transform.localScale = _scale;

		Vector3 pos = this.transform.position;
		pos.y = bottom.transform.position.y +(top.transform.position.y - bottom.transform.position.y) / 2f;
		this.transform.position = pos;

	}
}
