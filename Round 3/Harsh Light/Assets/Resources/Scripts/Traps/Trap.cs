using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	GameObject hand, light;
	Color handcolor, lightcolor;
	Vector3 handpos, lightpos;
	// position, direction, scale
	Vector3 pos, dir, sca;
	// times
	float appeart, growt, handt, keept, fadet, yend, ydelt;
	// moving with the background
	bool flag_moving = false;

	EdgeCollider2D _edgeCollider;

	// Use this for initialization
	void Awake () {
		// find son objs
		foreach (Transform trans in this.transform) {
			if (trans.gameObject.name == "hand")
				hand = trans.gameObject;
			else
				light = trans.gameObject;
			Debug.Log("name" + trans.gameObject.name);
		}
		// set transform
		handcolor = hand.GetComponent<SpriteRenderer> ().color;
		handcolor.a = 0;
		hand.GetComponent<SpriteRenderer> ().color = handcolor;

		lightcolor = light.GetComponent<SpriteRenderer> ().color;
		lightcolor.a = 0;
		light.GetComponent<SpriteRenderer> ().color = lightcolor;

		_edgeCollider = hand.GetComponent<EdgeCollider2D>();
	}
	
	// initialize location, direction, scale, and start appear
	public void init(float angle, float xstart, float ystart, float yend_, bool moving, float appeartime, float growtime, float handtime, float keeptime, float fadetime, float depth) {
		// rotate
		this.transform.rotation = Quaternion.Euler (0, 0, angle);
		// set parameters
		angle *= Mathf.PI / 180.0f;
		appeart = appeartime; growt = growtime; handt = handtime; keept = keeptime; fadet = fadetime;
		flag_moving = moving;
		yend = yend_; ydelt = Mathf.Abs(yend_ - ystart);
		// scale
		float ratio = 1.0f / Mathf.Cos (angle);
		// set position && direction
		pos = new Vector3 (xstart, ystart, depth);
		this.transform.position = pos;

		dir = (new Vector3 (Mathf.Sin (angle), 0 - Mathf.Cos (angle), 0)).normalized * ratio;
		// set scale
		this.transform.localScale *= Mathf.Abs(ratio);
		sca = light.transform.localScale;
		Vector3 newsca = sca; newsca.x *= Trap_Constant.grow_startratio;
		light.transform.localScale = newsca;
		// set pos
		handpos = hand.transform.localPosition;
		lightpos = light.transform.localPosition;

		// appear
		StartCoroutine ("showtrap");
	}

	// showtrap
	IEnumerator showtrap() {
		float time_sum = appeart + growt;
		// light appear
		for (float rest_time = appeart; rest_time > 0; rest_time -= Constant.timestep) {
			float ratio_appear = 1 - rest_time / appeart;
			float ratio_color = 1 - (rest_time + growt) / time_sum;
			// set color
			lightcolor.a = ratio_color;
			light.GetComponent<SpriteRenderer> ().color = lightcolor;
			// move
			light.transform.localPosition = lightpos + Vector3.down * ratio_appear * ydelt; // + Vector3.forward * light.transform.localPosition.z;

			if (flag_moving) 
				this.transform.position += Vector3.left * Constant.bgspeed * Constant.timestep;
			// wait
			yield return new WaitForSeconds(Constant.timestep);
		}

		// light grow
		for (float rest_time = growt; rest_time > 0; rest_time -= Constant.timestep) {
			float ratio_grow = 1 - rest_time / growt;
			float ratio_color = 1 - rest_time / time_sum;
			// set color
			lightcolor.a = ratio_color;
			light.GetComponent<SpriteRenderer> ().color = lightcolor;
			// scale
			Vector3 newsca = sca;
			newsca.x *= ratio_grow * (1 - Trap_Constant.grow_startratio) + Trap_Constant.grow_startratio;
			light.transform.localScale = newsca;

			if (flag_moving) 
				this.transform.position += Vector3.left * Constant.bgspeed * Constant.timestep;
			// wait
			yield return new WaitForSeconds(Constant.timestep);
		}

		// show hand
		for (float rest_time = handt; rest_time > 0; rest_time -= Constant.timestep) {
			float ratio = 1 - rest_time / handt;
			// set color
			handcolor.a = ratio;
			hand.GetComponent<SpriteRenderer> ().color = handcolor;
			// move
			hand.transform.localPosition = handpos + Vector3.down * ratio * ydelt; // + Vector3.forward * hand.transform.localPosition.z;

			if (flag_moving) 
				this.transform.position += Vector3.left * Constant.bgspeed * Constant.timestep;
			// wait
			yield return new WaitForSeconds(Constant.timestep);
		}

		// keep
		if (flag_moving)
			for (float rest_time = keept; rest_time > 0; rest_time -= Constant.timestep) {
				this.transform.position += Vector3.left * Constant.bgspeed * Constant.timestep;
				// wait
				yield return new WaitForSeconds(Constant.timestep);
			}
		else
			yield return new WaitForSeconds (keept);

		if(_edgeCollider != null){
			_edgeCollider.enabled = false;
		}

		// fadeout
		for (float rest_time = fadet; rest_time > 0; rest_time -= Constant.timestep) {
			float ratio = rest_time / fadet;
			// set color
			handcolor.a = ratio;
			hand.GetComponent<SpriteRenderer> ().color = handcolor;
			lightcolor.a = ratio;
			light.GetComponent<SpriteRenderer> ().color = lightcolor;

			if (flag_moving) 
				this.transform.position += Vector3.left * Constant.bgspeed * Constant.timestep;
			// wait
			yield return new WaitForSeconds(Constant.timestep);
		}

		Destroy (this.gameObject);
	}
}
