using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour {
	public enum TrapType
	{
		None = 0,
		Bullet,
		Focus,
		Closing,
	}
	
	float xstart, ystart, yend;

	[SerializeField]
	float angle, reset_time = 2;
	[SerializeField]
	bool moving;
	bool reset = true;
	[SerializeField]
	float depth;
	[SerializeField]
	TrapType type;

	GameObject TrapPrefab;

	// Use this for initialization
	void Start () {
		TrapPrefab = Resources.Load("Prefab/trap") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.parent.transform.position += Vector3.left * Constant.bgspeed * Time.deltaTime;
		if (!Constant.spacelimit.Contains (new Vector2 (this.transform.position.x, this.transform.position.y)))
			Destroy (this.gameObject);
	}

	GameObject generate(float angle, float xstart, float ystart, float yend=0, float depth = 0, bool moving = false) {
		GameObject newtrap = Instantiate (TrapPrefab);
		newtrap.GetComponent<Trap>().init (angle, xstart, ystart, yend, moving, Trap_Constant.appear_time, Trap_Constant.grow_time, Trap_Constant.hand_time, Trap_Constant.keep_time, Trap_Constant.fade_time, depth);

		Invoke ("ResetTrigger", reset_time);

		return newtrap;
	}

	GameObject generate_focus() {
		//Find Fairy
		GameObject Fairy = GameObject.Find ("Fairy");
		// angle
		float localangle = Quaternion.LookRotation(Fairy.transform.position- transform.position).z;
		// up or downs
		ystart = Trap_Constant.y_max;
		if (Fairy.transform.position.x < this.transform.position.x) {
			localangle +=20;
		}
		else {
			localangle -=20;
		}
		if (Random.Range (-1, 1) < 0) {
			localangle = 180 - localangle;
			ystart = Trap_Constant.y_min;
		}
		xstart = Fairy.transform.position.x;

		Invoke ("ResetTrigger", reset_time);

		return generate (localangle, xstart, ystart, Trap_Constant.y_norm);
	}

	void ResetTrigger(){
		reset = true;
	}

	void ExecuteTrigger(){
		if (reset) {
			Debug.Log ("Trigger Hit");
			xstart = this.transform.parent.transform.position.x;
			ystart = this.transform.parent.transform.position.y;
			if (type == TrapType.Bullet)
				generate (angle, xstart, ystart, yend, depth, moving);
			else if (type == TrapType.Focus)
				generate_focus ();
			reset = false;
		}
	}
}
