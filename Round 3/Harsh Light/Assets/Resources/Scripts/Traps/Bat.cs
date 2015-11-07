using UnityEngine;
using System.Collections;

public class Bat : MonoBehaviour {
	Vector3 dir;
	float angle;
	public float speed, distance;

	public Sprite[] bats;
	int spriteindex, rest;
	const int step = 3;

	void Awake() {
		speed = 4;
		distance = 1.5f;
		spriteindex = 0;
		rest = step;
	}
	// Use this for initialization
	void Start () {
		Vector3 vec = this.transform.localPosition;
		vec.x = 0; vec.y = 0;
		this.transform.localPosition = vec;
		this.transform.localScale *= 0.3f;

		dir = (new Vector3 (Random.Range (1.0f, 2), Random.Range (1.0f, 2), 0)).normalized;
		// Debug.Log (dir);
		angle = Random.Range (-2.5f, 2.5f) * Mathf.PI / 180;
	}
	
	// Update is called once per frame
	void Update () {
		rest -= 1;
		if (rest == 0) {
			spriteindex = (spriteindex + 1) % 3;
			this.GetComponent<SpriteRenderer> ().sprite = bats [spriteindex];
			rest = step;
		}
		// new direction
		float newx = dir.x * Mathf.Cos (angle) + dir.y * Mathf.Sin (angle);
		float newy = 0 - dir.x * Mathf.Sin (angle) + dir.y * Mathf.Cos (angle);
		dir = (new Vector3 (newx, newy, 0)).normalized;

		Vector3 newpos = this.transform.localPosition + dir * speed * Time.deltaTime;
		Vector3 pos2 = newpos; pos2.z = 0;
		if (pos2.magnitude > distance) {
			// if out of range
			dir = (Vector3.zero - this.transform.localPosition).normalized;
			angle = Random.Range (-2.5f, 2.5f) * Mathf.PI / 180;
			newpos = this.transform.localPosition + dir * speed * Time.deltaTime;
		}
		// Debug.Log (dir);

		// move
		this.transform.localPosition = newpos;
		if (dir.x * this.transform.localScale.x > 0) {
			Vector3 vec = this.transform.localScale;
			vec.x *= -1;
			this.transform.localScale = vec;
		}
	}
}
